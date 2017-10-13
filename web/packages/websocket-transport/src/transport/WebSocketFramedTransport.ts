/**
 * Copyright 2017 Plexus Interop Deutsche Bank AG
 * SPDX-License-Identifier: Apache-2.0
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
import { UniqueId, ConnectableFramedTransport, Frame, InternalMessagesConverter } from "@plexus-interop/transport-common";
import { BlockingQueueBase, BlockingQueue, CancellationToken, Logger, LoggerFactory } from "@plexus-interop/common";

export class WebSocketFramedTransport implements ConnectableFramedTransport {

    private log: Logger;

    private readonly socketOpenToken: CancellationToken = new CancellationToken();

    private connectionEstablishedPromise: Promise<void>;

    private dataFrame: Frame | null;

    private terminateSent: boolean = false;
    public terminateReceived: boolean = false;

    public static TERMINATE_MESSAGE: string = "<END>";
    public static SOCKET_CLOSE_TIMEOUT: number = 5000;
    public static CONNECTING: number = 0;
    public static OPEN: number = 1;
    public static CLOSING: number = 2;
    public static CLOSED: number = 3;

    constructor(
        private readonly socket: WebSocket,
        private readonly guid: UniqueId = UniqueId.generateNew(),
        private inMessagesBuffer: BlockingQueue<Frame> = new BlockingQueueBase<Frame>(),
        private messagesConverter: InternalMessagesConverter = new InternalMessagesConverter()) {
        this.socket.binaryType = "arraybuffer";
        this.log = LoggerFactory.getLogger(`WebSocketFramedTransport [${this.uuid().toString()}]`);
        this.connectionEstablishedPromise = this.createConnectionReadyPromise();
        this.bindToSocketEvents();
        this.log.debug("Created");
    }

    public connectionEstablished(): Promise<void> {
        return this.connectionEstablishedPromise;
    }

    public disconnect(): Promise<void> {
        if (this.socketOpenToken.isCancelled()) {
            this.log.warn("Already disconnected");
            return Promise.resolve();
        }
        if (!this.isSocketClosed()) {
            this.log.warn("Socket is CLOSED, cancelling connection");
            this.cancelConnectionAndCleanUp();
        }
        this.throwIfNotConnectedOrDisconnectRequested();
        return this.closeConnectionInternal();
    }

    public connected(): boolean {
        return this.socket.readyState === WebSocketFramedTransport.OPEN;
    }

    private isSocketClosed(): boolean {
        return this.socket.readyState === WebSocketFramedTransport.CLOSED
            || this.socket.readyState === WebSocketFramedTransport.CLOSING;
    }

    public async readFrame(): Promise<Frame> {
        this.throwIfNotConnectedOrDisconnectRequested();
        return this.inMessagesBuffer.blockingDequeue(this.socketOpenToken);
    }

    public async writeFrame(frame: Frame): Promise<void> {
        this.throwIfNotConnectedOrDisconnectRequested();
        const data: ArrayBuffer = this.messagesConverter.serialize(frame);
        if (this.log.isDebugEnabled()) {
            this.log.debug(`Sending header message of ${data.byteLength} bytes to server`);
        }
        this.socket.send(data);
        if (frame.isDataFrame()) {
            if (this.log.isDebugEnabled()) {
                this.log.debug(`Sending data message of ${frame.body.byteLength} bytes to server`);
            }
            this.socket.send(frame.body);
        } else if (frame.internalHeaderProperties.close) {
            this.sendTerminateMessage();
        }
    }

    private sendTerminateMessage(): void {
        this.log.debug("Sending terminate message");
        this.socket.send(WebSocketFramedTransport.TERMINATE_MESSAGE);
        this.terminateSent = true;
    }

    public getMaxFrameSize(): number {
        return 60000;
    }

    public uuid(): UniqueId {
        return this.guid;
    }

    private createConnectionReadyPromise(): Promise<void> {
        let opened = false;
        return new Promise<void>((resolve, reject) => {
            this.socket.onopen = () => {
                opened = true;
                this.handleOpen();
                resolve();
            };
            this.socket.addEventListener("error", (e) => {
                if (!opened) {
                    reject("Connection error");
                } else {
                    this.log.warn("Connection error", e);
                }
            });
        });
    }

    private bindToSocketEvents(): void {
        this.socket.onmessage = this.handleMessageEvent.bind(this);
        this.socket.onerror = this.handleError.bind(this);
        this.socket.onclose = this.handleCloseEvent.bind(this);
    }

    private async closeConnectionInternal(): Promise<void> {
        this.log.debug("Closing connection");
        this.cancelConnectionAndCleanUp();
        if (this.terminateSent && this.terminateReceived) {
            this.scheduleSocketDisconnect();
        } else if (this.terminateSent) {
            this.log.debug("Server terminate event not received, not closing the socket");
        } else if (this.terminateReceived) {
            this.sendTerminateMessage();
            this.scheduleSocketDisconnect();
        } else {
            this.sendTerminateMessage();
        }
    }

    private scheduleSocketDisconnect(): void {
        this.log.debug("Scheduling socket close action");
        setTimeout(() => {
            try {
                if (this.connected()) {
                    this.log.debug("Closing socket");
                    this.socket.close();
                } else {
                    this.log.debug("Already closed");
                }
            } catch (error) {
                this.log.error("Error on closing the socket", error);
            }
        }, WebSocketFramedTransport.SOCKET_CLOSE_TIMEOUT);
    }

    private cancelConnectionAndCleanUp(reason: string = "Connection closed"): void {
        this.log.debug(`Cancelling connection with reason: ${reason}`);
        this.socketOpenToken.cancel(reason);
        this.inMessagesBuffer.clear();
    }

    private handleCloseEvent(socket: WebSocket, ev: CloseEvent): void {
        this.log.debug("Connection closed event received", ev);
    }

    private handleMessageEvent(ev: MessageEvent): void {
        if (this.log.isDebugEnabled()) {
            this.log.debug("Message event received");
        }
        if (this.isTerminateMessage(ev)) {
            if (this.log.isDebugEnabled()) {
                this.log.debug("Terminate message received");
            }
            this.terminateReceived = true;
        } else if (this.terminateReceived) {
            this.log.warn("Terminate message already received, dropping frame", ev.data);
        } else {
            const data: Uint8Array = this.readEventData(ev);
            if (this.log.isDebugEnabled()) {
                this.log.debug(`Received message with ${data.byteLength} bytes`);
            }
            if (this.dataFrame) {
                this.dataFrame.body = data.buffer;
                this.inMessagesBuffer.enqueue(this.dataFrame);
                this.dataFrame = null;
            } else {
                const frame = this.messagesConverter.deserialize(data);
                if (frame.isDataFrame()) {
                    if (this.log.isDebugEnabled()) {
                        this.log.debug("Message header frame, waiting for data frame");
                    }
                    this.dataFrame = frame;
                } else {
                    this.inMessagesBuffer.enqueue(frame);
                }
            }
        }
    }

    private isTerminateMessage(ev: MessageEvent): boolean {
        return ev.data === WebSocketFramedTransport.TERMINATE_MESSAGE;
    }

    private readEventData(ev: MessageEvent): Uint8Array {
        if (ev.data instanceof Array) {
            return new Uint8Array(ev.data);
        } else if (this.isArrayBuffer(ev.data)) {
            if (this.log.isDebugEnabled()) {
                this.log.debug("Array Buffer message");
            }
            return new Uint8Array(ev.data);
        } else if (this.isArrayBufferView(ev.data)) {
            if (this.log.isDebugEnabled()) {
                this.log.debug("ArrayBufferView Buffer message");
            }
            return new Uint8Array(ev.data);
        } else {
            this.log.error("Unknown payload type", ev.data);
            throw new Error("Unknown payload type");
        }
    }

    private isArrayBuffer(value: any): boolean {
        return value && value.byteLength !== undefined;
    }

    private isArrayBufferView(value: any): boolean {
        return value && value.buffer && value.buffer.byteLength !== undefined;
    }

    private handleError(socket: WebSocket, ev: Event): void {
        this.log.error("Connection error received", ev);
        this.cancelConnectionAndCleanUp("Connection error received");
    }

    private handleOpen(): void {
        this.log.debug("Connection opened");
    }

    private throwIfNotConnected(): void {
        if (!this.connected()) {
            this.log.error(`Web Socket is not connected`);
            throw new Error(`Web Socket is not connected`);
        }
    }

    private throwIfNotConnectedOrDisconnectRequested(): void {
        if (!this.connected() || this.terminateSent) {
            throw new Error("Web Socket is not connected or terminate requested");
        }
    }
}
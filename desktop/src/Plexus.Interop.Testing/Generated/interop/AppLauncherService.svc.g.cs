/**
 * Copyright 2017-2021 Plexus Interop Deutsche Bank AG
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
// <auto-generated>
// 	Generated by the Plexus Interop compiler.  DO NOT EDIT!
// 	source: interop\app_launcher_service.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code
namespace Plexus.Interop.Testing.Generated {
	
	using System;
	using global::Plexus;
	using global::Plexus.Channels;
	using global::Plexus.Interop;
	using global::System.Threading.Tasks;
					
	public static partial class AppLauncherService {
		
		public const string Id = "interop.AppLauncherService";			
		public const string LaunchMethodId = "Launch";
		public const string AppLaunchedEventStreamMethodId = "AppLaunchedEventStream";
		
		public static readonly AppLauncherService.Descriptor DefaultDescriptor = CreateDescriptor();
		
		public static AppLauncherService.Descriptor CreateDescriptor() {
			return new AppLauncherService.Descriptor();
		} 
		
		public static AppLauncherService.Descriptor CreateDescriptor(string alias) {
			return new AppLauncherService.Descriptor(alias);
		}				
	
		public partial interface ILaunchProxy {
			IUnaryMethodCall<global::Plexus.Interop.Testing.Generated.AppLaunchResponse> Launch(global::Plexus.Interop.Testing.Generated.AppLaunchRequest request);
		}
		
		public partial interface IAppLaunchedEventStreamProxy {
			IServerStreamingMethodCall<global::Plexus.Interop.Testing.Generated.AppLaunchedEvent> AppLaunchedEventStream(global::Google.Protobuf.WellKnownTypes.Empty request);
		}
		
		public partial interface ILaunchImpl {
			Task<global::Plexus.Interop.Testing.Generated.AppLaunchResponse> Launch(global::Plexus.Interop.Testing.Generated.AppLaunchRequest request, MethodCallContext context);
		}
		
		public partial interface IAppLaunchedEventStreamImpl {
			Task AppLaunchedEventStream(global::Google.Protobuf.WellKnownTypes.Empty request, IWritableChannel<global::Plexus.Interop.Testing.Generated.AppLaunchedEvent> responseStream, MethodCallContext context);
		}
		
		public sealed partial class Descriptor {
		
			public UnaryMethod<global::Plexus.Interop.Testing.Generated.AppLaunchRequest, global::Plexus.Interop.Testing.Generated.AppLaunchResponse> LaunchMethod {get; private set; }
			public ServerStreamingMethod<global::Google.Protobuf.WellKnownTypes.Empty, global::Plexus.Interop.Testing.Generated.AppLaunchedEvent> AppLaunchedEventStreamMethod {get; private set; }
			
			public Descriptor() {				
				LaunchMethod = Method.Unary<global::Plexus.Interop.Testing.Generated.AppLaunchRequest, global::Plexus.Interop.Testing.Generated.AppLaunchResponse>(Id, LaunchMethodId);
				AppLaunchedEventStreamMethod = Method.ServerStreaming<global::Google.Protobuf.WellKnownTypes.Empty, global::Plexus.Interop.Testing.Generated.AppLaunchedEvent>(Id, AppLaunchedEventStreamMethodId);
			}
		
			public Descriptor(string alias) {
				LaunchMethod = Method.Unary<global::Plexus.Interop.Testing.Generated.AppLaunchRequest, global::Plexus.Interop.Testing.Generated.AppLaunchResponse>(Id, alias, LaunchMethodId);
				AppLaunchedEventStreamMethod = Method.ServerStreaming<global::Google.Protobuf.WellKnownTypes.Empty, global::Plexus.Interop.Testing.Generated.AppLaunchedEvent>(Id, alias, AppLaunchedEventStreamMethodId);
			}
		}
	}
					
}
#endregion Designer generated code

/**
 * Copyright 2017-2018 Plexus Interop Deutsche Bank AG
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
// 	source: plexus\interop\testing\greeting_service.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code
namespace Plexus.Interop.Testing.Generated {
	
	using System;
	using global::Plexus;
	using global::Plexus.Channels;
	using global::Plexus.Interop;
	using global::System.Threading.Tasks;
					
	public static partial class GreetingService {
		
		public const string Id = "plexus.interop.testing.GreetingService";			
		public const string HelloMethodId = "Hello";
		
		public static readonly GreetingService.Descriptor DefaultDescriptor = CreateDescriptor();
		
		public static GreetingService.Descriptor CreateDescriptor() {
			return new GreetingService.Descriptor();
		} 
		
		public static GreetingService.Descriptor CreateDescriptor(string alias) {
			return new GreetingService.Descriptor(alias);
		}				
	
		public partial interface IHelloProxy {
			IUnaryMethodCall<global::Plexus.Interop.Testing.Generated.GreetingResponse> Hello(global::Plexus.Interop.Testing.Generated.GreetingRequest request);
		}
		
		public partial interface IHelloImpl {
			Task<global::Plexus.Interop.Testing.Generated.GreetingResponse> Hello(global::Plexus.Interop.Testing.Generated.GreetingRequest request, MethodCallContext context);
		}
		
		public sealed partial class Descriptor {
		
			public UnaryMethod<global::Plexus.Interop.Testing.Generated.GreetingRequest, global::Plexus.Interop.Testing.Generated.GreetingResponse> HelloMethod {get; private set; }
			
			public Descriptor() {				
				HelloMethod = Method.Unary<global::Plexus.Interop.Testing.Generated.GreetingRequest, global::Plexus.Interop.Testing.Generated.GreetingResponse>(Id, HelloMethodId);
			}
		
			public Descriptor(string alias) {
				HelloMethod = Method.Unary<global::Plexus.Interop.Testing.Generated.GreetingRequest, global::Plexus.Interop.Testing.Generated.GreetingResponse>(Id, alias, HelloMethodId);
			}
		}
	}
					
}
#endregion Designer generated code

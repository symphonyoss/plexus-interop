// <auto-generated>
// 	Generated by the Plexus Interop compiler.  DO NOT EDIT!
// 	source: plexus\interop\testing\TestAppLauncher.interop
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code
namespace Plexus.Interop.Testing.Generated {
	
	using System;
	using global::Plexus;
	using global::Plexus.Channels;
	using global::Plexus.Interop;
	using global::System.Threading.Tasks;
					
					
	public partial interface ITestAppLauncherClient: IClient {
	}
	
	public sealed partial class TestAppLauncherClient: ClientBase, ITestAppLauncherClient {
		
		public const string Id = "plexus.interop.testing.TestAppLauncher";
		
		private static ClientOptions CreateClientOptions(TestAppLauncherClient.ServiceBinder serviceBinder, Func<ClientOptionsBuilder, ClientOptionsBuilder> setup = null) {
			ClientOptionsBuilder builder = new ClientOptionsBuilder().WithApplicationId(Id).WithDefaultConfiguration();
			serviceBinder.Bind(builder);
			if (setup != null) {
				builder = setup(builder);
			}
			return builder.Build();
		}
		
		public TestAppLauncherClient(
			TestAppLauncherClient.IAppLauncherServiceImpl appLauncherService,
			Func<ClientOptionsBuilder, ClientOptionsBuilder> setup = null
		)
		:this(new TestAppLauncherClient.ServiceBinder(
			appLauncherService
		), setup) { }
		
		public TestAppLauncherClient(TestAppLauncherClient.ServiceBinder serviceBinder, Func<ClientOptionsBuilder, ClientOptionsBuilder> setup = null): base(CreateClientOptions(serviceBinder, setup)) 
		{
		}
	
		public sealed partial class ServiceBinder {
			
			public ServiceBinder(
				TestAppLauncherClient.IAppLauncherServiceImpl appLauncherService
			) {
				_appLauncherServiceBinder = new TestAppLauncherClient.AppLauncherServiceBinder(appLauncherService);
			}
			
			private AppLauncherServiceBinder _appLauncherServiceBinder;
			
			public ClientOptionsBuilder Bind(ClientOptionsBuilder builder) {
				builder = _appLauncherServiceBinder.Bind(builder);
				return builder;
			}
		}
	
		public partial interface IAppLauncherServiceImpl:
			global::Plexus.Interop.Testing.Generated.AppLauncherService.ILaunchImpl
		{ }
		
		private sealed partial class AppLauncherServiceBinder {
			
			
			private readonly IAppLauncherServiceImpl _impl;
			
			public AppLauncherServiceBinder(IAppLauncherServiceImpl impl) {
				_impl = impl;
			}
			
			public ClientOptionsBuilder Bind(ClientOptionsBuilder builder) {
				return builder.WithProvidedService(global::Plexus.Interop.Testing.Generated.AppLauncherService.Id, Bind);
			}
			
			private ProvidedServiceDefinition.Builder Bind(ProvidedServiceDefinition.Builder builder) {
				builder = builder.WithUnaryMethod<global::Plexus.Interop.Testing.Generated.AppLaunchRequest, global::Plexus.Interop.Testing.Generated.AppLaunchResponse>(global::Plexus.Interop.Testing.Generated.AppLauncherService.LaunchMethodId, _impl.Launch);
				return builder; 							
			}
		}
		
		public sealed partial class AppLauncherServiceImpl: IAppLauncherServiceImpl
		{
			private readonly UnaryMethodHandler<global::Plexus.Interop.Testing.Generated.AppLaunchRequest, global::Plexus.Interop.Testing.Generated.AppLaunchResponse> _launchHandler;
			
			public AppLauncherServiceImpl(
				UnaryMethodHandler<global::Plexus.Interop.Testing.Generated.AppLaunchRequest, global::Plexus.Interop.Testing.Generated.AppLaunchResponse> launchHandler
			) {
				_launchHandler = launchHandler;
			}
			
			public Task<global::Plexus.Interop.Testing.Generated.AppLaunchResponse> Launch(global::Plexus.Interop.Testing.Generated.AppLaunchRequest request, MethodCallContext context) {
				return _launchHandler(request, context);
			}
		}					
		
		public sealed partial class AppLauncherServiceImpl<T>: IAppLauncherServiceImpl
			where T:
			global::Plexus.Interop.Testing.Generated.AppLauncherService.ILaunchImpl
		{
			private readonly T _impl;
			
			public AppLauncherServiceImpl(T impl) {
				_impl = impl;
			}
			
			public Task<global::Plexus.Interop.Testing.Generated.AppLaunchResponse> Launch(global::Plexus.Interop.Testing.Generated.AppLaunchRequest request, MethodCallContext context) {
				return _impl.Launch(request, context);
			}
		}
		
	}
}
#endregion Designer generated code

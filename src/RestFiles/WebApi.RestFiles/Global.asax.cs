using System.IO;
using System.ServiceModel;
using System.Web;
using System.Web.Routing;
using LightCore;
using WebApi.RestFiles.Operations;

namespace WebApi.RestFiles {
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class MvcApplication : HttpApplication {
		
		protected void Application_Start() {

			var appConfig = new AppConfig();
			
			var builder = new ContainerBuilder();
			
			builder.Register(appConfig);

			var container = builder.Build();
			
			RouteTable.Routes.SetDefaultHttpConfiguration(new LightCoreConfiguration(container)
			                                              	{
			                                              		EnableHelpPage = true,
																MaxReceivedMessageSize = 1024 * 1024,
																TransferMode = TransferMode.Streamed
			                                              	});

			RouteTable.Routes.MapServiceRoute<FilesApi>("files");
			RouteTable.Routes.MapServiceRoute<RevertFilesApi>("revertfiles");
		}
	}
}
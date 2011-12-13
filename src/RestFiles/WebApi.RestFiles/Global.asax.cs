using System.IO;
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
			
			if (!Directory.Exists(appConfig.RootDirectory)) {
				Directory.CreateDirectory(appConfig.RootDirectory);
				Directory.CreateDirectory(Path.Combine(appConfig.RootDirectory, "test"));
			}

			var builder = new ContainerBuilder();
			builder.Register(appConfig);

			var container = builder.Build();
			
			RouteTable.Routes.SetDefaultHttpConfiguration(new LightCoreConfiguration(container));
			RouteTable.Routes.MapServiceRoute<FilesApi>("files");
		}
	}
}
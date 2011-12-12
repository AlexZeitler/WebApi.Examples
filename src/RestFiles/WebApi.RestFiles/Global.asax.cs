using System.Configuration;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Routing;
using Microsoft.ApplicationServer.Http;
using WebApi.RestFiles.Operations;

namespace WebApi.RestFiles {
	// Note: For instructions on enabling IIS6 or IIS7 classic mode, 
	// visit http://go.microsoft.com/?LinkId=9394801

	public class MvcApplication : HttpApplication {
		
		protected void Application_Start() {
			RouteTable.Routes.SetDefaultHttpConfiguration(new WebApiConfiguration());
			RouteTable.Routes.MapServiceRoute<FilesApi>("files");

			var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			var root = Path.Combine(assemblyPath, ConfigurationManager.AppSettings["RootDirectory"]);
			if (!Directory.Exists(root)) {
				Directory.CreateDirectory(root);
				Directory.CreateDirectory(root + "test");
			}
		}
	}
}
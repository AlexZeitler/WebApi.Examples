using System.IO;
using System.Net;
using System.Net.Http;
using System.ServiceModel.Web;

namespace WebApi.RestFiles.Operations {
	public class RevertFilesApi {
		public AppConfig Config { get; set; }

		public RevertFilesApi(AppConfig config) {
			Config = config;
		}

		[WebInvoke(Method = "POST", UriTemplate = "")]
		public HttpResponseMessage Post()
		{

			Config.Initialize();
			
			return new HttpResponseMessage(HttpStatusCode.OK);
		}
	}
}
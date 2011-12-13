using LightCore;
using Microsoft.ApplicationServer.Http;

namespace WebApi.RestFiles.Operations {
	public class LightCoreConfiguration : WebApiConfiguration {
		public LightCoreConfiguration(IContainer container) {
			CreateInstance = (t, i, m) =>
			                 	{
			                 		return container.Resolve(t);
			                 	};
		}
	}
}
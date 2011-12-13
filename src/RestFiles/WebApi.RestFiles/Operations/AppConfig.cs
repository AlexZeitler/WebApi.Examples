using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;

namespace WebApi.RestFiles.Operations {
	public class AppConfig {
		public AppConfig() {
			var approot = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			var root = Path.Combine(approot, ConfigurationManager.AppSettings["RootDirectory"]);
			this.RootDirectory = root;
			this.TextFileExtensions = ConfigurationManager.AppSettings["TextFileExtensions"].Split(',');
			this.ExcludeDirectories = ConfigurationManager.AppSettings["ExcludeDirectories"].Split(',');
		}

		public string RootDirectory { get; set; }

		public IList<string> TextFileExtensions { get; set; }

		public IList<string> ExcludeDirectories { get; set; }
	}
}
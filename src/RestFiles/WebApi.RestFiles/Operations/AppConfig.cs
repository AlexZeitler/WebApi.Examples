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

			Initialize();
		}

		public void Initialize()
		{
			if (Directory.Exists(this.RootDirectory))
			{
				Directory.Delete(this.RootDirectory, true);
			}

			Directory.CreateDirectory(this.RootDirectory);
			DirectoryInfo testDirectory =
				Directory.CreateDirectory(Path.Combine(this.RootDirectory, "test"));


			using (StreamWriter outfile =
				new StreamWriter(Path.Combine(testDirectory.FullName, "readme.md")))
			{
				outfile.Write("Some markdown");
			}
		}

		public string RootDirectory { get; set; }

		public IList<string> TextFileExtensions { get; set; }

		public IList<string> ExcludeDirectories { get; set; }
	}
}
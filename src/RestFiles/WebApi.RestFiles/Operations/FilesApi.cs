using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.ServiceModel.Web;
using WebApi.RestFiles.Types;
using File = WebApi.RestFiles.Types.File;

namespace WebApi.RestFiles.Operations
{
	public class FilesApi
	{
		[WebGet(UriTemplate = "{*Path}")]
		public HttpResponseMessage<FilesResponse> Get(string path)
		{
			var approot = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			var root = Path.Combine(approot, ConfigurationManager.AppSettings["RootDirectory"].Replace("/", "\\"));
			var folder = Path.Combine(root, path.Replace("/","\\"));
			var filesResponse = new FilesResponse();
			filesResponse.Directory = GetFolderResult(folder);
			return new HttpResponseMessage<FilesResponse>(filesResponse);
		}

		private FolderResult GetFolderResult(string targetPath) {
			var result = new FolderResult();

			foreach (var dirPath in Directory.GetDirectories(targetPath)) {
				var dirInfo = new DirectoryInfo(dirPath);

				if (ConfigurationManager.AppSettings["ExcludeDirectories"].Contains(dirInfo.Name)) continue;

				result.Folders.Add(new Folder {
					Name = dirInfo.Name,
					ModifiedDate = dirInfo.LastWriteTimeUtc,
					FileCount = dirInfo.GetFiles().Length
				});
			}

			foreach (var filePath in Directory.GetFiles(targetPath)) {
				var fileInfo = new FileInfo(filePath);

				result.Files.Add(new File {
					Name = fileInfo.Name,
					Extension = fileInfo.Extension,
					FileSizeBytes = fileInfo.Length,
					ModifiedDate = fileInfo.LastWriteTimeUtc,
					IsTextFile = ConfigurationManager.AppSettings["TextFileExtensions"].Contains(fileInfo.Extension),
				});
			}

			return result;
		}
	}
}
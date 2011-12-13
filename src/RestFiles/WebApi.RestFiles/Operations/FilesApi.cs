using System.Configuration;
using System.IO;
using System.Net.Http;
using System.ServiceModel.Web;
using WebApi.RestFiles.Types;
using File = WebApi.RestFiles.Types.File;

namespace WebApi.RestFiles.Operations
{
	public class FilesApi
	{
		readonly AppConfig _config;

		public FilesApi(AppConfig config) {
			_config = config;
		}

		public AppConfig Config {
			get { return _config; }
		}

		[WebGet(UriTemplate = "{*Path}")]
		public HttpResponseMessage<FilesResponse> Get(string path)
		{
			var targetPath = Path.Combine(Config.RootDirectory, path.Replace("/","\\"));

			var isDirectory = Directory.Exists(targetPath);

			var response = isDirectory
				? new FilesResponse { Directory = GetFolderResult(targetPath) }
				: new FilesResponse { File = GetFileResult(new FileInfo(targetPath)) };
			
			return new HttpResponseMessage<FilesResponse>(response);
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

		private FileResult GetFileResult(FileInfo fileInfo) {
			var isTextFile = this.Config.TextFileExtensions.Contains(fileInfo.Extension);

			return new FileResult {
				Name = fileInfo.Name,
				Extension = fileInfo.Extension,
				FileSizeBytes = fileInfo.Length,
				IsTextFile = isTextFile,
				Contents = isTextFile ? System.IO.File.ReadAllText(fileInfo.FullName) : null,
				ModifiedDate = fileInfo.LastWriteTimeUtc,
			};
		}
	}
}
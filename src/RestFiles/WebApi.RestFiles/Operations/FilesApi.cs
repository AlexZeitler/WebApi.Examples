using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ServiceModel.Web;
using System.Web;
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
		public HttpResponseMessage Get(string path, HttpRequestMessage request)
		{
			var targetPath = Path.Combine(Config.RootDirectory, path.Replace("/","\\"));
			var forDownload = false;

			var queryString = HttpUtility.ParseQueryString(request.RequestUri.Query.ToLower());
			if(queryString.AllKeys.Contains("fordownload")) {
				forDownload = bool.Parse(queryString["fordownload"]);
			}

			var isDirectory = Directory.Exists(targetPath);

			if (!isDirectory && forDownload) {
				var file = new FileStream(targetPath, FileMode.Open);
				
				var message = new HttpResponseMessage {
					Content = new StreamContent(file),
				};
				
				message.Content.Headers.ContentDisposition = 
					new ContentDispositionHeaderValue("attachment");

				return message;
			}

			var response = isDirectory
				? new FilesResponse { Directory = GetFolderResult(targetPath) }
				: new FilesResponse { File = GetFileResult(new FileInfo(targetPath)) };
			
			return new HttpResponseMessage<FilesResponse>(response);
		}

		[WebInvoke(Method = "POST", UriTemplate = "{*Path}")]
		public HttpResponseMessage Post(string path, HttpRequestMessage request) {
			var targetPath = Path.Combine(Config.RootDirectory, path.Replace("/", "\\"));

			var targetDir = new FileInfo(targetPath);

			if (!Directory.Exists(targetDir.FullName)) {
				Directory.CreateDirectory(targetDir.FullName);
			}

			return new HttpResponseMessage(HttpStatusCode.OK);
		}

		private FolderResult GetFolderResult(string targetPath) {
			var result = new FolderResult();

			foreach (var dirPath in Directory.GetDirectories(targetPath)) {
				var dirInfo = new DirectoryInfo(dirPath);

				if (Config.ExcludeDirectories.Contains(dirInfo.Name)) continue;

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
					IsTextFile = Config.TextFileExtensions.Contains(fileInfo.Extension),
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
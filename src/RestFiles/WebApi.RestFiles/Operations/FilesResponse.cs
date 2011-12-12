using WebApi.RestFiles.Types;

namespace WebApi.RestFiles.Operations
{
	public class FilesResponse
	{
		public FolderResult Directory { get; set; }

		public FileResult File { get; set; }
	}
}
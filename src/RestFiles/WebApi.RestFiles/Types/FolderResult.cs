using System.Collections.Generic;

namespace WebApi.RestFiles.Types
{
	public class FolderResult
	{
		public FolderResult()
		{
			Folders = new List<Folder>();
			Files = new List<File>();
		}

		public List<Folder> Folders { get; set; }

		public List<File> Files { get; set; }
	}
}
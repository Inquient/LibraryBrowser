using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectoryContentTool
{
	public static class DirectoryContentReader
	{
		public static (List<string> files, List<string> directories) ReadDirectory(string directoryPath)
		{
			List<string> allFiles = new List<string>();
            List<string> allSubDirectories = new List<string>();
            ReadDirectoryContent(directoryPath, ref allFiles, ref allSubDirectories);
			return (allFiles, allSubDirectories);

		}

		public static void ReadDirectoryContent(string directoryPath, ref List<string> filesList, ref List<string> directoriesList)
		{
			var subDirectories = Directory.EnumerateDirectories(directoryPath);
			filesList.AddRange(Directory.EnumerateFiles(directoryPath));
			directoriesList.AddRange(subDirectories);

			if (subDirectories.Any())
			{
				foreach(var subDirectory in subDirectories)
				{
                    ReadDirectoryContent(subDirectory, ref filesList, ref directoriesList);
				}
			}
		}
	}
}

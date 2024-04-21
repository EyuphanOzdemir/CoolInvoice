using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoicrCoreModels.Extensions
{
	public static class DirectoryExtensions
	{
		public static void DeleteFiles(this DirectoryInfo directory, string extension = null)
		{
			if (directory.Exists)
			{
				string[] filesToDelete;
				if (extension != null)
					filesToDelete = Directory.GetFiles(directory.FullName, $"*{extension}");
				else
					filesToDelete = Directory.GetFiles(directory.FullName);

				foreach (string file in filesToDelete)
				{
					File.Delete(file);
				}
			}
		}


		public static int CountFiles(this DirectoryInfo directory, string extension = null)
		{
			int fileCount = 0;

			if (directory.Exists)
			{
				string[] files;
				if (extension != null)
					files = Directory.GetFiles(directory.FullName, $"*{extension}");
				else
					files = Directory.GetFiles(directory.FullName);

				fileCount = files.Length;
			}

			return fileCount;
		}
	}
}
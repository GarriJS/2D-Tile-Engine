using System.Collections.Generic;
using System.IO;

namespace Engine.Core.Files.Services.Contracts
{
	/// <summary>
	/// Represents a JSON service.
	/// </summary>
	public interface IJsonService
	{
		/// <summary>
		/// Gets the JSON file stream.
		/// </summary>
		/// <param name="contentManagerName">The content manager name.</param>
		/// <param name="folderName">The folder name.</param>
		/// <param name="fileName">The file name.</param>
		/// <returns>The JSON file stream.</returns>
		public FileStream GetJsonFileStream(string contentManagerName, string folderName, string fileName);

		/// <summary>
		/// Gets the JSON file path.
		/// </summary>
		/// <param name="contentManagerName">The content manager name.</param>
		/// <param name="folderName">The folder name.</param>
		/// <param name="fileName">The file name.</param>
		/// <param name="createDirectoryIfDoesNotExist">A value describing whether to create the directory if it does not exist.</param>
		/// <returns>The JSON file path.</returns>
		public string GetJsonFilePath(string contentManagerName, string folderName, string fileName, bool createDirectoryIfDoesNotExist = false);

		/// <summary>
		/// Gets the JSON file names inside the folder.
		/// </summary>
		/// <param name="contentManagerName">The content manager name.</param>
		/// <param name="folderName">The folder name.</param>
		/// <returns>The JSON file names.</returns>
		public IList<string> GetJsonFileNames(string contentManagerName, string folderName);
	}
}

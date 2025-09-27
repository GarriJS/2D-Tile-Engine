using Engine.Core.Files.Contracts;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;

namespace Engine.Core.Files
{
	/// <summary>
	/// Represents a JSON service.
	/// </summary>
	/// <remarks>
	/// Initializes a new instance of the JSON service.
	/// </remarks>
	/// <param name="gameServices">The game services.</param>
	public class JsonService(GameServiceContainer gameServices) : IJsonService
	{
		private readonly GameServiceContainer _gameServices = gameServices;

		/// <summary>
		/// Gets or sets the current directory.
		/// </summary>
		private string CurrentDirectory {get; set;} = Directory.GetCurrentDirectory();

		/// <summary>
		/// Gets the JSON file stream.
		/// </summary>
		/// <param name="contentManagerName">The content manager name.</param>
		/// <param name="folderName">The folder name.</param>
		/// <param name="fileName">The file name.</param>
		/// <returns>The JSON file stream.</returns>
		public FileStream GetJsonFileStream(string contentManagerName, string folderName, string fileName)
		{
			var filePath = this.GetJsonFilePath(contentManagerName, folderName, fileName);

			if (false == File.Exists(filePath))
			{ 
				return null;
			}

			return File.OpenRead(filePath);
		}

		/// <summary>
		/// Gets the JSON file path.
		/// </summary>
		/// <param name="contentManagerName">The content manager name.</param>
		/// <param name="folderName">The folder name.</param>
		/// <param name="fileName">The file name.</param>
		/// <param name="createDirectoryIfDoesNotExist">A value describing whether to create the directory if it does not exist.</param>
		/// <returns>The JSON file path.</returns>
		public string GetJsonFilePath(string contentManagerName, string folderName, string fileName, bool createDirectoryIfDoesNotExist = false)
		{
			var folderPath = Path.Combine(this.CurrentDirectory, contentManagerName, folderName);

			if ((true == createDirectoryIfDoesNotExist) &&
				(false == Directory.Exists(folderPath)))
			{
				Directory.CreateDirectory(folderPath);
			}

			return Path.Combine(folderPath, $"{fileName}.json");
		}

		/// <summary>
		/// Gets the JSON file names inside the folder.
		/// </summary>
		/// <param name="contentManagerName">The content manager name.</param>
		/// <param name="folderName">The folder name.</param>
		/// <returns>The JSON file names.</returns>
		public IList<string> GetJsonFileNames(string contentManagerName, string folderName)
		{
			var folderPath = Path.Combine(this.CurrentDirectory, contentManagerName, folderName);

			if (false == Directory.Exists(folderPath))
			{
				return [];
			}

			var files = Directory.GetFiles(folderPath, "*.json");
			var fileNames = new List<string>(files.Length);

			foreach (var file in files)
			{
				fileNames.Add(Path.GetFileNameWithoutExtension(file));
			}

			return fileNames;
		}
	}
}

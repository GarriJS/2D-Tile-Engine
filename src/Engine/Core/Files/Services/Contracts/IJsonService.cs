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
		FileStream GetJsonFileStream(string contentManagerName, string folderName, string fileName);
	}
}

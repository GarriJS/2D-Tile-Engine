using System.IO;
using System.Text.Json;

namespace Engine.DiskModels
{
	/// <summary>
	/// Represents a model serializer.
	/// </summary>
	/// <typeparam name="T">The type being serialized to.</typeparam>
	public class ModelSerializer<T>
	{
		/// <summary>
		/// Deserializes the file stream.
		/// </summary>
		/// <param name="fileStream">The file stream.</param>
		/// <returns>The deserializes result.</returns>
		public T Deserialize(FileStream fileStream, JsonSerializerOptions options = null)
		{
			var result = JsonSerializer.Deserialize<T>(fileStream, options);

			return result;
		}

		/// <summary>
		/// Deserializes the file path.
		/// </summary>
		/// <param name="filePath">The JSON file path.</param>
		/// <returns>The deserializes result.</returns>
		public T Deserialize(string filePath, JsonSerializerOptions options = null)
		{
			using var stream = File.OpenRead(filePath);
			{
				var result = Deserialize(stream, options);

				return result;
			}
		}

		/// <summary>
		/// Serializes the data to the file path.
		/// </summary>
		/// <param name="filePath">The JSON file path.</param>
		/// <param name="data">The data to serialize.</param>
		/// <param name="options">The options.</param>
		public void Serialize(string filePath, T data, JsonSerializerOptions options = null)
		{
			var folder = Path.GetDirectoryName(filePath);

			if (false == Directory.Exists(folder))
				Directory.CreateDirectory(folder);

			using var stream = File.Create(filePath);
			JsonSerializer.Serialize(stream, data, options);
		}
	}
}

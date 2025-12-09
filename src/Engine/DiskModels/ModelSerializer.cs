using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Engine.DiskModels
{
	/// <summary>
	/// Represents a model serializer.
	/// </summary>
	/// <typeparam name="T">The type being serialized to.</typeparam>
	public class ModelSerializer<T> where T : BaseDiskModel
	{
		/// <summary>
		/// Deserializes the file stream.
		/// </summary>
		/// <param name="fileStream">The file stream.</param>
		/// <param name="options">The options.</param>
		/// <returns>The deserializes result.</returns>
		public T Deserialize(FileStream fileStream, JsonSerializerOptions options = null)
		{
			using var reader = new StreamReader(fileStream);
			string json = reader.ReadToEnd();

			var result = DiskModelDeserializer.Deserialize(json, options);

			return result as T;
		}

		/// <summary>
		/// Deserializes the file path.
		/// </summary>
		/// <param name="filePath">The JSON file path.</param>
		/// <param name="options">The options.</param>
		/// <returns>The deserializes result.</returns>
		public T Deserialize(string filePath, JsonSerializerOptions options = null)
		{
			using var stream = File.OpenRead(filePath);
			{
				return Deserialize(stream, options);
			}
		}

		/// <summary>
		/// Deserializes the file stream to a list.
		/// </summary>
		/// <param name="fileStream">The file stream.</param>
		/// <returns>The deserializes result.</returns>
		public List<T> DeserializeList(FileStream fileStream)
		{
			using var reader = new StreamReader(fileStream);
			string json = reader.ReadToEnd();

			var arrayNode = JsonNode.Parse(json)?.AsArray();
			if (null == arrayNode)
				return [];

			var result = new List<T>();

			foreach (var node in arrayNode)
			{
				var model = DiskModelDeserializer.Deserialize(node.ToJsonString());
				if (model is T tModel)
					result.Add(tModel);
			}

			return result;
		}

		/// <summary>
		/// Deserializes the file path to a list.
		/// </summary>
		/// <param name="filePath">The JSON file path.</param>
		/// <returns>The deserializes result.</returns>
		public List<T> DeserializeList(string filePath)
		{
			using var stream = File.OpenRead(filePath);
			var result = DeserializeList(stream);

			return result;
		}

		/// <summary>
		/// Serializes the data to the file path.
		/// </summary>
		/// <param name="filePath">The JSON file path.</param>
		/// <param name="data">The data to serialize.</param>
		/// <param name="options">The options.</param>
		public void Serialize(string filePath, T data, JsonSerializerOptions options)
		{
			var folder = Path.GetDirectoryName(filePath);

			if (false == Directory.Exists(folder))
				Directory.CreateDirectory(folder);

			using var stream = File.Create(filePath);
			JsonSerializer.Serialize(stream, data, options);
		}
	}
}

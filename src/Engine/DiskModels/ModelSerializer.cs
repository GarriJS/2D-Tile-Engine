using System.IO;
using System.Runtime.Serialization.Json;

namespace Engine.DiskModels
{
	/// <summary>
	/// Represents a model serializer.
	/// </summary>
	/// <typeparam name="T">The type being serialized to.</typeparam>
	public class ModelSerializer<T>
	{
		/// <summary>
		/// Gets the serializer.
		/// </summary>
		private DataContractJsonSerializer Serializer { get; } = new DataContractJsonSerializer(typeof(T));

		/// <summary>
		/// Deserializes the file stream.
		/// </summary>
		/// <param name="fileStream">The file stream.</param>
		/// <returns>The deserializes result.</returns>
		public T Deserialize(FileStream fileStream)
		{ 
			return (T)this.Serializer.ReadObject(fileStream);
		}

		/// <summary>
		/// Deserializes the file path.
		/// </summary>
		/// <param name="filePath">The JSON file path.</param>
		/// <returns>The deserializes result.</returns>
		public T Deserialize(string filePath)
		{
			using var stream = File.OpenRead(filePath);
			return Deserialize(stream);
		}

		/// <summary>
		/// Serializes the data to the file path.
		/// </summary>
		/// <param name="filePath">The JSON file path.</param>
		/// <param name="data">The data to serialize.</param>
		public void Serialize(string filePath, T data)
		{
			var folder = Path.GetDirectoryName(filePath);

			if (false == Directory.Exists(folder))
			{
				Directory.CreateDirectory(folder);
			}

			using var stream = File.Create(filePath);
			this.Serializer.WriteObject(stream, data);
		}
	}
}

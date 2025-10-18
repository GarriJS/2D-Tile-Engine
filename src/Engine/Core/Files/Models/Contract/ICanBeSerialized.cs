namespace Engine.Core.Files.Models.Contract
{
	/// <summary>
	/// Represents something that can be serialized.
	/// </summary>
	/// <typeparam name="T">The serialization type.</typeparam>
	public interface ICanBeSerialized<T>
	{
		/// <summary>
		/// Converts the object to a serialization model.
		/// </summary>
		/// <returns>The serialization model.</returns>
		public T ToModel();
	}
}

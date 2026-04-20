using Microsoft.Xna.Framework;

namespace Engine.Physics.Models
{
	/// <summary>
	/// Represents a vector2 extender.
	/// </summary>
	/// <typeparam name="T">The type being extended with vector2.</typeparam>
	readonly public struct Vector2Extender<T>
	{
		/// <summary>
		/// Gets or sets the vector2.
		/// </summary>
		required readonly public Vector2 Vector2 { get; init; }

		/// <summary>
		/// Gets or sets the subject.
		/// </summary>
		required readonly public T Subject { get; init; }
	}
}

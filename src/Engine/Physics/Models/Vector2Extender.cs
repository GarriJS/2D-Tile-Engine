using Microsoft.Xna.Framework;

namespace Engine.Physics.Models
{
	/// <summary>
	/// Represents a vector2 extender.
	/// </summary>
	/// <typeparam name="T">The type being extended with vector2.</typeparam>
	public struct Vector2Extender<T>
	{
		/// <summary>
		/// Gets or sets the vector.
		/// </summary>
		required public Vector2 Vector { get; set; }

		/// <summary>
		/// Gets or sets the subject.
		/// </summary>
		required public T Subject { get; set; }
	}
}

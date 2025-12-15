using Microsoft.Xna.Framework;

namespace Engine.Physics.Models
{
	/// <summary>
	/// Represents a location extender.
	/// </summary>
	/// <typeparam name="T">The type being extended with location.</typeparam>
	public class LocationExtender<T>
	{
		/// <summary>
		/// Gets or sets the location.
		/// </summary>
		public Vector2 Location { get; set; }

		/// <summary>
		/// Gets or sets the subject.
		/// </summary>
		public T Subject { get; set; }
	}
}

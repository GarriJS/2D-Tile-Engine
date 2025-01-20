using Engine.Physics.Models.Contracts;
using Microsoft.Xna.Framework;

namespace Engine.Physics.Models
{
	/// <summary>
	/// Represents a simple area.
	/// </summary>
	public class SimpleArea : IAmAArea
	{
		/// <summary>
		/// Get or sets the width.
		/// </summary>
		public float Width { get; set; }

		/// <summary>
		/// Gets or sets the height.
		/// </summary>
		public float Height { get; set; }

		/// <summary>
		/// Gets or sets the position.
		/// </summary>
		public Position Position { get; set; }

		/// <summary>
		/// Determines if a the area contains the coordinate.
		/// </summary>
		/// <param name="coordinate">The coordinate.</param>
		/// <returns>A value indicating whether the area contains the coordinate.</returns>
		public bool Contains(Vector2 coordinate)
		{ 
			return this.Position.X <= coordinate.X &&
				   this.Position.X + this.Width >= coordinate.X &&
				   this.Position.Y <= coordinate.Y &&
				   this.Position.Y + this.Height >= coordinate.Y;
		}
	}
}

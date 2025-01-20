using Microsoft.Xna.Framework;

namespace Engine.Physics.Models
{
	/// <summary>
	/// Represents a offset area.
	/// </summary>
	public class OffsetArea : SimpleArea
	{
		/// <summary>
		/// Gets or sets the offset position.
		/// </summary>
		public Vector2 OffsetPosition { get => new(this.Position.X + this.HorizontalOffset, this.Position.Y + this.VerticalOffset); }

		/// <summary>
		/// Gets or sets the vertical offset.
		/// </summary>
		public float VerticalOffset { get; set; }

		/// <summary>
		/// Gets or sets the horizontal offset.
		/// </summary>
		public float HorizontalOffset { get; set; }

		/// <summary>
		/// Determines if a the area contains the coordinate.
		/// </summary>
		/// <param name="coordinate">The coordinate.</param>
		/// <returns>A value indicating whether the area contains the coordinate.</returns>
		public new bool Contains(Vector2 coordinate)
		{
			var offsetPosition = this.OffsetPosition;	

			return offsetPosition.X <= coordinate.X &&
				   offsetPosition.X + this.Width >= coordinate.X &&
				   offsetPosition.Y <= coordinate.Y &&
				   offsetPosition.Y + this.Height >= coordinate.Y;
		}
	}
}

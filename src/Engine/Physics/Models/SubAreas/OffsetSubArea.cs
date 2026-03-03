using Engine.Core.Files.Models.Contract;
using Engine.DiskModels.Physics;
using Microsoft.Xna.Framework;

namespace Engine.Physics.Models.SubAreas
{
	/// <summary>
	/// Represents a offset sub area.
	/// </summary>
	public class OffsetSubArea : SubArea, ICanBeSerialized<OffsetSubAreaModel>
	{
		/// <summary>
		/// Gets or sets the horizontal offset.
		/// </summary>
		public float HorizontalOffset { get; set; }

		/// <summary>
		/// Gets or sets the vertical offset.
		/// </summary>
		public float VerticalOffset { get; set; }

		/// <summary>
		/// Coverts the offset sub area to a rectangle.
		/// </summary>
		/// <param name="location">The location of the rectangle.</param>
		/// <returns>The rectangle.</returns>
		override public Rectangle ToRectangle(Vector2? location)
		{
			location ??= default;

			var result = new Rectangle
			{
				X = (int)(location.Value.X + this.HorizontalOffset),
				Y = (int)(location.Value.Y + this.VerticalOffset),
				Width = (int)this.Width,
				Height = (int)this.Height
			};

			return result;
		}

		/// <summary>
		/// Converts the object to a serialization model.
		/// </summary>
		/// <returns>The serialization model.</returns>
		override public OffsetSubAreaModel ToModel()
		{
			var result = new OffsetSubAreaModel
			{
				Width = Width,
				Height = Height,
				HorizontalOffset = HorizontalOffset,
				VerticalOffset = VerticalOffset
			};

			return result;
		}

		/// <summary>
		/// The offset sub area as a string.
		/// </summary>
		/// <returns>The offset sub area as a string.</returns>
		override public string ToString()
		{
			return "{Width:" + this.Width + " Height:" + this.Height + "HorizontalOffset:" + this.HorizontalOffset + "VerticalOffset:" + this.VerticalOffset + "}";
		}
	}
}

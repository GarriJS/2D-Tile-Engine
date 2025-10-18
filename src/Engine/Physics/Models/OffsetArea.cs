using Engine.Core.Files.Models.Contract;
using Engine.DiskModels.Physics;
using Microsoft.Xna.Framework;

namespace Engine.Physics.Models
{
	/// <summary>
	/// Represents a offset area.
	/// </summary>
	public class OffsetArea : SimpleArea, ICanBeSerialized<OffsetAreaModel>
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
		new public bool Contains(Vector2 coordinate)
		{
			var offsetPosition = this.OffsetPosition;	
			var result = offsetPosition.X <= coordinate.X &&
						 offsetPosition.X + this.Width >= coordinate.X &&
						 offsetPosition.Y <= coordinate.Y &&
						 offsetPosition.Y + this.Height >= coordinate.Y;
		
			return result;
		}

		/// <summary>
		/// Converts the object to a serialization model.
		/// </summary>
		/// <returns>The serialization model.</returns>
		new public OffsetAreaModel ToModel()
		{
			var positionModel = this.Position.ToModel();

			return new OffsetAreaModel
			{
				Width = this.Width,
				Height = this.Height,
				Position = positionModel,
				HorizontalOffset = this.HorizontalOffset,
				VerticalOffset = this.VerticalOffset	
			};
		}
	}
}

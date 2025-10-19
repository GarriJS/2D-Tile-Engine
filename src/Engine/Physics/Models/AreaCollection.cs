using Engine.Core.Files.Models.Contract;
using Engine.DiskModels.Physics;
using Engine.Physics.Models.Contracts;
using Microsoft.Xna.Framework;

namespace Engine.Physics.Models
{
	/// <summary>
	/// Represents a area collection
	/// </summary>
	public class AreaCollection(float width, float height) : IAmAArea, ICanBeSerialized<AreaCollectionModel>
	{
		/// <summary>
		/// Gets the width.
		/// </summary>
		public float Width { get; } = width;

		/// <summary>
		/// Gets the height.
		/// </summary>
		public float Height { get; } = height;

		/// <summary>
		/// Gets or sets the position.
		/// </summary>
		public Position Position { get; set; }

		/// <summary>
		/// Gets or sets the sub areas.
		/// </summary>
		public OffsetSubArea[] SubAreas { get; set; }

		/// <summary>
		/// Determines if a the area contains the coordinate.
		/// </summary>
		/// <param name="coordinate">The coordinate.</param>
		/// <returns>A value indicating whether the area contains the coordinate.</returns>
		public bool Contains(Vector2 coordinate)
		{
			foreach (var area in SubAreas)
			{
				var truePosition = this.Position.Coordinates + new Vector2
				{
					X = area.HorizontalOffset,
					Y = area.VerticalOffset
				};

				if (truePosition.X <= coordinate.X &&
					truePosition.X + area.Width >= coordinate.X &&
					truePosition.Y <= coordinate.Y &&
					truePosition.Y + area.Height >= coordinate.Y)
				{
					return true;
				}
			}
			
			return false;
		}

		/// <summary>
		/// Converts the object to a serialization model.
		/// </summary>
		/// <returns>The serialization model.</returns>
		public AreaCollectionModel ToModel()
		{
			var positionModel = this.Position.ToModel();
			var offsetSubArea = new OffsetSubAreaModel[this.SubAreas.Length];

			for (int i = 0; i < this.SubAreas.Length; i++)
			{
				offsetSubArea[i] = this.SubAreas[i].ToModel();
			}

			return new AreaCollectionModel
			{
				Width = this.Width,
				Height = this.Height,
				Position = positionModel,
				SubAreas = offsetSubArea
			};
		}
	}
}

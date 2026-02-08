using Engine.Core.Files.Models.Contract;
using Engine.DiskModels.Physics;
using Engine.Monogame;
using Engine.RunTime.Services.Contracts;
using Microsoft.Xna.Framework;
using System.Linq;

namespace Engine.Physics.Models
{
	/// <summary>
	/// Represents a offset area.
	/// </summary>
	sealed public class OffsetArea : SimpleArea, ICanBeSerialized<OffsetAreaModel>
	{
		/// <summary>
		/// Gets the offset position.
		/// </summary>
		public Vector2 OffsetPosition
		{
			get => new()
			{
				X = this.Position.X + HorizontalOffset,
				Y = this.Position.Y + VerticalOffset
			};
		}

		/// <summary>
		/// Gets or sets the vertical offset.
		/// </summary>
		required public float VerticalOffset { get; set; }

		/// <summary>
		/// Gets or sets the horizontal offset.
		/// </summary>
		required public float HorizontalOffset { get; set; }

		/// <summary>
		/// The area to a rectangle.
		/// </summary>
		override public Rectangle ToRectangle { get => new() { X = (int)(this.Position.X + this.HorizontalOffset), Y = (int)(this.Position.Y + this.VerticalOffset), Width = (int)this.Width, Height = (int)this.Height }; }

		/// <summary>
		/// Determines if a the area contains the coordinate.
		/// </summary>
		/// <param name="coordinate">The coordinate.</param>
		/// <returns>A value indicating whether the area contains the coordinate.</returns>
		override public bool Contains(Vector2 coordinate)
		{
			var result = this.Position.X + HorizontalOffset <= coordinate.X &&
						 this.Position.X + HorizontalOffset + Width >= coordinate.X &&
						 this.Position.Y + VerticalOffset <= coordinate.Y &&
						 this.Position.Y + VerticalOffset + Height >= coordinate.Y;

			return result;
		}

		/// <summary>
		/// Draws the sub drawable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		/// <param name="color">The color.</param>
		/// <param name="offset">The offset.</param>
		override public void Draw(GameTime gameTime, GameServiceContainer gameServices, Color color, Vector2 offset = default)
		{
			var drawingService = gameServices.GetService<IDrawingService>();
			var thisRectangle = new Rectangle
			{
				X = (int)(this.Position.X + this.HorizontalOffset + offset.X),
				Y = (int)(this.Position.Y + this.VerticalOffset + offset.Y),
				Width = (int)this.Width,
				Height = (int)this.Height
			};
			var sideRectangles = thisRectangle.GetEdgeRectangles(1)
											  .ToArray();

			foreach (var sideRectangle in sideRectangles ?? [])
				drawingService.DrawRectangle(sideRectangle, color);
		}

		/// <summary>
		/// Converts the object to a serialization model.
		/// </summary>
		/// <returns>The serialization model.</returns>
		override public OffsetAreaModel ToModel()
		{
			var positionModel = Position.ToModel();
			var result = new OffsetAreaModel
			{
				Width = Width,
				Height = Height,
				Position = positionModel,
				HorizontalOffset = HorizontalOffset,
				VerticalOffset = VerticalOffset
			};

			return result;
		}
	}
}

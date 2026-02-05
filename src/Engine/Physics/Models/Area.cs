using Engine.Core.Files.Models.Contract;
using Engine.DiskModels.Physics;
using Engine.Monogame;
using Engine.Physics.Models.Contracts;
using Engine.RunTime.Services.Contracts;
using Microsoft.Xna.Framework;
using System.Linq;

namespace Engine.Physics.Models
{
	/// <summary>
	/// Represents a simple area.
	/// </summary>
	public class SimpleArea : IAmAArea, ICanBeSerialized<AreaModel>
	{
		/// <summary>
		/// Get or sets the width.
		/// </summary>
		required public float Width { get; set; }

		/// <summary>
		/// Gets or sets the height.
		/// </summary>
		required public float Height { get; set; }

		/// <summary>
		/// Gets or sets the position.
		/// </summary>
		required public Position Position { get; set; }

		/// <summary>
		/// Determines if a the area contains the coordinate.
		/// </summary>
		/// <param name="coordinate">The coordinate.</param>
		/// <returns>A value indicating whether the area contains the coordinate.</returns>
		virtual public bool Contains(Vector2 coordinate)
		{ 
			var result = this.Position.X <= coordinate.X &&
						 this.Position.X + this.Width >= coordinate.X &&
						 this.Position.Y <= coordinate.Y &&
						 this.Position.Y + this.Height >= coordinate.Y;

			return result;
		}

		/// <summary>
		/// Draws the sub drawable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		/// <param name="color">The color.</param>
		/// <param name="offset">The offset.</param>
		virtual public void Draw(GameTime gameTime, GameServiceContainer gameServices, Color color, Vector2 offset = default)
		{
			var drawingService = gameServices.GetService<IDrawingService>();
			var thisRectangle = new Rectangle
			{
				X = (int)(this.Position.X + offset.X),
				Y = (int)(this.Position.Y + offset.Y),
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
		virtual public AreaModel ToModel()
		{ 
			var positionModel = this.Position.ToModel();
			var result = new AreaModel
			{
				Width = this.Width,
				Height = this.Height,
				Position = positionModel
			};

			return result;
		}
	}
}

using Engine.Core.Files.Models.Contract;
using Engine.DiskModels.Physics;
using Engine.Monogame;
using Engine.RunTime.Models.Contracts;
using Engine.RunTime.Services.Contracts;
using Microsoft.Xna.Framework;
using System.Linq;

namespace Engine.Physics.Models.SubAreas
{
	/// <summary>
	/// Represents a sub area.
	/// </summary>
	public class SubArea : IAmSubDrawable, ICanBeSerialized<SubAreaModel>
	{
		/// <summary>
		/// Gets or sets the width.
		/// </summary>
		public float Width;

		/// <summary>
		/// Gets or set the height.
		/// </summary>
		public float Height;

		/// <summary>
		/// Gets the sub area as a vector.
		/// </summary>
		public Vector2 ToVector => new() { X = Width, Y = Height };

		/// <summary>
		/// Initializes the sub area.
		/// </summary>
		public SubArea() { }

		/// <summary>
		/// Initializes the sub area.
		/// </summary>
		/// <param name="vector">The vector.</param>
		public SubArea(Vector2 vector)
		{
			this.Width = vector.X;
			this.Height = vector.Y;
		}

		/// <summary>
		/// Draws the sub drawable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		/// <param name="coordinates">The coordinates.</param>
		/// <param name="color">The color.</param>
		/// <param name="offset">The offset.</param>
		public void Draw(GameTime gameTime, GameServiceContainer gameServices, Vector2 coordinates, Color color, Vector2 offset = default)
		{
			var drawingService = gameServices.GetService<IDrawingService>();
			var thisRectangle = new Rectangle
			{
				X = (int)(coordinates.X + offset.X),
				Y = (int)(coordinates.Y + offset.Y),
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
		virtual public SubAreaModel ToModel()
		{
			var result = new SubAreaModel
			{
				Width = Width,
				Height = Height
			};

			return result;
		}

		/// <summary>
		/// The offset sub area as a string.
		/// </summary>
		/// <returns>The offset sub area as a string.</returns>
		override public string ToString()
		{
			var result = "{Width:" + this.Width + " Height:" + this.Height + "}";

			return result;
		}
	}
}

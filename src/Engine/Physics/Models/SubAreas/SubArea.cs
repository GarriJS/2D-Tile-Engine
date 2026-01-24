using Engine.Core.Files.Models.Contract;
using Engine.DiskModels.Physics;
using Engine.RunTime.Models.Contracts;
using Engine.RunTime.Services.Contracts;
using Microsoft.Xna.Framework;

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
		/// <param name="position">The position.</param>
		/// <param name="color">The color.</param>
		/// <param name="offset">The offset.</param>
		public void Draw(GameTime gameTime, GameServiceContainer gameServices, Position position, Color color, Vector2 offset = default)
		{
			var topLeft = position.Coordinates + offset;
			var drawingService = gameServices.GetService<IDrawingService>();
			var topRect = new Rectangle
			{
				X = (int)topLeft.X,
				Y = (int)topLeft.Y,
				Width = (int)Width,
				Height = 1
			};
			var bottomRect = new Rectangle
			{
				X = (int)topLeft.X,
				Y = (int)topLeft.Y + (int)Height - 1,
				Width = (int)Width,
				Height = 1
			};
			var leftRect = new Rectangle
			{
				X = (int)topLeft.X,
				Y = (int)topLeft.Y,
				Width = 1,
				Height = (int)Height
			};
			var rightRect = new Rectangle
			{
				X = (int)topLeft.X + (int)Width - 1,
				Y = (int)topLeft.Y,
				Width = 1,
				Height = (int)Height
			};
			drawingService.DrawRectangle(topRect, color);
			drawingService.DrawRectangle(bottomRect, color);
			drawingService.DrawRectangle(leftRect, color);
			drawingService.DrawRectangle(rightRect, color);
		}

		/// <summary>
		/// Converts the object to a serialization model.
		/// </summary>
		/// <returns>The serialization model.</returns>
		virtual public SubAreaModel ToModel()
		{
			var result =  new SubAreaModel
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

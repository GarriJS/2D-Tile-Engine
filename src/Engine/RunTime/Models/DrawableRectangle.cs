using Engine.Debugging.Models.Contracts;
using Engine.RunTime.Models.Contracts;
using Engine.RunTime.Services.Contracts;
using Microsoft.Xna.Framework;

namespace Engine.RunTime.Models
{
	/// <summary>
	/// Represents a drawable rectangle.
	/// </summary>
	public class DrawableRectangle : IAmDrawable, IAmDebugDrawable
	{
		/// <summary>
		/// Gets or sets the draw layer.
		/// </summary>
		public int DrawLayer { get; set; }

		/// <summary>
		/// Gets or sets the color.
		/// </summary>
		public Color Color { get; set; } = Color.MonoGameOrange;

		/// <summary>
		/// Gets or sets the color.
		/// </summary>
		public Color DebugColor { get; set; } = Color.MonoGameOrange;

		/// <summary>
		/// Gets or sets the rectangle.
		/// </summary>
		required public Rectangle Rectangle { get; set; }

		/// <summary>
		/// Draws the drawable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		public void Draw(GameTime gameTime, GameServiceContainer gameServices)
		{
			var drawService = gameServices.GetService<IDrawingService>();
			drawService.DrawRectangle(this.Rectangle, this.Color);
		}

		/// <summary>
		/// Draws the debug drawable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		public void DrawDebug(GameTime gameTime, GameServiceContainer gameServices)
		{
			var drawService = gameServices.GetService<IDrawingService>();
			drawService.DrawRectangle(this.Rectangle, this.DebugColor);
		}
	}
}

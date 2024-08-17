using Engine.Drawing.Models;
using Engine.Drawing.Models.Contracts;
using Microsoft.Xna.Framework;

namespace Engine.Drawing.Services.Contracts
{
	public interface IDrawingService
	{
		/// <summary>
		/// Begins the draw.
		/// </summary>
		public void BeginDraw();

		/// <summary>
		/// Ends the draw.
		/// </summary>
		public void EndDraw();

		/// <summary>
		/// Draws the drawable. 
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="drawable">The drawable.</param>
		public void Draw(GameTime gameTime, IAmDrawable drawable);

		/// <summary>
		/// Draws the animated.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="animated">The animated.</param>
		public void Draw(GameTime gameTime, IAmAnimated animated);

		/// <summary>
		/// Draws the animated.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="animation">The animation.</param>
		/// <param name="coordinates">The coordinates.</param>
		/// <param name="color">The color.</param>
		public void Draw(GameTime gameTime, Animation animation, Vector2 coordinates, Color color);

		/// <summary>
		/// Writes the drawable text.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="drawableText">The drawable text.</param>
		public void Write(GameTime gameTime, DrawableText drawableText);

		/// <summary>
		/// Writes the drawable text.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="drawableText">The drawable text.</param>
		/// <param name="color">The color.</param>
		public void Write(GameTime gameTime, DrawableText drawableText, Color color);
	}
}

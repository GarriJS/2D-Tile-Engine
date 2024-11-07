using Engine.Drawing.Models;
using Engine.Drawing.Models.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
		public void Draw(Texture2D texture, Vector2 coordinates, Rectangle sourceRectangle, Color color);

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
		/// Writes the text.
		/// </summary>
		/// <param name="font">The front.</param>
		/// <param name="text">The text.</param>
		/// <param name="coordinates">The coordinates.</param>
		public void Write(SpriteFont font, string text, Vector2 coordinates, Color color);

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

		/// <summary>
		/// Writes and draws the writable drawable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="writableDrawable">The writable drawable.</param>
		public void Draw(GameTime gameTime, IAmWriteableAndDrawable writableDrawable);
	}
}

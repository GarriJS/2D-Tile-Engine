using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.RunTime.Services.Contracts
{
	/// <summary>
	/// Represents a drawing service.
	/// </summary>
	public interface IDrawingService
	{
		/// <summary>
		/// Gets the sprite batch.
		/// </summary>
		public SpriteBatch SpriteBatch { get; }

		/// <summary>
		/// Gets the pixel.
		/// </summary>
		public Texture2D Pixel { get; }

		/// <summary>
		/// Begins the draw.
		/// </summary>
		public void BeginDraw();

		/// <summary>
		/// Ends the draw.
		/// </summary>
		public void EndDraw();

		/// <summary>
		/// Draws the texture. 
		/// </summary>
		/// <param name="texture">The texture.</param>
		/// <param name="drawCoordinates">The draw coordinates.</param>
		/// <param name="sourceRectangle">The source rectangle.</param>
		/// <param name="color">The color.</param>
		public void Draw(Texture2D texture, Vector2 drawCoordinates, Rectangle sourceRectangle, Color color);

		/// <summary>
		/// Draws the texture. 
		/// </summary>
		/// <param name="texture">The texture.</param>
		/// <param name="drawCoordinates">The draw coordinates.</param>
		/// <param name="sourceRectangle">The source rectangle.</param>
		/// <param name="stretchBox">The stretch box.</param>
		/// <param name="color">The color.</param>
		public void Draw(Texture2D texture, Vector2 drawCoordinates, Rectangle sourceRectangle, Vector2 stretchBox, Color color);

		/// <summary>
		/// Draws the rectangle.
		/// </summary>
		/// <param name="rectangle">The rectangle.</param>
		/// <param name="color">The color.</param>
		public void DrawRectangle(Rectangle rectangle, Color color);
	}
}

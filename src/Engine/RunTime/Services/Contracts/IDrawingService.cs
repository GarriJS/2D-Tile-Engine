using Engine.Drawables.Models.Contracts;
using Engine.Physics.Models;
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
		/// <param name="coordinates">The coordinates.</param>
		/// <param name="sourceRectangle">The source rectangle.</param>
		/// <param name="color">The color.</param>
		public void Draw(Texture2D texture, Vector2 coordinates, Rectangle sourceRectangle, Color color);

		/// <summary>
		/// Draws the texture. 
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="drawable">The drawable.</param>
		/// <param name="offset">The offset.</param>
		public void Draw(GameTime gameTime, IHaveAnImage drawable, Vector2 offset = default);

		/// <summary>
		/// Draws the sub drawable. 
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="subDrawable">The sub drawable.</param>
		/// <param name="position">The position.</param>
		/// <param name="offset">The offset.</param>
		public void Draw(GameTime gameTime, IAmSubDrawable subDrawable, Position position, Vector2 offset);
	}
}

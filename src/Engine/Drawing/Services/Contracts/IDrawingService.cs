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
	}
}

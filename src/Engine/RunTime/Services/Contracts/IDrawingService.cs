using Engine.Drawables.Models;
using Engine.Drawables.Models.Contracts;
using Engine.UI.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.RunTime.Services.Contracts
{
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
		/// Draws the animation.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="animation">The animation.</param>
		/// <param name="coordinates">The coordinates.</param>
		public void Draw(GameTime gameTime, Animation animation, Vector2 coordinates);

		/// <summary>
		/// Draws the user interface zone.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="uiZone">The user interface zone.</param>
		public void Draw(GameTime gameTime, UiZone uiZone);
	}
}

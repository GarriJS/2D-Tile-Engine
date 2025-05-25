using Engine.Drawables.Models;
using Engine.Drawables.Models.Contracts;
using Engine.Physics.Models;
using Engine.UI.Models.Contracts;
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

		/// <summary>
		/// Draws the animation.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="animation">The animation.</param>
		/// <param name="coordinates">The coordinates.</param>
		public void Draw(GameTime gameTime, Animation animation, Vector2 coordinates);

		/// <summary>
		/// Draws the user interface element.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="element">The element.</param>
		/// <param name="position">The position.</param>
		/// <param name="verticalOffset">The vertical offset.</param>
		/// <param name="horizontalOffset">The horizontal offset.</param>
		public void Draw(GameTime gameTime, IAmAUiElement element, Position position, float verticalOffset = 0, float horizontalOffset = 0);
	}
}

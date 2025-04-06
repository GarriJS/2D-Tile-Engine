using Engine.Drawables.Models;
using Engine.Drawables.Models.Contracts;
using Engine.Drawables.Services.Contracts;
using Engine.Physics.Models;
using Engine.RunTime.Services.Contracts;
using Engine.UI.Models.Contracts;
using Engine.UI.Models.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.RunTime.Services
{
	/// <summary>
	/// Represents a drawing service.
	/// </summary>
	public class DrawingService : IDrawingService
	{
		private readonly GameServiceContainer _gameServices;

		/// <summary>
		/// Gets the sprite batch.
		/// </summary>
		public SpriteBatch SpriteBatch { get; }

		/// <summary>
		/// Initializes a new instance of the drawing service.
		/// </summary>
		/// <param name="gameServices">The game services.</param>
		public DrawingService(GameServiceContainer gameServices)
		{
			this._gameServices = gameServices;
			var graphicDeviceService = this._gameServices.GetService<IGraphicsDeviceService>();
			this.SpriteBatch = new SpriteBatch(graphicDeviceService.GraphicsDevice);
		}

		/// <summary>
		/// Begins the draw.
		/// </summary>
		public void BeginDraw()
		{
			this.SpriteBatch.Begin();
		}

		/// <summary>
		/// Ends the draw.
		/// </summary>
		public void EndDraw()
		{
			this.SpriteBatch.End();
		}

		/// <summary>
		/// Draws the drawable. 
		/// </summary>
		/// <param name="texture">The texture.</param>
		/// <param name="coordinates">The coordinates.</param>
		/// <param name="sourceRectangle">The source rectangle.</param>
		/// <param name="color">The color.</param>
		private void Draw(Texture2D texture, Vector2 coordinates, Rectangle sourceRectangle, Color color)
		{
			this.SpriteBatch.Draw(texture, coordinates, sourceRectangle, color);
		}

		/// <summary>
		/// Draws the drawable. 
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="drawable">The drawable.</param>
		/// <param name="offset">The offset.</param>
		public void Draw(GameTime gameTime, IAmDrawable drawable, Vector2 offset = default)
		{
			this.SpriteBatch.Draw(drawable.Image.Texture, drawable.Position.Coordinates + offset, drawable.Image.TextureBox, Color.White);
		}

		/// <summary>
		/// Draws the sub drawable. 
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="subDrawable">The sub drawable.</param>
		/// <param name="position">The position.</param>
		/// <param name="offset">The offset.</param>
		public void Draw(GameTime gameTime, IAmSubDrawable subDrawable, Position position, Vector2 offset)
		{
			this.SpriteBatch.Draw(subDrawable.Image.Texture, position.Coordinates + offset, subDrawable.Image.TextureBox, Color.White);
		}

		/// <summary>
		/// Draws the animation.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="animation">The animation.</param>
		/// <param name="coordinates">The coordinates.</param>
		public void Draw(GameTime gameTime, Animation animation, Vector2 coordinates)
		{
			this.Draw(animation.CurrentFrame.Texture, coordinates, animation.CurrentFrame.TextureBox, Color.White);
			var animationService = this._gameServices.GetService<IAnimationService>();
			animationService.UpdateAnimationFrame(gameTime, animation);
		}

		/// <summary>
		/// Draws the user interface element.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="element">The element.</param>
		/// <param name="position">The position.</param>
		/// <param name="verticalOffset">The vertical offset.</param>
		/// <param name="horizontalOffset">The horizontal offset.</param>
		public void Draw(GameTime gameTime, IAmAUiElement element, Position position, float verticalOffset = 0, float horizontalOffset = 0)
		{
			var offset = new Vector2(horizontalOffset, verticalOffset);
			this.Draw(element.Image.Texture, position.Coordinates + offset, element.Image.TextureBox, Color.White);

			if (element is UiButton button)
			{
				if (null != button.ClickAnimation)
				{
					var clickableOffset = new Vector2((button.Area.X - button.ClickableArea.X) / 2, (button.Area.Y - button.ClickableArea.Y) / 2);
					this.Draw(gameTime, button.ClickAnimation, position.Coordinates + offset + clickableOffset);
				}
			}

			if ((element is IAmAUiElementWithText elementWithText) &&
				(false == string.IsNullOrEmpty(elementWithText.Text)))
			{
				var writingService = this._gameServices.GetService<IWritingService>();
				var textMeasurements = writingService.MeasureString("Monobold", elementWithText.Text);
				var textPosition = position.Coordinates + offset + (element.Area / 2) - (textMeasurements / 2);
				writingService.Draw("Monobold", elementWithText.Text, textPosition, Color.Maroon);
			}
		}
	}
}

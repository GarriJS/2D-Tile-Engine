using Engine.Controls.Typing;
using Engine.Drawing.Models;
using Engine.Drawing.Models.Contracts;
using Engine.Drawing.Services.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Drawing.Services
{
	/// <summary>
	/// Represents a drawing service.
	/// </summary>
	public class DrawingService : IDrawingService
    {
		private readonly GameServiceContainer _gameServiceContainer;
        
		/// <summary>
		/// Gets the sprite batch.
		/// </summary>
		private SpriteBatch SpriteBatch { get; }

		/// <summary>
		/// Initializes a new instance of the drawing service.
		/// </summary>
		/// <param name="gameServices">The game services.</param>
        public DrawingService(GameServiceContainer gameServices)
		{
			this._gameServiceContainer = gameServices;
			var graphicDeviceService = this._gameServiceContainer.GetService<IGraphicsDeviceService>();
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
		/// <param name="gameTime">The game time.</param>
		/// <param name="drawable">The drawable.</param>
		public void Draw(GameTime gameTime, IAmDrawable drawable)
		{
			this.SpriteBatch.Draw(drawable.Sprite.Texture, drawable.Position.Coordinates, drawable.Sprite.TextureBox, Color.White);
		}

		/// <summary>
		/// Draws the animated.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="animated">The animated.</param>
		public void Draw(GameTime gameTime, IAmAnimated animated)
		{
			var animationService = this._gameServiceContainer.GetService<IAnimationService>();
			animationService.UpdateAnimationFrame(gameTime, animated.Animation);
			this.SpriteBatch.Draw(animated.Sprite.Texture, animated.Position.Coordinates, animated.Sprite.TextureBox, Color.White);
		}

		/// <summary>
		/// Draws the animated.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="animation">The animation.</param>
		/// <param name="coordinates">The coordinates.</param>
		/// <param name="color">The color.</param>
		public void Draw(GameTime gameTime, Animation animation, Vector2 coordinates, Color color)
		{
			var animationService = this._gameServiceContainer.GetService<IAnimationService>();
			animationService.UpdateAnimationFrame(gameTime, animation);
			this.SpriteBatch.Draw(animation.CurrentFrame.Texture, coordinates, animation.CurrentFrame.TextureBox, color);
		}

		/// <summary>
		/// Writes the drawable text.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="drawableText">The drawable text.</param>
		public void Write(GameTime gameTime, DrawableText drawableText)
		{
			var formattedText = KeyboardTyping.FormatForDrawString(drawableText.Text);
			
			this.SpriteBatch.DrawString(drawableText.SpriteFont, formattedText, drawableText.Position.Coordinates, Color.White);
		}

		/// <summary>
		/// Writes the drawable text.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="drawableText">The drawable text.</param>
		/// <param name="color">The color.</param>
		public void Write(GameTime gameTime, DrawableText drawableText, Color color)
		{
			var formattedText = KeyboardTyping.FormatForDrawString(drawableText.Text);

			this.SpriteBatch.DrawString(drawableText.SpriteFont, formattedText, drawableText.Position.Coordinates, color);
		}
	}
}

using Engine.Controls.Typing;
using Engine.Drawing.Models;
using Engine.Drawing.Models.Contracts;
using Engine.Drawing.Services.Contracts;
using Engine.UserInterface.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Drawing.Services
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
		private SpriteBatch SpriteBatch { get; }

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
		public void Draw(Texture2D texture, Vector2 coordinates, Rectangle sourceRectangle, Color color)
		{
			this.SpriteBatch.Draw(texture, coordinates, sourceRectangle, color);
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
			var animationService = this._gameServices.GetService<IAnimationService>();
			animationService.UpdateAnimationFrame(gameTime, animated.Animation);
			this.SpriteBatch.Draw(animated.Sprite.Texture, animated.Position.Coordinates, animated.Sprite.TextureBox, Color.White);
		}

		/// <summary>
		/// Draws the text line collection.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="textLineCollection">The text line collection.</param>
		public void Draw(GameTime gameTime, TextLineCollection textLineCollection)
		{
			this.Draw(textLineCollection.Sprite.Texture, new Vector2(0, 0), textLineCollection.Sprite.TextureBox, Color.White);

			var drawCoordinates = new Vector2(
				textLineCollection.TextOffset.X,
				textLineCollection.TextOffset.Y);

			foreach (var textLine in textLineCollection.TextLines)
			{
				var textureBox = new Rectangle(
					textLine.Sprite.TextureBox.X,
					textLine.Sprite.TextureBox.Y,
					textLine.Sprite.TextureBox.Width - (int)drawCoordinates.X,
					textLine.Sprite.TextureBox.Height);

				this.Draw(textLine.Sprite.Texture, drawCoordinates, textureBox, Color.White);

				drawCoordinates.Y += textLine.Sprite.SpritesheetBox.Height;
			}
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
			var animationService = this._gameServices.GetService<IAnimationService>();
			animationService.UpdateAnimationFrame(gameTime, animation);
			this.SpriteBatch.Draw(animation.CurrentFrame.Texture, coordinates, animation.CurrentFrame.TextureBox, color);
		}

		/// <summary>
		/// Writes the text.
		/// </summary>
		/// <param name="font">The front.</param>
		/// <param name="text">The text.</param>
		/// <param name="coordinates">The coordinates.</param>
		public void Write(SpriteFont font, string text, Vector2 coordinates, Color color)
		{
			var formattedText = KeyboardTyping.FormatForDrawString(text);

			this.SpriteBatch.DrawString(font, formattedText, coordinates, color);
		}

		/// <summary>
		/// Writes the drawable text.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="drawableText">The drawable text.</param>
		public void Write(GameTime gameTime, DrawableText drawableText)
		{
			var formattedText = KeyboardTyping.FormatForDrawString(drawableText.Text);
			
			this.SpriteBatch.DrawString(drawableText.Font, formattedText, drawableText.Position.Coordinates, Color.White);
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

			this.SpriteBatch.DrawString(drawableText.Font, formattedText, drawableText.Position.Coordinates, color);
		}

		/// <summary>
		/// Writes and draws the writable drawable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="writableDrawable">The writable drawable.</param>
		public void Draw(GameTime gameTime, IAmWriteableAndDrawable writableDrawable)
		{
			this.SpriteBatch.Draw(writableDrawable.Sprite.Texture, writableDrawable.Position.Coordinates, writableDrawable.Sprite.TextureBox, Color.White);
		}
	}
}

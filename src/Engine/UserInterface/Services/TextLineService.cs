using Engine.Drawing.Models;
using Engine.Drawing.Services.Contracts;
using Engine.UserInterface.Models;
using Engine.UserInterface.Services.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Engine.UserInterface.Services
{
	/// <summary>
	/// Represents a text line service.
	/// </summary>
	/// <remarks>
	/// Initializes a new instance of the text line service.
	/// </remarks>
	/// <param name="gameServices">The game services.</param>
	public class TextLineService(GameServiceContainer gameServices) : ITextLineService
	{
		private readonly GameServiceContainer _gameServices = gameServices;

		/// <summary>
		/// Moves the text line view area.
		/// </summary>
		/// <param name="textLine">The text line.</param>
		/// <param name="rightMostViewableCharIndex">The right mot viewable char index.</param>
		public void MoveTextLineViewArea(SubTextLine textLine, int rightMostViewableCharIndex)
		{
			var textSize = textLine.Font.MeasureString(textLine.Text);
			
			if (textSize.X <= textLine.Width - textLine.TextBuffer.X)
			{
				return;
			}

			var textSizeToViewableChar = textLine.Font.MeasureString(textLine.Text[..rightMostViewableCharIndex]);
			this.UpdateTextLineSprite(textLine, (int)Math.Abs(textLine.Width - textSizeToViewableChar.X));
		}

		/// <summary>
		/// Updates the text line sprite.
		/// </summary>
		/// <param name="textLine">The text line.</param>
		public void UpdateTextLineSprite(SubTextLine textLine, int viewableTextOffset = 0)
		{
			var textSize = textLine.Font.MeasureString(textLine.Text);
			var graphicsDeviceService = this._gameServices.GetService<IGraphicsDeviceService>();
			var drawingService = this._gameServices.GetService<IDrawingService>();
			var textTextureHeight = (int)(textSize.Y + (textLine.TextBuffer.Y * 2));
			var textTextureWidth = (int)(textSize.X + textLine.TextBuffer.X * 2);
			var textTexture = new RenderTarget2D(
				graphicsDeviceService.GraphicsDevice,
				textTextureWidth,
				textTextureHeight);

			graphicsDeviceService.GraphicsDevice.SetRenderTarget(textTexture);
			drawingService.BeginDraw();

			drawingService.Draw(textLine.Background.Texture, new Vector2(0, 0), textLine.Background.TextureBox, Color.White);
			drawingService.Write(textLine.Font, textLine.Text, new Vector2(0, 0), Color.White);

			drawingService.EndDraw();

			var spriteTexture = new RenderTarget2D(
				graphicsDeviceService.GraphicsDevice,
				textLine.Width,
				textTextureHeight);
			textLine.Sprite ??= new Sprite
			{
				SpritesheetCoordinate = new Point(0, 0),
				SpritesheetName = "Generated_TextLine",
				TextureName = "Generated_TextLine",
				TextureBox = new Rectangle(0, 0, textLine.Width, textTextureHeight),
				SpritesheetBox = new Rectangle(0, 0, textLine.Width, textTextureHeight)
			};

			graphicsDeviceService.GraphicsDevice.SetRenderTarget(spriteTexture);
			drawingService.BeginDraw();

			drawingService.Draw(textLine.Background.Texture, new Vector2(0, 0), textLine.Background.TextureBox, Color.White);
			drawingService.Draw(textTexture, textLine.TextBuffer, new Rectangle(viewableTextOffset, 0, textTextureWidth, textTextureHeight), Color.White);

			drawingService.EndDraw();
			graphicsDeviceService.GraphicsDevice.SetRenderTarget(null);

			var finalTexture = new Texture2D(graphicsDeviceService.GraphicsDevice, textLine.Width, textTextureHeight);
			Color[] data = new Color[textLine.Width * textTextureHeight];
			spriteTexture.GetData(data);
			finalTexture.SetData(data);

			textTexture.Dispose();
			spriteTexture.Dispose();
			textLine.Sprite.Dispose();

			textLine.Sprite.Texture = finalTexture;
		}
	}
}

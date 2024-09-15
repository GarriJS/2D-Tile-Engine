using Engine.Drawing.Models;
using Engine.Drawing.Services.Contracts;
using Engine.UserInterface.Models;
using Engine.UserInterface.Services.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

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
		/// <param name="stringIndex">The string index.</param>
		public void MoveTextLineViewArea(SubTextLine textLine, int stringIndex)
		{
			if (stringIndex < 0)
			{ 
				stringIndex = 0;
			}

			if (stringIndex >= textLine.Text.Length)
			{ 
				stringIndex = textLine.Text.Length - 1;
			}

			var textSize = textLine.Font.MeasureString(textLine.Text);
			var newPosition = textLine.Sprite?.TextureBox.X;

			if (true == newPosition.HasValue)
			{
				newPosition = newPosition.Value + 0;

				if (0 > newPosition)
				{
					newPosition = 0;
				}
				else if (newPosition > textSize.X + (textLine.TextBuffer.X * 2) + 4 - textLine.Width)
				{
					newPosition = (int)(textSize.X + (textLine.TextBuffer.X * 2) + 4 - textLine.Width);
				}

				if (newPosition == textLine.Sprite?.TextureBox.X)
				{
					return;
				}
			}

			var textureHeight = (int)(textSize.Y + (textLine.TextBuffer.Y * 2));
			textLine.Sprite ??= new Sprite
			{
				SpritesheetCoordinate = new Point(0, 0),
				SpritesheetName = "Generated_TextInputLine",
				TextureName = "Generated_TextInputLine",   
				TextureBox = new Rectangle(0, 0, textLine.Width, textureHeight),
				SpritesheetBox = new Rectangle(0, 0, textLine.Width, textureHeight)
			};

			textLine.Sprite.TextureBox = new Rectangle
			{
				X = newPosition ?? 0,
				Y = 0,
				Width = textLine.Width,
				Height = textureHeight,
			};
		}

		/// <summary>
		/// Updates the text line sprite.
		/// </summary>
		/// <param name="textLine">The text line.</param>
		/// <param name="newText">The new text.</param>
		/// <param name="stringIndex">The string index.</param>
		public void UpdateTextLineSprite(SubTextLine textLine, string newText = null, int? stringIndex = null)
		{
			var textSize = textLine.Font.MeasureString(textLine.Text);

			if (null != newText)
			{
				textLine.Text = newText;
				var newTextSize = textLine.Font.MeasureString(newText);
				this.MoveTextLineViewArea(textLine, (int)(newTextSize.X - textLine.Width + (textLine.TextBuffer.X * 2)) + 4 - textLine.Width);
				textSize = newTextSize;
			}

			var graphicsDeviceService = this._gameServices.GetService<IGraphicsDeviceService>();
			var drawingService = this._gameServices.GetService<IDrawingService>();
			var textureHeight = (int)(textSize.Y + (textLine.TextBuffer.Y * 2));
			var textWidth = textLine.Width - (int)(textLine.TextBuffer.X * 2);
			var textTexture = new RenderTarget2D(
				graphicsDeviceService.GraphicsDevice,
				textWidth,
				textureHeight);

			graphicsDeviceService.GraphicsDevice.SetRenderTarget(textTexture);
			drawingService.BeginDraw();

			drawingService.Draw(textLine.Background.Texture, new Vector2(0, 0), textLine.Background.TextureBox, Color.White);
			drawingService.Write(textLine.Font, textLine.Text, new Vector2(0, 0), Color.White);

			drawingService.EndDraw();

			var spriteTexture = new RenderTarget2D(
				graphicsDeviceService.GraphicsDevice,
				textLine.Width,
				textureHeight);
			textLine.Sprite ??= new Sprite
			{
				SpritesheetCoordinate = new Point(0, 0),
				SpritesheetName = "Generated_TextInputLine",
				TextureName = "Generated_TextInputLine",
				TextureBox = new Rectangle(0, 0, textLine.Width, textureHeight),
				SpritesheetBox = new Rectangle(0, 0, textLine.Width, textureHeight)
			};

			graphicsDeviceService.GraphicsDevice.SetRenderTarget(spriteTexture);
			drawingService.BeginDraw();

			drawingService.Draw(textLine.Background.Texture, new Vector2(0, 0), textLine.Background.TextureBox, Color.White);
			drawingService.Draw(textTexture, textLine.TextBuffer, new Rectangle(0, 0, textWidth, textureHeight), Color.White);

			drawingService.EndDraw();
			graphicsDeviceService.GraphicsDevice.SetRenderTarget(null);

			var finalTexture = new Texture2D(graphicsDeviceService.GraphicsDevice, textLine.Width, textureHeight);
			Color[] data = new Color[textLine.Width * textureHeight];
			spriteTexture.GetData(data);
			finalTexture.SetData(data);

			textTexture.Dispose();
			spriteTexture.Dispose();
			textLine.Sprite.Dispose();

			textLine.Sprite.Texture = finalTexture;
		}

		/// <summary>
		/// Updates the text line collection sprite.
		/// </summary>
		/// <param name="textLineCollection">The text line collection.</param>
		/// <param name="newTextLine">The new text line.</param>
		/// <param name="frontAdd">A value indicating whether to do a front add.</param>
		/// <param name="bottomToTop">A value indicating whether the text line collection is populated bottom to top.</param>
		public void UpdateTextLineCollectionSprite(TextLineCollection textLineCollection, SubTextLine newTextLine = null, bool frontAdd = false, bool bottomToTop = false)
		{
			var horizontalSize = 0;
			var verticalSize = 0;

			if (null == textLineCollection.TextLines)
			{
				textLineCollection.TextLines = new List<SubTextLine>();
			}

			if (null != newTextLine)
			{
				if (true == frontAdd)
				{
					textLineCollection.TextLines.Insert(0, newTextLine);
				}
				else
				{ 
					textLineCollection.TextLines.Add(newTextLine);
				}
			}

			foreach (var textLine in textLineCollection.TextLines)
			{
				if (null == textLine.Sprite)
				{
					this.UpdateTextLineSprite(textLine);
				}

				if (horizontalSize < textLine.Sprite.SpritesheetBox.Width)
				{
					horizontalSize = textLine.Sprite.SpritesheetBox.Width;
				}

				verticalSize += textLine.Sprite.SpritesheetBox.Height;
			}

			var graphicsDeviceService = this._gameServices.GetService<IGraphicsDeviceService>();
			var drawingService = this._gameServices.GetService<IDrawingService>();

			if (horizontalSize < textLineCollection.Width)
			{ 
				horizontalSize = textLineCollection.Width;
			}

			if (verticalSize < textLineCollection.Height)
			{ 
				verticalSize = textLineCollection.Height;
			}

			var texture = new RenderTarget2D(
				graphicsDeviceService.GraphicsDevice,
				horizontalSize,
				verticalSize);
			textLineCollection.Sprite ??= new Sprite
			{
				SpritesheetCoordinate = new Point(0, 0),
				SpritesheetName = "Generated_TextInputLine",
				TextureName = "Generated_TextInputLine",
				TextureBox = new Rectangle(0, 0, textLineCollection.Width, textLineCollection.Height),
				SpritesheetBox = new Rectangle(0, 0, textLineCollection.Width, textLineCollection.Height)
			};

			graphicsDeviceService.GraphicsDevice.SetRenderTarget(texture);
			drawingService.BeginDraw();

			var drawCoordinates = new Vector2(
				textLineCollection.TextOffset.X,
				true == bottomToTop ? textLineCollection.Height - textLineCollection.TextOffset.Y : textLineCollection.TextOffset.Y
			);

			drawingService.Draw(textLineCollection.Background.Texture, new Vector2(0, 0), textLineCollection.Background.TextureBox, Color.White);

			foreach (var textLine in textLineCollection.TextLines)
			{			
				if (true == bottomToTop)
				{
					drawCoordinates.Y -= textLine.Sprite.SpritesheetBox.Height;
				}

				drawingService.Draw(textLine.Sprite.Texture, drawCoordinates, textLine.Sprite.TextureBox, Color.White);

				if (false == bottomToTop)
				{
					drawCoordinates.Y += textLine.Sprite.SpritesheetBox.Height;
				}
			}

			drawingService.EndDraw();
			graphicsDeviceService.GraphicsDevice.SetRenderTarget(null);

			textLineCollection.Sprite.Dispose();
			textLineCollection.Sprite.Texture = texture;
		}
	}
}

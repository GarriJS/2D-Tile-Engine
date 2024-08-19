using Engine.Drawing.Models;
using Engine.Drawing.Services.Contracts;
using Engine.UserInterface.Models;
using Engine.UserInterface.Services.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.UserInterface.Services
{
	/// <summary>
	/// Represents a text line service.
	/// </summary>
	/// <remarks>
	/// Initializes a new instance of the text line service.
	/// </remarks>
	/// <param name="gameServices">The game services.</param>
	public class TextLineService(GameServiceContainer gameServices) : ITextInputLineService
	{
		private readonly GameServiceContainer _gameServiceContainer = gameServices;

		/// <summary>
		/// Moves the text line view area.
		/// </summary>
		/// <param name="textLine">The text line.</param>
		/// <param name="horizontalMoveAmount">The horizontal move amount.</param>
		public void MoveTextLineViewArea(TextLine textLine, int horizontalMoveAmount)
		{
			var textSize = textLine.Font.MeasureString(textLine.Text);
			var newPosition = textLine.Sprite?.TextureBox.X;

			if (true == newPosition.HasValue)
			{
				newPosition = newPosition.Value + horizontalMoveAmount;

				if (0 > newPosition)
				{
					newPosition = 0;
				}
				else if (newPosition > textSize.X + (textLine.TextOffset.X * 2) + 2 - textLine.MaxVisibleTextWidth)
				{
					newPosition = (int)(textSize.X + (textLine.TextOffset.X * 2) + 2 - textLine.MaxVisibleTextWidth);
				}

				if (newPosition == textLine.Sprite?.TextureBox.X)
				{
					return;
				}
			}

			textLine.Sprite ??= new Sprite
			{
				SpritesheetCoordinate = new Point(0, 0),
				SpritesheetName = "Generated_TextInputLine",
				TextureName = "Generated_TextInputLine"
			};

			textLine.Sprite.TextureBox = new Rectangle
			{
				X = newPosition ?? 0,
				Y = 0,
				Width = textLine.MaxVisibleTextWidth,
				Height = (int)(textSize.Y + (textLine.TextOffset.Y * 2)),
			};
		}

		/// <summary>
		/// Updates the text line sprite.
		/// </summary>
		/// <param name="textLine">The text line.</param>
		/// <param name="newText">The new text.</param>
		public void UpdateTextLineSprite(TextLine textLine, string newText = null)
		{
			var textSize = textLine.Font.MeasureString(textLine.Text);

			if (null != newText)
			{
				textLine.Text = newText;
				var newTextSize = textLine.Font.MeasureString(newText);
				this.MoveTextLineViewArea(textLine, (int)(newTextSize.X - textLine.MaxVisibleTextWidth + (textLine.TextOffset.X * 2)) + 2 - textLine.Sprite.TextureBox.X);
				textSize = newTextSize;
			}
			else
			{
				this.MoveTextLineViewArea(textLine, (int)(textSize.X + (textLine.TextOffset.X * 2)));
			}

			var graphicsDeviceService = this._gameServiceContainer.GetService<IGraphicsDeviceService>();
			var drawingService = this._gameServiceContainer.GetService<IDrawingService>();
			
			var texture = new RenderTarget2D(
				graphicsDeviceService.GraphicsDevice,
				(int)(textSize.X + (textLine.TextOffset.X * 2) + 2),
				(int)(textSize.Y + (textLine.TextOffset.Y * 2)));
			textLine.Sprite ??= new Sprite
			{
				SpritesheetCoordinate = new Point(0, 0),
				SpritesheetName = "Generated_TextInputLine",
				TextureName = "Generated_TextInputLine"
			};

			graphicsDeviceService.GraphicsDevice.SetRenderTarget(texture);
			drawingService.BeginDraw();

			drawingService.Draw(textLine.Background.Texture, new Vector2(), textLine.Background.TextureBox, Color.White);
			drawingService.Write(textLine.Font, textLine.Text, textLine.TextOffset + new Vector2(2, 0), Color.White);

			drawingService.EndDraw();
			graphicsDeviceService.GraphicsDevice.SetRenderTarget(null);

			textLine.Sprite.Dispose();
			textLine.Sprite.Texture = texture;
		}
	}
}

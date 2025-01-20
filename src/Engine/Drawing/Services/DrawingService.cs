using Engine.Drawing.Models.Contracts;
using Engine.Drawing.Services.Contracts;
using Engine.Physics.Models;
using Engine.UI.Models;
using Engine.UI.Models.Contracts;
using Engine.UI.Models.Elements;
using Engine.UI.Models.Enums;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

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
			this.SpriteBatch.Draw(drawable.Image.Texture, drawable.Position.Coordinates, drawable.Image.TextureBox, Color.White);
		}

		/// <summary>
		/// Draws the user interface zone.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="uiZone">The user interface zone.</param>
		public void Draw(GameTime gameTime, UiZone uiZone)
		{
			if (null != uiZone.Image)
			{
				this.SpriteBatch.Draw(uiZone.Image.Texture, uiZone.Position.Coordinates, uiZone.Image.TextureBox, Color.White);
			}

			if (true != uiZone.ElementRows?.Any())
			{
				return;
			}

			var height = uiZone.ElementRows.Sum(e => e.Height + e.BottomPadding + e.TopPadding);
			var rowVerticalOffset = uiZone.JustificationType switch
			{
				UiZoneJustificationTypes.None => 0,
				UiZoneJustificationTypes.Center => (uiZone.Area.Height - height) / 2,
				UiZoneJustificationTypes.Top => 0,
				UiZoneJustificationTypes.BottomReverseWrap => uiZone.Area.Height - height,
				_ => 0,
			};

			foreach (var elementRow in uiZone.ElementRows)
			{
				switch (uiZone.JustificationType)
				{
					case UiZoneJustificationTypes.BottomReverseWrap:
					case UiZoneJustificationTypes.Center:
					case UiZoneJustificationTypes.None:
					case UiZoneJustificationTypes.Top:
					default:
						rowVerticalOffset += elementRow.TopPadding;
						this.Draw(gameTime, elementRow, uiZone.Position, rowVerticalOffset);
						rowVerticalOffset += (elementRow.BottomPadding + elementRow.Height);
						break;
				}
			}
		}

		/// <summary>
		/// Draws the user interface row.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="uiRow">The user interface row.</param>
		/// <param name="position">The position.</param>
		/// <param name="heightOffset">The height offset.</param>
		private void Draw(GameTime gameTime, UiRow uiRow, Position position, float heightOffset)
		{
			if (null != uiRow.Image)
			{
				this.SpriteBatch.Draw(uiRow.Image.Texture, position.Coordinates + new Vector2(0, heightOffset - uiRow.TopPadding), uiRow.Image.TextureBox, Color.White);
			}

			if (true != uiRow?.SubElements.Any())
			{
				return;
			}

			var width = uiRow.SubElements.Sum(e => e.Area.X + e.LeftPadding + e.RightPadding);
			var elementHorizontalOffset = uiRow.HorizontalJustificationType switch
			{
				UiRowHorizontalJustificationTypes.None => 0,
				UiRowHorizontalJustificationTypes.Center => (uiRow.Width - width) / 2,
				UiRowHorizontalJustificationTypes.Left => 0,
				UiRowHorizontalJustificationTypes.RightReverseWrap => uiRow.Width,
				_ => 0,
			};

			var largestHeight = uiRow.SubElements.OrderByDescending(e => e.Area.Y)
												 .FirstOrDefault().Area.Y;

			foreach (var element in uiRow.SubElements)
			{
				var verticallyCenterOffset = 0f;

				switch (uiRow.VerticalJustificationType)
				{
					case UiRowVerticalJustificationTypes.Bottom:
						verticallyCenterOffset = (largestHeight - element.Area.Y);
						break;
					case UiRowVerticalJustificationTypes.Center:
						verticallyCenterOffset = (largestHeight - element.Area.Y) / 2;
						break;
					case UiRowVerticalJustificationTypes.None:
					case UiRowVerticalJustificationTypes.Top:
						break;
				}

				switch (uiRow.HorizontalJustificationType)
				{
					case UiRowHorizontalJustificationTypes.RightReverseWrap:
						elementHorizontalOffset -= (element.RightPadding + element.Area.X);
						this.Draw(gameTime, element, position, new Vector2(elementHorizontalOffset, heightOffset + verticallyCenterOffset));
						elementHorizontalOffset -= element.LeftPadding;
						break;
					case UiRowHorizontalJustificationTypes.Center:
					case UiRowHorizontalJustificationTypes.None:
					case UiRowHorizontalJustificationTypes.Left:
					default:
						elementHorizontalOffset += element.LeftPadding;
						this.Draw(gameTime, element, position, new Vector2(elementHorizontalOffset, heightOffset + verticallyCenterOffset));
						elementHorizontalOffset += (element.RightPadding + element.Area.X);
						break;
				}
			}
		}

		/// <summary>
		/// Draws the user interface element.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="element">The element.</param>
		/// <param name="position">The position.</param>
		/// <param name="offset">The offset.</param>
		private void Draw(GameTime gameTime, IAmAUiElement element, Position position, Vector2 offset)
		{
			this.Draw(element.Image.Texture, position.Coordinates + offset, element.Image.TextureBox, Color.White);

			if (element is UiButton button)
			{
				if (false == string.IsNullOrEmpty(button.ButtonText))
				{
					var writingService = this._gameServices.GetService<IWritingService>();
					var textMeasurements = writingService.MeasureString("Monobold", button.ButtonText);
					var textPosition = position.Coordinates + offset + (element.Area / 2) - (textMeasurements / 2);
					writingService.Draw("Monobold", button.ButtonText, textPosition, Color.Maroon);
				}
			}
		}
	}
}

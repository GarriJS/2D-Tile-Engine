using Common.Controls.CursorInteraction.Models;
using Common.Controls.CursorInteraction.Models.Abstract;
using Common.Controls.CursorInteraction.Models.Contracts;
using Common.UserInterface.Enums;
using Common.UserInterface.Models.Contracts;
using Engine.Graphics.Models;
using Engine.Graphics.Services.Contracts;
using Engine.Physics.Models;
using Engine.RunTime.Services.Contracts;
using Microsoft.Xna.Framework;

namespace Common.UserInterface.Models.Elements
{
    /// <summary>
    /// Represents a user interface button.
    /// </summary>
    public class UiButton : IAmAUiElementWithText, ICanBeClicked<IAmAUiElement>
	{
		/// <summary>
		/// Gets or sets the user interface element name.
		/// </summary>
		public string UiElementName { get; set; }

		/// <summary>
		/// Gets or sets the visibility group.
		/// </summary>
		public int VisibilityGroup { get; set; }

		/// <summary>
		/// Gets or sets the text.
		/// </summary>
		public string Text { get; set; }

		/// <summary>
		/// Gets or sets the left padding.
		/// </summary>
		public float LeftPadding { get; set; }

		/// <summary>
		/// Gets or sets the right padding.
		/// </summary>
		public float RightPadding { get; set; }

		/// <summary>
		/// Gets or sets the user interface element type.
		/// </summary>
		public UiElementTypes ElementType { get; set; }

		/// <summary>
		/// Gets or sets the user interface size type.
		/// </summary>
		public UiElementSizeTypes SizeType { get; set; }

		/// <summary>
		/// Gets or sets the clickable area scaler.
		/// </summary>
		public Vector2 ClickableAreaScaler { get; set; }

		/// <summary>
		/// Gets or sets the area.
		/// </summary>
		public Vector2 Area { get; set; }

		/// <summary>
		/// Gets the graphic.
		/// </summary>
		public Image Graphic { get; set; }

		/// <summary>
		/// Gets or sets the clickable image.
		/// </summary>
		public Image ClickableImage { get => this.ClickAnimation?.CurrentFrame; }

		/// <summary>
		/// Gets or sets the clickable animation.
		/// </summary>
		public TriggeredAnimation ClickAnimation { get; set; }

		/// <summary>
		/// Gets the base hover configuration.
		/// </summary>
		public BaseHoverConfiguration BaseHoverConfig { get => this.HoverConfig; }

		/// <summary>
		/// Gets or sets the hover configuration.
		/// </summary>
		public HoverConfiguration<IAmAUiElement> HoverConfig { get; set; }

		/// <summary>
		/// Gets or sets the press configuration.
		/// </summary>
		public PressConfiguration<IAmAUiElement> PressConfig { get; set; }

		/// <summary>
		/// Gets or sets the click configuration.
		/// </summary>
		public ClickConfiguration<IAmAUiElement> ClickConfig { get; set; }

		/// <summary>
		/// Raises the hover event.
		/// </summary>
		/// <param name="elementLocation">The element location.</param>
		public void RaiseHoverEvent(Vector2 elementLocation)
		{
			this.HoverConfig?.RaiseHoverEvent(this, elementLocation);
		}

		/// <summary>
		/// Raises the press event.
		/// </summary>
		/// <param name="elementLocation">The element location.</param>
		public void RaisePressEvent(Vector2 elementLocation)
		{
			this.PressConfig?.RaisePressEvent(this, elementLocation);
		}

		/// <summary>
		/// Raises the click event.
		/// </summary>
		/// <param name="elementLocation">The element location.</param>
		public void RaiseClickEvent(Vector2 elementLocation)
		{
			this.ClickConfig?.RaiseClickEvent(this, elementLocation);
		}

		/// <summary>
		/// Draws the sub drawable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		/// <param name="position">The position.</param>
		/// <param name="offset">The offset.</param>
		public void Draw(GameTime gameTime, GameServiceContainer gameServices, Position position, Vector2 offset = default)
		{
			var drawingService = gameServices.GetService<IDrawingService>();
			var writingService = gameServices.GetService<IWritingService>();

			drawingService.Draw(this.Graphic.Texture, position.Coordinates + offset, this.Graphic.TextureBox, Color.White);

			if (null != this.ClickAnimation)
			{
				this.ClickAnimation.Draw(gameTime, gameServices, position, offset + this.ClickConfig.Offset);
			}

			if (false == string.IsNullOrEmpty(this.Text))
			{
				var textMeasurements = writingService.MeasureString("Monobold", this.Text);
				var textPosition = position.Coordinates + offset + (this.Area / 2) - (textMeasurements / 2);
				writingService.Draw("Monobold", this.Text, textPosition, Color.Maroon);
			}
		}

		/// <summary>
		/// Disposes of the user interface button.
		/// </summary>
		public void Dispose()
		{
			this.Graphic?.Dispose();
			this.HoverConfig?.Dispose();
			this.PressConfig?.Dispose();
			this.ClickConfig?.Dispose();
		}
	}
}

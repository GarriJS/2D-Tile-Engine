using Common.Controls.CursorInteraction.Models;
using Common.Controls.CursorInteraction.Models.Abstract;
using Common.Controls.CursorInteraction.Models.Contracts;
using Common.UserInterface.Enums;
using Common.UserInterface.Models.Contracts;
using Engine.Graphics.Models;
using Engine.Physics.Models;
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
		/// Gets or sets the graphic text.
		/// </summary>
		public GraphicalText GraphicText { get; set; }

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
		/// <param name="pressLocation">The press location.</param>
		public void RaisePressEvent(Vector2 elementLocation, Vector2 pressLocation)
		{
			this.PressConfig?.RaisePressEvent(this, elementLocation, pressLocation);
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
			var textMeasurements = this.GraphicText?.GetTextDimensions() ?? default;
			var finalOffset = new Vector2
			{
				X = offset.X + (this.Area.X / 2) - (textMeasurements.X / 2) + this.LeftPadding,
				Y = offset.Y + (this.Area.Y / 2) - (textMeasurements.Y / 2)
			};
			this.Graphic?.Draw(gameTime, gameServices, position, offset);
			this.GraphicText?.Draw(gameTime, gameServices, position, finalOffset);
			this.ClickAnimation?.Draw(gameTime, gameServices, position, offset);
		}

		/// <summary>
		/// Disposes of the user interface button.
		/// </summary>
		public void Dispose()
		{
			this.HoverConfig?.Dispose();
			this.PressConfig?.Dispose();
			this.ClickConfig?.Dispose();
		}
	}
}

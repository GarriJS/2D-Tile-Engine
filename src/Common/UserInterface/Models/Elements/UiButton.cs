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
		/// Gets the inside width.
		/// </summary>
		public float InsideWidth { get => this.InsidePadding.LeftPadding + this.Area.X + this.InsidePadding.RightPadding; }

		/// <summary>
		/// Gets the inside height.
		/// </summary>
		public float InsideHeight { get => this.InsidePadding.TopPadding + this.Area.Y + this.InsidePadding.BottomPadding; }

		/// <summary>
		/// Gets or sets the horizontal user interface size type.
		/// </summary>
		public UiElementSizeTypes HorizontalSizeType { get; set; }

		/// <summary>
		/// Gets or sets the vertical user interface size type.
		/// </summary>
		public UiElementSizeTypes VerticalSizeType { get; set; }

		/// <summary>
		/// Gets or sets the cached element offset.
		/// </summary>
		public Vector2? CachedElementOffset { get; set; }

		/// <summary>
		/// Gets or sets the clickable area scaler.
		/// </summary>
		public Vector2 ClickableAreaScaler { get; set; }

		/// <summary>
		/// Gets or sets the area.
		/// </summary>
		public Vector2 Area { get; set; }

		/// <summary>
		/// Gets or sets the inside user interface padding. 
		/// </summary>
		public UiPadding InsidePadding { get; set; }

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
				X = offset.X + (this.Area.X / 2) - (textMeasurements.X / 2) + this.InsidePadding.LeftPadding,
				Y = offset.Y + (this.Area.Y / 2) - (textMeasurements.Y / 2) + this.InsidePadding.TopPadding
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

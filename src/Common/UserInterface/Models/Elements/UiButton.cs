using Common.Controls.CursorInteraction.Models;
using Common.Controls.CursorInteraction.Models.Abstract;
using Common.Controls.CursorInteraction.Models.Contracts;
using Common.UserInterface.Enums;
using Common.UserInterface.Models.Contracts;
using Engine.Graphics.Models;
using Engine.Physics.Models;
using Engine.Physics.Models.SubAreas;
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
		/// Gets the total width.
		/// </summary>
		public float TotalWidth { get => this.OutsidePadding.LeftPadding + this.InsideWidth + this.OutsidePadding.RightPadding; }

		/// <summary>
		/// Gets the total height.
		/// </summary>
		public float TotalHeight { get => this.OutsidePadding.TopPadding + this.InsideHeight + this.OutsidePadding.BottomPadding; }

		/// <summary>
		/// Gets the inside width.
		/// </summary>
		public float InsideWidth { get => this.InsidePadding.LeftPadding + this.Area.Width + this.InsidePadding.RightPadding; }

		/// <summary>
		/// Gets the inside height.
		/// </summary>
		public float InsideHeight { get => this.InsidePadding.TopPadding + this.Area.Height + this.InsidePadding.BottomPadding; }

		/// <summary>
		/// Gets or sets the horizontal user interface size type.
		/// </summary>
		public UiElementSizeType HorizontalSizeType { get; set; }

		/// <summary>
		/// Gets or sets the vertical user interface size type.
		/// </summary>
		public UiElementSizeType VerticalSizeType { get; set; }

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
		public SubArea Area { get; set; }

		/// <summary>
		/// Gets or sets the outside user interface padding.
		/// </summary>
		public UiPadding OutsidePadding { get; set; }

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
			var graphicOffset = offset + new Vector2
			{
				X = this.OutsidePadding.LeftPadding,
				Y = this.OutsidePadding.TopPadding
			};
			this.Graphic?.Draw(gameTime, gameServices, position, graphicOffset);
			this.ClickAnimation?.Draw(gameTime, gameServices, position, graphicOffset);

			if (null != this.GraphicText)
			{
				var graphicTextOffset = graphicOffset + new Vector2
				{
					X = this.InsidePadding.LeftPadding,
					Y = this.InsidePadding.TopPadding
				};

				var textDimensions = this.GraphicText.GetTextDimensions();
				var centeredOffset = new Vector2(
					(this.Area.Width - textDimensions.X) / 2f,
					(this.Area.Height - textDimensions.Y) / 2f
				);

				this.GraphicText.Draw(gameTime, gameServices, position, graphicTextOffset + centeredOffset);
			}
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

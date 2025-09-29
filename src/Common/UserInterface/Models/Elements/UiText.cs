using Common.Controls.CursorInteraction.Models;
using Common.Controls.CursorInteraction.Models.Abstract;
using Common.UserInterface.Enums;
using Common.UserInterface.Models.Contracts;
using Engine.Graphics.Models;
using Engine.Physics.Models;
using Microsoft.Xna.Framework;

namespace Common.UserInterface.Models.Elements
{
	/// <summary>
	/// Represents a user interface text.
	/// </summary>
	public class UiText : IAmAUiElementWithText
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
		/// Draws the sub drawable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		/// <param name="position">The position.</param>
		/// <param name="offset">The offset.</param>
		public void Draw(GameTime gameTime, GameServiceContainer gameServices, Position position, Vector2 offset = default)
		{
			var textMeasurements = this.GraphicText?.GetTextDimensions();

			this.Graphic?.Draw(gameTime, gameServices, position, offset);
			this.GraphicText?.Draw(gameTime, gameServices, position, offset + (this.Area / 2) - (textMeasurements.Value / 2));
		}

		/// <summary>
		/// Disposes of the user interface button.
		/// </summary>
		public void Dispose()
		{
			this.HoverConfig?.Dispose();
			this.PressConfig?.Dispose();
		}
	}
}

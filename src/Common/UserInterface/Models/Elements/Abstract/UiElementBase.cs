using Common.Controls.CursorInteraction.Models;
using Common.Controls.CursorInteraction.Models.Abstract;
using Common.Controls.Cursors.Models;
using Common.UserInterface.Enums;
using Common.UserInterface.Models.Contracts;
using Engine.Graphics.Models.Contracts;
using Engine.Physics.Models.SubAreas;
using Microsoft.Xna.Framework;

namespace Common.UserInterface.Models.Elements.Abstract
{
	/// <summary>
	/// Represents a user interface element base class.
	/// </summary>
	abstract public class UiElementBase : IAmAUiElement
	{
		/// <summary>
		/// Gets or sets the cached offset.
		/// </summary>
		public Vector2? CachedOffset { get; set; }

		/// <summary>
		/// Gets or sets the user interface element name.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets the total width.
		/// </summary>
		public float TotalWidth { get => Margin.LeftMargin + InsideWidth + Margin.RightMargin; }

		/// <summary>
		/// Gets the total height.
		/// </summary>
		public float TotalHeight { get => Margin.TopMargin + InsideHeight + Margin.BottomMargin; }

		/// <summary>
		/// Gets the inside width.
		/// </summary>
		public float InsideWidth { get => Area.Width; }

		/// <summary>
		/// Gets the inside height.
		/// </summary>
		public float InsideHeight { get => Area.Height; }

		/// <summary>
		/// Gets or sets the horizontal user interface size type.
		/// </summary>
		public UiElementSizeType HorizontalSizeType { get; set; }

		/// <summary>
		/// Gets or sets the vertical user interface size type.
		/// </summary>
		public UiElementSizeType VerticalSizeType { get; set; }

		/// <summary>
		/// Gets or sets the area.
		/// </summary>
		public SubArea Area { get; set; }

		/// <summary>
		/// Gets or sets the user interface margin.
		/// </summary>
		public UiMargin Margin { get; set; }

		/// <summary>
		/// Gets the SimpleText.
		/// </summary>
		public IAmAGraphic Graphic { get; set; }

		/// <summary>
		/// Gets or sets the hover cursor.
		/// </summary>
		public Cursor HoverCursor { get; set; }

		/// <summary>
		/// Gets the base cursor configuration.
		/// </summary>
		public BaseCursorConfiguration BaseCursorConfiguration { get => CursorConfiguration; }

		/// <summary>
		/// Gets or sets the cursor configuration
		/// </summary>
		public CursorConfiguration<IAmAUiElement> CursorConfiguration { get; set; }

		/// <summary>
		/// Raises the hover event.
		/// </summary>
		/// <param name="cursorInteraction">The cursor interaction.</param>
		public void RaiseHoverEvent(CursorInteraction<IAmAUiElement> cursorInteraction)
		{
			CursorConfiguration?.RaiseHoverEvent(cursorInteraction);
		}

		/// <summary>
		/// Raises the press event.
		/// </summary>
		/// <param name="cursorInteraction">The cursor interaction.</param>
		public void RaisePressEvent(CursorInteraction<IAmAUiElement> cursorInteraction)
		{
			CursorConfiguration?.RaisePressEvent(cursorInteraction);
		}

		/// <summary>
		/// Draws the sub drawable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		/// <param name="coordinates">The coordinates.</param>
		/// <param name="color">The color.</param>
		/// <param name="offset">The offset.</param>
		abstract public void Draw(GameTime gameTime, GameServiceContainer gameServices, Vector2 coordinates, Color color, Vector2 offset = default);

		/// <summary>
		/// Draws the debug drawable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		/// <param name="coordinates">The coordinates.</param>
		/// <param name="color">The color.</param>
		/// <param name="offset">The offset.</param>
		virtual public void DrawDebug(GameTime gameTime, GameServiceContainer gameServices, Vector2 coordinates, Color color, Vector2 offset = default)
		{
			var graphicOffset = offset + CachedOffset ?? default;
			Area.Draw(gameTime, gameServices, coordinates, color, graphicOffset);
		}

		/// <summary>
		/// Disposes of the user interface button.
		/// </summary>
		abstract public void Dispose();
	}
}

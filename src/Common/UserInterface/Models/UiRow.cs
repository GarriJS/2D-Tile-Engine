using Common.Controls.CursorInteraction.Models;
using Common.Controls.CursorInteraction.Models.Abstract;
using Common.Controls.CursorInteraction.Models.Contracts;
using Common.Controls.Cursors.Models;
using Common.UserInterface.Enums;
using Common.UserInterface.Models.Contracts;
using Engine.Debugging.Models.Contracts;
using Engine.Graphics.Models.Contracts;
using Engine.Physics.Models;
using Engine.Physics.Models.Contracts;
using Engine.Physics.Models.SubAreas;
using Engine.RunTime.Models.Contracts;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.UserInterface.Models
{
	/// <summary>
	/// Represents a user interface row.
	/// </summary>
	sealed public class UiRow : IAmSubDrawable, IAmDebugSubDrawable, IHaveASubArea, IHaveAHoverCursor, ICanBeHovered<UiRow>, IDisposable
	{
		/// <summary>
		/// Gets or sets the cached offset.
		/// </summary>
		public Vector2? CachedOffset { get; set; }

		/// <summary>
		/// Gets or sets the user interface row name.
		/// </summary>
		required public string Name { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the user interface row should flex.
		/// </summary>
		required public bool Flex { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether to extend the background to the margins.
		/// </summary>
		required public bool ExtendBackgroundToMargin { get; set; }

		/// <summary>
		/// Gets or sets the available width to the row.
		/// </summary>
		required public float AvailableWidth { get; set; }

		/// <summary>
		/// Gets the total width.
		/// </summary>
		public float TotalWidth { get => this.Margin.LeftMargin + this.InsideWidth + this.Margin.RightMargin; }

		/// <summary>
		/// Gets the total height.
		/// </summary>
		public float TotalHeight { get => this.Margin.TopMargin + this.InsideHeight + this.Margin.BottomMargin; }

		/// <summary>
		/// Get the inside width.
		/// </summary>
		public float InsideWidth { get => this.Area.Width; }

		/// <summary>
		/// Gets the inside height.
		/// </summary>
		public float InsideHeight { get => this.Area.Height; }

		/// <summary>
		/// Gets or sets the user interface margin
		/// </summary>
		required public UiMargin Margin { get; set; }

		/// <summary>
		/// Gets or sets the user interface horizontal justification type. 
		/// </summary>
		required public UiHorizontalJustificationType HorizontalJustificationType { get; set; }

		/// <summary>
		/// Gets or sets the user interface vertical justification type.
		/// </summary>
		required public UiVerticalJustificationType VerticalJustificationType { get; set; }

		/// <summary>
		/// Gets or sets the area.
		/// </summary>
		required public SubArea Area { get; set; }

		/// <summary>
		/// Gets the graphic.
		/// </summary>
		required public IAmAGraphic Graphic { get; set; }

		/// <summary>
		/// Gets or sets the hover cursor.
		/// </summary>
		required public Cursor HoverCursor { get; set; }

		/// <summary>
		/// Gets the base hover configuration.
		/// </summary>
		public BaseCursorConfiguration BaseCursorConfiguration { get => this.CursorConfiguration; }

		/// <summary>
		/// Gets or sets the hover configuration.
		/// </summary>
		required public CursorConfiguration<UiRow> CursorConfiguration { get; set; }

		/// <summary>
		/// The user interface elements.
		/// </summary>
		readonly public List<IAmAUiElement> _elements = [];

		/// <summary>
		/// Raises the hover event.
		/// </summary>
		/// <param name="cursorInteraction">The cursor interaction.</param>
		public void RaiseHoverEvent(CursorInteraction<UiRow> cursorInteraction)
		{
			this.CursorConfiguration?.RaiseHoverEvent(cursorInteraction);
		}

		/// <summary>
		/// Draws the sub drawable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		/// <param name="coordinates">The coordinates.</param>
		/// <param name="color">The color.</param>
		/// <param name="offset">The offset.</param>
		public void Draw(GameTime gameTime, GameServiceContainer gameServices, Vector2 coordinates, Color color, Vector2 offset = default)
		{
			var marginGraphicOffset = this.ExtendBackgroundToMargin ? 
				new Vector2
				{ 
					X = -this.Margin.LeftMargin,
					Y = -this.Margin.TopMargin
				} :
				default;
			var graphicOffset = offset + (this.CachedOffset ?? default);
			var backgroundOffset = graphicOffset + marginGraphicOffset;
			this.Graphic?.Draw(gameTime, gameServices, coordinates, color, backgroundOffset);

			foreach (var element in this._elements ?? [])
				element.Draw(gameTime, gameServices, coordinates, color, graphicOffset);
		}

		/// <summary>
		/// Updates the offsets.
		/// </summary>
		public void UpdateOffsets()
		{
			foreach (var layout in this.EnumerateLayout() ?? [])
				layout.Subject.CachedOffset = layout.Vector;
		}

		/// <summary>
		/// Enumerates the layout.
		/// </summary>
		/// <returns>The enumerated layout.</returns>
		public IEnumerable<Vector2Extender<IAmAUiElement>> EnumerateLayout()
		{
			var contentWidth = this._elements.Sum(e => e.TotalWidth);
			var horizontalOffset = this.HorizontalJustificationType switch
			{
				UiHorizontalJustificationType.Center => (this.TotalWidth - contentWidth) / 2,
				UiHorizontalJustificationType.Right => this.TotalWidth - contentWidth,
				_ => 0
			};

			if (horizontalOffset < 0)
				horizontalOffset = 0;

			foreach (var element in this._elements ?? [])
			{
				var elementLeft = horizontalOffset + element.Margin.LeftMargin;
				var elementRight = elementLeft + element.InsideWidth;
				var verticalOffset = this.VerticalJustificationType switch
				{
					UiVerticalJustificationType.Center => (element.TotalHeight - this.InsideHeight) / 2,
					UiVerticalJustificationType.Bottom => element.TotalHeight - this.InsideHeight,
					_ => 0
				};

				if (verticalOffset < 0)
					verticalOffset = 0;

				var elementTop = verticalOffset + element.Margin.TopMargin;
				var elementBottom = elementTop + element.InsideHeight;
				var result = new Vector2Extender<IAmAUiElement>
				{
					Vector = new Vector2
					{
						X = elementLeft,
						Y = elementTop
					},
					Subject = element,
				};

				yield return result;

				horizontalOffset += element.TotalWidth;
			}
		}

		/// <summary>
		/// Draws the debug drawable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		/// <param name="coordinates">The coordinates.</param>
		/// <param name="color">The color.</param>
		/// <param name="offset">The offset.</param>
		public void DrawDebug(GameTime gameTime, GameServiceContainer gameServices, Vector2 coordinates, Color color, Vector2 offset = default)
		{
			var graphicOffset = offset + (this.CachedOffset ?? default);

			foreach (var element in this._elements)
				element.DrawDebug(gameTime, gameServices, coordinates, color, graphicOffset);

			this.Area.Draw(gameTime, gameServices, coordinates, color, graphicOffset);
		}

		/// <summary>
		/// Disposes of the user interface row.
		/// </summary>
		public void Dispose()
		{
			this.CursorConfiguration.Dispose();

			foreach (var subElement in this._elements ?? Enumerable.Empty<IAmAUiElement>())
				subElement?.Dispose();
		}
	}
}

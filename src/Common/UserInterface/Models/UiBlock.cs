using Common.Controls.CursorInteraction.Models;
using Common.Controls.CursorInteraction.Models.Abstract;
using Common.Controls.CursorInteraction.Models.Contracts;
using Common.Controls.Cursors.Models;
using Common.UserInterface.Enums;
using Common.UserInterface.Models.LayoutInfo;
using Engine.Graphics.Models.Contracts;
using Engine.Physics.Models;
using Engine.Physics.Models.Contracts;
using Engine.Physics.Models.SubAreas;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.UserInterface.Models
{
	/// <summary>
	/// Represents a user interface block.
	/// </summary>
	public class UiBlock : IHaveASubArea, IHaveAHoverCursor, ICanBeHovered<UiBlock>, IDisposable
	{
		/// <summary>
		/// Gets or sets the cached offset.
		/// </summary>
		public Vector2? CachedOffset { get; set; }

		/// <summary>
		/// Gets or sets the user interface block name.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the user interface block should flex the rows by vertically stacking them.
		/// </summary>
		public bool FlexRows { get; set; }

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
		public UiMargin Margin { get; set; }

		/// <summary>
		/// Gets or sets the user interface horizontal justification type. 
		/// </summary>
		public UiHorizontalJustificationType HorizontalJustificationType { get; set; }

		/// <summary>
		/// Gets or sets the user interface vertical justification type.
		/// </summary>
		public UiVerticalJustificationType VerticalJustificationType { get; set; }

		/// <summary>
		/// Gets or sets the area.
		/// </summary>
		public SubArea Area { get; set; }

		/// <summary>
		/// Gets the graphic.
		/// </summary>
		public IAmAGraphic Graphic { get; set; }

		/// <summary>
		/// Gets or sets the hover cursor.
		/// </summary>
		public Cursor HoverCursor { get; set; }

		/// <summary>
		/// Gets the base hover configuration.
		/// </summary>
		public BaseCursorConfiguration BaseCursorConfiguration { get => this.CursorConfiguration; }

		/// <summary>
		/// Gets or sets the hover configuration.
		/// </summary>
		public CursorConfiguration<UiBlock> CursorConfiguration { get; set; }

		/// <summary>
		/// Gets or sets the rows.
		/// </summary>
		public List<UiRow> Rows { get; set; }

		/// <summary>
		/// Raises the hover event.
		/// </summary>
		/// <param name="cursorInteraction">The cursor interaction.</param>
		public void RaiseHoverEvent(CursorInteraction<UiBlock> cursorInteraction)
		{
			this.CursorConfiguration?.RaiseHoverEvent(cursorInteraction);
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
			var graphicOffset = offset + (this.CachedOffset ?? default);
			this.Graphic?.Draw(gameTime, gameServices, position, graphicOffset);

			foreach (var elementRow in this.Rows ?? [])
			{
				elementRow.Draw(gameTime, gameServices, position, graphicOffset);
			}
		}

		/// <summary>
		/// Updates the offsets.
		/// </summary>
		public void UpdateOffsets()
		{
			foreach (var rowLayout in this.EnumerateLayout() ?? [])
			{
				rowLayout.Row.CachedOffset = rowLayout.Offset;
				rowLayout.Row.UpdateOffsets();
			}
		}

		/// <summary>
		/// Enumerates the rowLayout.
		/// </summary>
		/// <returns>The enumerated rowLayout.</returns>
		public IEnumerable<RowLayoutInfo> EnumerateLayout()
		{
			var contentWidth = this.Rows.Sum(e => e.TotalWidth);
			var contentHeight = this.Rows.Sum(e => e.TotalHeight);
			var horizontalOffset = this.HorizontalJustificationType switch
			{
				UiHorizontalJustificationType.Center => (this.TotalWidth - contentWidth) / 2,
				UiHorizontalJustificationType.Right => this.TotalWidth - contentWidth,
				_ => 0
			};
			var verticalOffset = this.VerticalJustificationType switch
			{
				UiVerticalJustificationType.Center => (this.Area.Height - contentHeight) / 2,
				UiVerticalJustificationType.Bottom => this.Area.Height - contentHeight,
				_ => 0
			};

			if (horizontalOffset < 0)
				horizontalOffset = 0;

			if (verticalOffset < 0)
				verticalOffset = 0;

			foreach (var row in this.Rows ?? [])
			{
				var rowTop = verticalOffset + row.Margin.TopMargin;
				var rowLeft = horizontalOffset + row.Margin.LeftMargin;
				var result = new RowLayoutInfo
				{
					Row = row,
					Offset = new Vector2
					{
						X = rowLeft,
						Y = rowTop
					}
				};

				yield return result;

				verticalOffset += row.TotalHeight;
			}
		}

		/// <summary>
		/// Disposes of the user interface block.
		/// </summary>
		public void Dispose()
		{
			foreach (var row in this.Rows ?? [])
			{
				row?.Dispose();
			}
		}
	}
}

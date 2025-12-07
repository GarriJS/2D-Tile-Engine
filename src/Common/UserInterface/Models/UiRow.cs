using Common.Controls.CursorInteraction.Models;
using Common.Controls.CursorInteraction.Models.Abstract;
using Common.Controls.CursorInteraction.Models.Contracts;
using Common.Controls.Cursors.Models;
using Common.UserInterface.Enums;
using Common.UserInterface.Models.Contracts;
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
	public class UiRow : IAmSubDrawable, IHaveASubArea, IHaveAHoverCursor, ICanBeHovered<UiRow>, IDisposable
	{
		/// <summary>
		/// Gets or sets the cached element graphicOffset.
		/// </summary>
		public Vector2? CachedRowOffset { get; set; }

		/// <summary>
		/// Gets or sets the user interface row name.
		/// </summary>
		public string UiRowName { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the user interface row should flex.
		/// </summary>
		public bool Flex { get; set; }

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
		/// Gets or sets the user interface row horizontal justification type. 
		/// </summary>
		public UiRowHorizontalJustificationType HorizontalJustificationType { get; set; }

		/// <summary>
		/// Gets or sets the user interface row vertical justification type.
		/// </summary>
		public UiRowVerticalJustificationType VerticalJustificationType { get; set; }

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
		public CursorConfiguration<UiRow> CursorConfiguration { get; set; }

		/// <summary>
		/// Gets or sets the sub elements.
		/// </summary>
		public List<IAmAUiElement> SubElements { get; set; }

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
		/// <param name="position">The position.</param>
		/// <param name="offset">The offset.</param>
		public void Draw(GameTime gameTime, GameServiceContainer gameServices, Position position, Vector2 offset = default)
		{
			var graphicOffset = offset + (this.CachedRowOffset ?? default);
			this.Graphic?.Draw(gameTime, gameServices, position, graphicOffset);
			var contentOffset = graphicOffset + new Vector2
			{
				X = this.Margin.LeftMargin,
				Y = this.Margin.TopMargin,
			};

			foreach (var element in this.SubElements ?? [])
			{
				element.Draw(gameTime, gameServices, position, contentOffset + (element.CachedElementOffset ?? default));
			}
		}

		/// <summary>
		/// Updates the rows offsets.
		/// </summary>
		public void UpdateRowOffsets()
		{
			var contentWidth = this.SubElements.Sum(e => e.TotalWidth);
			var rowHorizontalJustificationOffset = this.HorizontalJustificationType switch
			{
				UiRowHorizontalJustificationType.Center => (this.Area.Width - contentWidth) / 2,
				UiRowHorizontalJustificationType.Right => this.Area.Width - contentWidth,
				_ => 0
			};

			if (0 > rowHorizontalJustificationOffset)
			{
				rowHorizontalJustificationOffset = default;
			}

			var elementOffset = new Vector2
			{
				X = rowHorizontalJustificationOffset,
				Y = 0
			};

			foreach (var element in this.SubElements ?? [])
			{
				var rowVerticalJustificationOffset = this.VerticalJustificationType switch
				{
					UiRowVerticalJustificationType.Center => (this.Area.Height - element.TotalHeight) / 2,
					UiRowVerticalJustificationType.Bottom => this.Area.Height - element.TotalHeight,
					_ => 0
				};

				if (0 > rowVerticalJustificationOffset)
				{
					rowVerticalJustificationOffset = 0;
				}

				elementOffset.Y = rowVerticalJustificationOffset;
				element.CachedElementOffset = elementOffset;
				elementOffset.X += element.TotalWidth;
			}
		}

		/// <summary>
		/// Disposes of the user interface row.
		/// </summary>
		public void Dispose()
		{
			foreach (var subElement in this.SubElements ?? Enumerable.Empty<IAmAUiElement>())
			{
				subElement?.Dispose();
			}
		}
	}
}

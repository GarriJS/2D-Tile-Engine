using Common.Controls.CursorInteraction.Models;
using Common.Controls.CursorInteraction.Models.Abstract;
using Common.Controls.CursorInteraction.Models.Contracts;
using Common.Controls.Cursors.Models;
using Common.UserInterface.Enums;
using Common.UserInterface.Models.Contracts;
using Common.UserInterface.Models.LayoutInfo;
using Engine.Debugging.Models.Contracts;
using Engine.Graphics.Models.Contracts;
using Engine.Physics.Models.Contracts;
using Engine.Physics.Models.SubAreas;
using Engine.RunTime.Models.Contracts;
using Engine.RunTime.Services.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.UserInterface.Models
{
	/// <summary>
	/// Represents a user interface block.
	/// </summary>
	public class UiBlock : IAmSubDrawable, IAmSubPreRenderable, IAmDebugSubDrawable, IAmScrollable, IHaveASubArea, IHaveAHoverCursor, ICanBeHovered<UiBlock>, IDisposable
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
		/// Gets or sets a value indicating whether to extend the background to the margins.
		/// </summary>
		public bool ExtendBackgroundToMargin { get; set; }

		/// <summary>
		/// Gets or sets the available width to the row.
		/// </summary>
		public float AvailableWidth { get; set; }

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
		/// Gets the scroll state.
		/// </summary>
		public ScrollState ScrollState { get; set; }

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
		/// <param name="coordinates">The coordinates.</param>
		/// <param name="color">The color.</param>
		/// <param name="offset">The offset.</param>
		public void Draw(GameTime gameTime, GameServiceContainer gameServices, Vector2 coordinates, Color color, Vector2 offset = default)
		{
			var drawingService = gameServices.GetService<IDrawingService>();
			var graphicOffset = offset + (this.CachedOffset ?? default);

			if (null != this.ScrollState?.ScrollRenderTarget)
			{
				var sourceRectangle = this.ScrollState.GetSourceRectanlge();
				drawingService.Draw(this.ScrollState.ScrollRenderTarget, coordinates + graphicOffset, sourceRectangle, Color.White);
			}
			else
				this.DrawContents(gameTime, gameServices, coordinates, Color.White, graphicOffset);
		}

		/// <summary>
		/// Draws the contents.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		/// <param name="coordinates">The coordinates.</param>
		/// <param name="color">The color.</param>
		/// <param name="offset">The offset.</param>
		private void DrawContents(GameTime gameTime, GameServiceContainer gameServices, Vector2 coordinates, Color color, Vector2 offset = default)
		{
			var marginGraphicOffset = this.ExtendBackgroundToMargin ?
				new Vector2
				{
					X = -this.Margin.LeftMargin,
					Y = -this.Margin.TopMargin
				} :
				default;
			var backgroundOffset = offset + marginGraphicOffset;
			this.Graphic?.Draw(gameTime, gameServices, coordinates, color, backgroundOffset);

			foreach (var elementRow in this.Rows ?? [])
				elementRow.Draw(gameTime, gameServices, coordinates, color, offset);
		}

		/// <summary>
		/// Assess if prerendering is needed.
		/// </summary>
		/// <returns>A value indicating whether prerendering is needed.</returns>
		public bool ShouldPreRender()
		{
			if (null == this.ScrollState)
				return false;

			var contentHeight = this.Rows.Sum(e => e.TotalHeight);
			var result = contentHeight > this.ScrollState.MaxVisibleHeight;

			return result;
		}

		/// <summary>
		/// Does the prerender.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game service.</param>
		public void PreRender(GameTime gameTime, GameServiceContainer gameServices, Vector2 coordinates, Color color, Vector2 offset = default)
		{
			if ((null == this.ScrollState) ||
				(true == this.ScrollState.DisableScrolling))
				return;

			var graphicDeviceService = gameServices.GetService<IGraphicsDeviceService>();
			var device = graphicDeviceService.GraphicsDevice;
			var drawingService = gameServices.GetService<IDrawingService>();
			var contentHeight = this.Rows.Sum(e => e.TotalHeight);

			if ((null == this.ScrollState.ScrollRenderTarget) ||
				(this.ScrollState.ScrollRenderTarget.Width != this.Area.Width) ||
				(this.ScrollState.ScrollRenderTarget.Height != contentHeight))
			{
				this.ScrollState.ScrollRenderTarget?.Dispose();
				this.ScrollState.ScrollRenderTarget = new RenderTarget2D(device, (int)this.Area.Width, (int)contentHeight);
			}

			var previousTargets = device.GetRenderTargets();
			device.SetRenderTarget(this.ScrollState.ScrollRenderTarget);
			device.Clear(Color.Transparent);
			drawingService.BeginDraw();

			this.DrawContents(gameTime, gameServices, default, color, offset);

			drawingService.EndDraw();
			device.SetRenderTargets(previousTargets);
		}

		/// <summary>
		/// Updates the offsets.
		/// </summary>
		public void UpdateOffsets()
		{
			foreach (var rowLayout in this.EnumerateLayout(includeScrollOffset: false) ?? [])
			{
				rowLayout.Row.CachedOffset = rowLayout.Offset;
				rowLayout.Row.UpdateOffsets();
			}
		}

		/// <summary>
		/// Enumerates the rowLayout.
		/// </summary>
		/// <param name="includeScrollOffset">A value indicating whether to include the scroll offset.</param>
		/// <returns>The enumerated rowLayout.</returns>
		public IEnumerable<RowLayoutInfo> EnumerateLayout(bool includeScrollOffset)
		{
			var contentHeight = this.Rows.Sum(e => e.TotalHeight);
			var verticalOffset = this.VerticalJustificationType switch
			{
				UiVerticalJustificationType.Center => (this.Area.Height - contentHeight) / 2,
				UiVerticalJustificationType.Bottom => this.Area.Height - contentHeight,
				_ => 0
			};

			if (verticalOffset < 0)
				verticalOffset = 0;

			if ((true == includeScrollOffset) &&
				(null != this.ScrollState))
				verticalOffset -= this.ScrollState.VerticalScrollOffset;

			foreach (var row in this.Rows ?? [])
			{
				var horizontalOffset = row.HorizontalJustificationType switch
				{
					UiHorizontalJustificationType.Center => (row.AvailableWidth - row.TotalWidth) / 2,
					UiHorizontalJustificationType.Right => row.AvailableWidth - row.TotalWidth,
					_ => 0
				};
				if (horizontalOffset < 0)
					horizontalOffset = 0;

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
		/// Draws the debug drawable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		/// <param name="coordinates">The coordinates.</param>
		/// <param name="color">The color.</param>
		/// <param name="offset">The offset.</param>
		public void DrawDebug(GameTime gameTime, GameServiceContainer gameServices, Vector2 coordinates, Color color, Vector2 offset = default)
		{
			var scrollOffset = new Vector2
			{
				X = 0,
				Y = -this.ScrollState?.VerticalScrollOffset ?? default
			};
			var graphicOffset = offset + scrollOffset + (this.CachedOffset ?? default);

			foreach (var uiRow in this.Rows)
				uiRow.DrawDebug(gameTime, gameServices, coordinates, color, graphicOffset);

			this.Area.Draw(gameTime, gameServices, coordinates, color, graphicOffset);
		}

		/// <summary>
		/// Disposes of the user interface block.
		/// </summary>
		public void Dispose()
		{
			foreach (var row in this.Rows ?? [])
				row?.Dispose();
		}
	}
}

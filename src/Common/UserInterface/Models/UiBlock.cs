using Common.Controls.CursorInteraction.Models;
using Common.Controls.CursorInteraction.Models.Abstract;
using Common.Controls.CursorInteraction.Models.Contracts;
using Common.Controls.CursorInteraction.Services.Contracts;
using Common.Controls.Cursors.Models;
using Common.UserInterface.Enums;
using Common.UserInterface.Models.Contracts;
using Common.UserInterface.Services.Contracts;
using Engine.Debugging.Models.Contracts;
using Engine.Graphics.Models;
using Engine.Graphics.Models.Contracts;
using Engine.Physics.Models;
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
	sealed public class UiBlock : IAmSubDrawable, IAmSubPreRenderable, IAmDebugSubDrawable, IAmScrollable, IHaveAHoverCursor, ICanBeHovered<UiBlock>, IDisposable
	{
		/// <summary>
		/// Gets or sets the user interface layout cache.
		/// </summary>
		public UiLayoutCache UiLayoutCache { get; set; }

		/// <summary>
		/// Gets or sets the user interface block name.
		/// </summary>
		required public string Name { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the user interface block should flex the rows by vertically stacking them.
		/// </summary>
		required public bool FlexRows { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to resize the texture.
        /// </summary>
        required public bool ResizeTexture { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to extend the background to the margins.
        /// </summary>
        required public bool ExtendBackgroundToMargin { get; set; }

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
		public float InsideWidth { get => this.UiLayoutCache?.InsideArea.Width ?? 0; }

		/// <summary>
		/// Gets the inside height.
		/// </summary>
		public float InsideHeight { get => this.UiLayoutCache?.InsideArea.Height ?? 0; }

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
		/// Gets the graphic.
		/// </summary>
		required public IAmAGraphic Graphic { get; set; }

		/// <summary>
		/// Gets the scroll state.
		/// </summary>
		required public ScrollState ScrollState { get; set; }

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
		public CursorConfiguration<UiBlock> CursorConfiguration { get; set; }

		/// <summary>
		/// The rows.
		/// </summary>
		readonly public List<UiRow> _rows = [];

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
			var graphicOffset = offset + (this.UiLayoutCache?.Offset ?? default);

			if (this.ScrollState?.ScrollRenderTarget is not null)
			{
				var sourceRectangle = this.ScrollState.GetSourceRectanlge();
				drawingService.Draw(this.ScrollState.ScrollRenderTarget, coordinates + graphicOffset, sourceRectangle, Color.White);
				this.ScrollState.Draw(gameTime, gameServices, coordinates, color, graphicOffset);
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

			foreach (var elementRow in this._rows ?? [])
				elementRow.Draw(gameTime, gameServices, coordinates, color, offset);
		}

		/// <summary>
		/// Assess if prerendering is needed.
		/// </summary>
		/// <returns>A value indicating whether prerendering is needed.</returns>
		public bool ShouldPreRender()
		{
			if (this.ScrollState is null)
				return false;

			var contentHeight = this._rows.Sum(e => e.TotalHeight);
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
			if ((null == this.UiLayoutCache) ||
				(this.ScrollState is null) ||
				(true == this.ScrollState.DisableScrolling))
				return;

			var graphicDeviceService = gameServices.GetService<IGraphicsDeviceService>();
			var device = graphicDeviceService.GraphicsDevice;
			var drawingService = gameServices.GetService<IDrawingService>();
			var contentHeight = this._rows.Sum(e => e.TotalHeight);

			if ((this.ScrollState.ScrollRenderTarget is null) ||
				(this.ScrollState.ScrollRenderTarget.Width != this.UiLayoutCache.TotalArea.Width) ||
				(this.ScrollState.ScrollRenderTarget.Height != contentHeight))
			{
				this.ScrollState.ScrollRenderTarget?.Dispose();
				this.ScrollState.ScrollRenderTarget = new RenderTarget2D(device, (int)this.UiLayoutCache.TotalArea.Width, (int)contentHeight);
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
        /// Refreshes the layout cache.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        /// <param name="gameServices">The game services.</param>
        /// <param name="availableWidth">The available width.</param>
        /// <param name="availabelHeight">The available height.</param>
        public void RefreshLayoutCache(GameTime gameTime, GameServiceContainer gameServices, float availableWidth, float availabelHeight)
        {
            var fixedSizedWidth = this._rows.Where(e => e.HasWidthFlexElements).Sum(e => e.TotalWidth);
            var fixedSizedHeight = this._rows.Where(e => e.HasHeightFlexElements).Sum(e => e.TotalHeight);
            this.UiLayoutCache = new UiLayoutCache
            {
                FixedSizedWidth = fixedSizedWidth,
                FixedSizedHeight = fixedSizedHeight,
                InsideArea = new SubArea
                {
                    Width = 0,
                    Height = 0
                },
                TotalArea = new SubArea
                {
                    Width = 0 + this.Margin.LeftMargin + this.Margin.RightMargin,
                    Height = 0 + this.Margin.TopMargin + this.Margin.BottomMargin
                }
            };
            this.UpdateRowSizes(gameTime, gameServices, availableWidth, availabelHeight);
        }

        /// <summary>
        /// Updates the row sizes.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        /// <param name="gameServices">The game services.</param>
        /// <param name="availableWidth">The available width.</param>
        /// <param name="availabelHeight">The available height.</param>
        private void UpdateRowSizes(GameTime gameTime, GameServiceContainer gameServices, float availableWidth, float availabelHeight)
		{
			this._rows.RemoveAll(e => 0 == e._elements.Count);

            if ((null == this.UiLayoutCache) ||
				(0 == this._rows.Count))
                return;

            var insideWidth = availableWidth - (this.Margin.LeftMargin + this.Margin.RightMargin);
            availabelHeight -= this.Margin.TopMargin + this.Margin.BottomMargin;
            var uiRowService = gameServices.GetService<IUiRowService>();
            var flexWidthRows = this._rows.Where(e => e.HasWidthFlexElements || 
													  (UiHorizontalJustificationType.SpaceBetween == e.HorizontalJustificationType))
										  .ToList();
			var flexHeightRows = this._rows.Where(e => e.HasHeightFlexElements ||
													   (UiVerticalJustificationType.SpaceBetween == e.VerticalJustificationType))
										   .ToList();
			var flexWidth = 0f;
			var flexHeight = 0f;

            if (0 != flexWidthRows.Count)
				flexWidth = insideWidth;

            if (0 != flexHeightRows.Count)
                flexHeight = availabelHeight - this.UiLayoutCache.FixedSizedHeight / flexHeightRows.Count;

            foreach (var row in this._rows)
                row.RefreshLayoutCache(gameTime, gameServices, flexWidth, flexHeight);

			if (true == this.FlexRows)
			{
				var overflowingRows = this._rows.Where(e => e.TotalWidth > insideWidth)
												.ToArray();

				if (0 != overflowingRows.Length)
				{
					float? targetWidth = null;

					foreach (var row in overflowingRows)
					{
						// this is really split even and not center
						if (UiHorizontalJustificationType.Center == this.HorizontalJustificationType)
							targetWidth = insideWidth / (float)Math.Ceiling(row.TotalWidth / insideWidth);

						var newRows = uiRowService.SplitRow(row, insideWidth);
						
						foreach (var newRow in newRows)
							newRow.RefreshLayoutCache(gameTime, gameServices, flexWidth, flexHeight);

						var originalRowIndex = this._rows.IndexOf(row);
						this._rows.InsertRange(originalRowIndex, newRows);
						this._rows.Remove(row);
                    }
				}
			}

			if (0 == insideWidth)
				insideWidth = this._rows.Max(e => e.TotalWidth);

			var contentHight = this._rows.Sum(e => e.TotalHeight);
            this.UpdateBlockArea(gameServices, insideWidth, contentHight);
        }

        /// <summary>
        /// Updates the block area.
        /// </summary>
        /// <param name="gameServices">The game service.</param>
        /// <param name="insideWidth">The inside width.</param>
        /// <param name="insideHeight">The inside height.</param>
        private void UpdateBlockArea(GameServiceContainer gameServices, float insideWidth, float insideHeight)
        {
            this.UiLayoutCache.InsideArea = new SubArea
            {
                Width = (int)insideWidth,
                Height = (int)insideHeight
            };
            this.UiLayoutCache.TotalArea = new SubArea
            {
                Width = (int)insideWidth + this.Margin.LeftMargin + this.Margin.RightMargin,
                Height = (int)insideHeight + this.Margin.TopMargin + this.Margin.BottomMargin
            };

			if (null == this.CursorConfiguration)
            {
                var cursorInteractionService = gameServices.GetService<ICursorInteractionService>();
                this.CursorConfiguration = cursorInteractionService.GetCursorConfiguration<UiBlock>(this.UiLayoutCache.TotalArea);
			}
			else
				this.CursorConfiguration.Area = this.UiLayoutCache.TotalArea;

            if ((null != this.Graphic) &&
                ((true == this.ResizeTexture) ||
                 (this.Graphic is CompositeImage)))
                if (this.ExtendBackgroundToMargin)
                    this.Graphic.SetDrawDimensions(this.UiLayoutCache.TotalArea);
                else
                    this.Graphic.SetDrawDimensions(this.UiLayoutCache.InsideArea);
        }

        /// <summary>
        /// Updates the offsets.
        /// </summary>
        public void UpdateOffsets()
		{
			foreach (var rowLayout in this.EnumerateRowPosition(includeScrollOffset: false) ?? [])
			{
				rowLayout.Subject.UiLayoutCache.Offset = rowLayout.Vector2;
				rowLayout.Subject.UpdateElementOffsets();
			}

			var contentWidth = this._rows.Max(e => e.TotalWidth);
			var smallestRowOffSet = this._rows.Min(e => e.UiLayoutCache.Offset.X);
			this.ScrollState?.UpdateOffset(this.TotalWidth, contentWidth, smallestRowOffSet);
		}

		/// <summary>
		/// Enumerates the row positions.
		/// </summary>
		/// <param name="includeScrollOffset">A value indicating whether to include the scroll offset.</param>
		/// <returns>The enumerated row positions.</returns>
		public IEnumerable<Vector2Extender<UiRow>> EnumerateRowPosition(bool includeScrollOffset)
		{
			var contentWidth = this._rows.Sum(e => e.TotalWidth);
			var contentHeight = this._rows.Sum(e => e.TotalHeight);
			var verticalOffset = this.VerticalJustificationType switch
			{
				UiVerticalJustificationType.Center => (this.InsideHeight - contentHeight) / 2,
				UiVerticalJustificationType.Bottom => this.InsideHeight - contentHeight,
				_ => 0
			};

			if (verticalOffset < 0)
				verticalOffset = 0;

			if ((true == includeScrollOffset) &&
				(this.ScrollState is not null))
				verticalOffset -= this.ScrollState.VerticalScrollOffset;

            foreach (var row in this._rows)
			{
				var horizontalOffset = row.HorizontalJustificationType switch
				{
					UiHorizontalJustificationType.Center => (this.InsideWidth - row.TotalWidth) / 2,
					UiHorizontalJustificationType.Right => this.InsideWidth - row.TotalWidth,
                    _ => 0
				};

				if (horizontalOffset < 0)
					horizontalOffset = 0;

				var rowTop = verticalOffset + row.Margin.TopMargin;
				var rowLeft = horizontalOffset + row.Margin.LeftMargin;
				var result = new Vector2Extender<UiRow>
				{
					Vector2 = new Vector2
					{
						X = rowLeft,
						Y = rowTop
					},
					Subject = row,
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
			var graphicOffset = offset + scrollOffset + (this.UiLayoutCache?.Offset ?? default);

			foreach (var uiRow in this._rows)
				uiRow.DrawDebug(gameTime, gameServices, coordinates, color, graphicOffset);

            this.UiLayoutCache?.TotalArea.Draw(gameTime, gameServices, coordinates, color, graphicOffset);
		}

		/// <summary>
		/// Disposes of the user interface block.
		/// </summary>
		public void Dispose()
		{
			this.ScrollState?.Dispose();
			this.CursorConfiguration.Dispose();

			foreach (var row in this._rows ?? [])
				row?.Dispose();
		}
	}
}

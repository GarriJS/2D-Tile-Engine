using Common.Controls.CursorInteraction.Models;
using Common.Controls.CursorInteraction.Models.Abstract;
using Common.Controls.CursorInteraction.Models.Contracts;
using Common.Controls.CursorInteraction.Services.Contracts;
using Common.Controls.Cursors.Models;
using Common.UserInterface.Enums;
using Common.UserInterface.Models.Contracts;
using Engine.Debugging.Models.Contracts;
using Engine.Graphics.Models;
using Engine.Graphics.Models.Contracts;
using Engine.Physics.Models;
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
    /// Represents a user interface modal
    /// </summary>
    public class UiModal : IAmDrawable, IAmPreRenderable, IAmDebugDrawable, IAmScrollable, IAmAUiParent, IHaveAHoverCursor, ICanBeHovered<UiModal>, IDisposable
    {
        /// <summary>
        /// Gets or sets a value indicating if the user interface modal will recalculate the cached offsets on the next draw.
        /// </summary>
        required public bool ResetCalculateCachedOffsets { get; set; }

        /// <summary>
        /// Gets or sets the user interface modal name.
        /// </summary>
        required public string Name { get; set; }

        /// <summary>
        /// Gets or sets the draw layer.
        /// </summary>
        required public int DrawLayer { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to resize the texture.
        /// </summary>
        required public bool ResizeTexture { get; set; }

        /// <summary>
        /// Gets or sets the user interface modal vertical justification type. 
        /// </summary>
        required public UiVerticalJustificationType VerticalJustificationType { get; set; }

        /// <summary>
        /// Gets or sets the horizontal modal size type.
        /// </summary>
        required public UiModalSizeType HorizontalModalSizeType { get; set; }

        /// <summary>
        /// Gets or sets the vertical modal size type.
        /// </summary>
        required public UiModalSizeType VerticalModalSizeType { get; set; }

        /// <summary>
        /// Gets the SimpleText.
        /// </summary>
        required public IAmAGraphic Graphic { get; set; }

        /// <summary>
        /// Gets the position.
        /// </summary>
        public Position Position { get => this.Area?.Position; }

        /// <summary>
        /// Gets or sets the area.
        /// </summary>
        required public IAmAArea Area { get; set; }

        /// <summary>
        /// Gets the scroll state.
        /// </summary>
        required public ScrollState ScrollState { get; set; }

        /// <summary>
        /// Gets or sets the hover cursor.
        /// </summary>
        required public Cursor HoverCursor { get; set; }

        /// <summary>
        /// Gets the base cursor configuration.
        /// </summary>
        public BaseCursorConfiguration BaseCursorConfiguration { get => this.CursorConfiguration; }

        /// <summary>
        /// Gets or sets the cursor configuration
        /// </summary>
        required public CursorConfiguration<UiModal> CursorConfiguration { get; set; }

        /// <summary>
        /// The user interface blocks.
        /// </summary>
        readonly private List<UiBlock> _blocks = [];

        /// <summary>
        /// The user interface blocks.
        /// </summary>
        public List<UiBlock> Blocks { get => this._blocks; }

        /// <summary>
        /// Raises the hover event.
        /// </summary>
        /// <param name="cursorInteraction">The cursor interaction.</param>
        public void RaiseHoverEvent(CursorInteraction<UiModal> cursorInteraction)
        {
            this.CursorConfiguration?.RaiseHoverEvent(cursorInteraction);
        }

        /// <summary>
        /// Draws the drawable.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        /// <param name="gameServices">The game services.</param>
        public void Draw(GameTime gameTime, GameServiceContainer gameServices)
        {
            var drawingService = gameServices.GetService<IDrawingService>();

            if (this.ScrollState?.ScrollRenderTarget is not null)
            {
                var sourceRectangle = this.ScrollState.GetSourceRectanlge();
                drawingService.Draw(this.ScrollState.ScrollRenderTarget, this.Position.Coordinates, sourceRectangle, Color.White);
                this.ScrollState.Draw(gameTime, gameServices, this.Position.Coordinates, Color.White);
            }
            else
                this.DrawContents(gameTime, gameServices, this.Position.Coordinates, Color.White);
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
            this.Graphic?.Draw(gameTime, gameServices, coordinates, color, offset);

            if (true == this.ResetCalculateCachedOffsets)
            {
                this.RefreshLayoutCache(gameTime, gameServices);
                this.UpdateModalOffsets();
            }

            foreach (var block in this._blocks ?? [])
                block.Draw(gameTime, gameServices, coordinates, color, offset);
        }

        /// <summary>
        /// Assess if prerendering is needed.
        /// </summary>
        /// <returns>A value indicating whether prerendering is needed.</returns>
        public bool ShouldPreRender()
        {
            var needsSubRender = this._blocks.Any(e => true == e.ShouldPreRender());

            if (true == needsSubRender)
                return true;

            if (this.ScrollState is null)
                return false;

            var contentHeight = this._blocks.Sum(e => e.TotalHeight);
            var hasExessHeight = contentHeight > this.ScrollState.MaxVisibleHeight;

            return hasExessHeight;
        }

        /// <summary>
        /// Does the prerender.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        /// <param name="gameServices">The game service.</param>
        public void PreRender(GameTime gameTime, GameServiceContainer gameServices)
        {
            var subPrerenders = this._blocks.Where(e => true == e.ShouldPreRender())
                                            .ToArray();

            foreach (var subPrerender in subPrerenders ?? [])
                subPrerender.PreRender(gameTime, gameServices, default, Color.White);

            if ((this.ScrollState is null) ||
                (true == this.ScrollState.DisableScrolling))
                return;

            var graphicDeviceService = gameServices.GetService<IGraphicsDeviceService>();
            var device = graphicDeviceService.GraphicsDevice;
            var drawingService = gameServices.GetService<IDrawingService>();
            var contentHeight = this._blocks.Sum(e => e.TotalHeight);

            if ((this.ScrollState.ScrollRenderTarget is null) ||
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

            this.DrawContents(gameTime, gameServices, default, Color.White);

            drawingService.EndDraw();
            device.SetRenderTargets(previousTargets);
        }

        /// <summary>
        /// Refreshes the layout caches.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        /// <param name="gameServices">The game services.</param>
        public void RefreshLayoutCache(GameTime gameTime, GameServiceContainer gameServices)
        {
            this._blocks.RemoveAll(e => 0 == e._rows.Count);

            foreach (var block in this._blocks)
                block.RefreshLayoutCache(gameTime, gameServices, this.Area.Width, this.Area.Height);

            var updateWidth = UiModalSizeType.FitContent == this.HorizontalModalSizeType;
            var updateHeight = UiModalSizeType.FitContent == this.VerticalModalSizeType;

            if ((true == updateWidth) ||
                (true == updateHeight))
            {
                this.FitModalContent(gameServices, updateWidth, updateHeight);
                    
                foreach (var block in this._blocks)
                    block.RefreshLayoutCache(gameTime, gameServices, this.Area.Width, this.Area.Height);
            }
        }

        /// <summary>
        /// Updates the modal area to fit its content.
        /// </summary>
        /// <param name="gameServices">The game service.</param>
        /// <param name="updateWidth">A value indicating whether to update the width.</param>
        /// <param name="updateHeight">A value indicating whether to update the height.</param>
        private void FitModalContent(GameServiceContainer gameServices, bool updateWidth = true, bool updateHeight = true)
        {
            var contentWidth = this._blocks.Max(r => r.TotalWidth);
            var contentHeight = this._blocks.Sum(r => r.TotalHeight);
            this.Area = new SimpleArea
            {
                Position = this.Area.Position,
                Width = updateWidth ? contentWidth : this.Area.Width,
                Height = updateHeight ? contentHeight : this.Area.Height,
            };

            if (null == this.CursorConfiguration)
            {
                var cursorInteractionService = gameServices.GetService<ICursorInteractionService>();
                this.CursorConfiguration = cursorInteractionService.GetCursorConfiguration<UiModal>(this.Area.ToSubArea);
            }
            else
                this.CursorConfiguration.Area = this.Area.ToSubArea;

            if ((null != this.Graphic) &&
                ((true == this.ResizeTexture) ||
                 (this.Graphic is CompositeImage)))
                    this.Graphic.SetDrawDimensions(this.Area.ToSubArea);
        }

        /// <summary>
        /// Updates the modal offsets.
        /// </summary>
        public void UpdateModalOffsets()
        {
            foreach (var blockLayout in this.EnumerateBlockPositions(includeScrollOffset: false) ?? [])
            {
                blockLayout.Subject.UiLayoutCache.Offset = blockLayout.Vector2;
                blockLayout.Subject.UpdateOffsets();
            }

            this.ScrollState?.UpdateOffset(this.Area.Width, this.Area.Width, 0);
            this.ResetCalculateCachedOffsets = false;
        }

        /// <summary>
        /// Enumerates the Block positions.
        /// </summary>
        /// <param name="includeScrollOffset">A value indicating whether to include the scroll offset.</param>
        /// <returns>The enumerated Block positions.</returns>
        public IEnumerable<Vector2Extender<UiBlock>> EnumerateBlockPositions(bool includeScrollOffset)
        {
            var contentHeight = this._blocks.Sum(r => r.TotalHeight);
            var verticalOffset = this.VerticalJustificationType switch
            {
                UiVerticalJustificationType.Center => (this.Area.Height - contentHeight) / 2,
                UiVerticalJustificationType.Bottom => this.Area.Height - contentHeight,
                _ => 0
            };

            if (verticalOffset < 0)
                verticalOffset = 0;

            if ((true == includeScrollOffset) &&
                (this.ScrollState is not null))
                verticalOffset -= this.ScrollState.VerticalScrollOffset;

            var spaceBetweenBlocks = 0f;
            var carryOverVerticalOffset = 0f;

            if (UiVerticalJustificationType.SpaceBetween == this.VerticalJustificationType)
                if (1 < this._blocks.Count)
                    spaceBetweenBlocks = (this.Area.Height - contentHeight) / (this._blocks.Count - 1);
                else
                    carryOverVerticalOffset = (this.Area.Height - contentHeight) / 2;

            foreach (var block in this._blocks)
            {
                var horizontalOffset = block.HorizontalJustificationType switch
                {
                    UiHorizontalJustificationType.Center => (this.Area.Width - block.TotalWidth) / 2,
                    UiHorizontalJustificationType.Right => this.Area.Width - block.TotalWidth,
                    _ => 0
                };
                var blockTop = verticalOffset + carryOverVerticalOffset + block.Margin.TopMargin;
                var blockLeft = horizontalOffset + block.Margin.LeftMargin;
                var result = new Vector2Extender<UiBlock>
                {
                    Vector2 = new Vector2
                    {
                        X = blockLeft,
                        Y = blockTop
                    },
                    Subject = block
                };

                yield return result;

                if (UiVerticalJustificationType.SpaceBetween == this.VerticalJustificationType)
                    verticalOffset += spaceBetweenBlocks;

                verticalOffset += block.TotalHeight;
            }
        }

        /// <summary>
        /// Draws the debug drawable.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        /// <param name="gameServices">The game services.</param>
        public void DrawDebug(GameTime gameTime, GameServiceContainer gameServices)
        {
            var offset = new Vector2
            {
                X = 0,
                Y = -this.ScrollState?.VerticalScrollOffset ?? default
            };

            foreach (var block in this._blocks)
                block.DrawDebug(gameTime, gameServices, this.Position.Coordinates, Color.MonoGameOrange, offset);

            this.Area.Draw(gameTime, gameServices, Color.MonoGameOrange);
        }

        /// <summary>
        /// Disposes of the user interface container.
        /// </summary>
        public void Dispose()
        {
            this.ScrollState?.Dispose();
            this.CursorConfiguration?.Dispose();

            foreach (var block in this._blocks)
                block?.Dispose();
        }
    }
}

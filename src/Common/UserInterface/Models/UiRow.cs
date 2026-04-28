using Common.Controls.CursorInteractions.Models;
using Common.Controls.CursorInteractions.Models.Abstract;
using Common.Controls.CursorInteractions.Models.Contracts;
using Common.Controls.Cursors.Models;
using Common.UserInterface.Constants;
using Common.UserInterface.Enums;
using Common.UserInterface.Models.Contracts;
using Common.UserInterface.Services;
using Common.UserInterface.Services.Contracts;
using Engine.Debugging.Models.Contracts;
using Engine.Graphics.Models;
using Engine.Graphics.Models.Contracts;
using Engine.Physics.Models;
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
    sealed public class UiRow : IAmSubDrawable, IAmDebugSubDrawable, IHaveAHoverCursor, ICanBeHovered, IDisposable
    {
        /// <summary>
        /// Gets or sets user interface layout cache.
        /// </summary>
        public UiLayoutCache UiLayoutCache { get; set; }

        /// <summary>
        /// Gets or sets the user interface row name.
        /// </summary>
        required public string Name { get; set; }

        /// <summary>
        /// Get a value indicating whether the row has width flexible elements.
        /// </summary>
        public bool HasWidthFlexElements { get => this._elements.Any(e => e.IsFlexWidth); }

        /// <summary>
        /// Get a value indicating whether the row has height flexible elements.
        /// </summary>
        public bool HasHeightFlexElements { get => this._elements.Any(e => e.IsFlexHeight); }

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
        /// Gets the sub area.
        /// </summary>
        public SubArea SubArea { get => this.UiLayoutCache?.TotalArea; }

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
        public CursorConfiguration CursorConfiguration { private get; init; }

        /// <summary>
        /// The user interface elements.
        /// </summary>
        readonly public List<IAmAUiElement> _elements = [];

        /// <summary>
        /// Raises the hover event.
        /// </summary>
        /// <param name="cursorInteraction">The cursor interaction.</param>
        public void RaiseHoverEvent(CursorInteraction cursorInteraction)
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
            var graphicOffset = offset + (this.UiLayoutCache?.Offset ?? default);
            var backgroundOffset = graphicOffset + marginGraphicOffset;
            this.Graphic?.Draw(gameTime, gameServices, coordinates, color, backgroundOffset);

            foreach (var element in this._elements ?? [])
                element.Draw(gameTime, gameServices, coordinates, color, graphicOffset);
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
            var fixedSizedWidth = this._elements.Where(e => e.IsFlexWidth).Sum(e => e.TotalWidth);
            var fixedSizedHeight = this._elements.Where(e => e.IsFlexHeight).Sum(e => e.TotalHeight);
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
            this.UpdateElementSize(gameTime, gameServices, availableWidth, availabelHeight);
        }

        /// <summary>
        /// Updates the element size.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        /// <param name="gameServices">The game services.</param>
        /// <param name="availableWidth">The available width.</param>
        /// <param name="availabelHeight">The available height.</param>
        private void UpdateElementSize(GameTime gameTime, GameServiceContainer gameServices, float availableWidth, float availabelHeight)
        {
            if ((null == this.UiLayoutCache) ||
                (0 == this._elements.Count))
                return;

            availableWidth -= this.Margin.LeftMargin + this.Margin.RightMargin;
            availabelHeight -= this.Margin.TopMargin + this.Margin.BottomMargin;
            var uiElementService = gameServices.GetService<IUiElementService>();
            var uiScreenService = gameServices.GetService<IUiScreenService>();
            var flexWidthElements = this._elements.Where(e => true == UiGroupService._dynamicSizedTypes.Contains(e.HorizontalSizeType)).ToList();
            var flexHeightElements = this._elements.Where(e => true == UiGroupService._dynamicSizedTypes.Contains(e.VerticalSizeType)).ToList();

            if (0 != flexWidthElements.Count)
            {
                var flexWidth = availableWidth - this.UiLayoutCache.FixedSizedWidth / flexWidthElements.Count;
                var minWidth = uiScreenService.ScreenZoneSize.Width * ElementSizesScalars.ExtraSmall.X;

                if (flexWidth < minWidth)
                {
                    flexWidth = minWidth;
                    //logging
                }

                foreach (var element in flexWidthElements)
                    uiElementService.UpdateElementWidth(element, flexWidth);
            }

            if (0 != flexHeightElements.Count)
            {
                var flexHeight = availabelHeight - this.UiLayoutCache.FixedSizedHeight / flexHeightElements.Count;
                var minHeight = uiScreenService.ScreenZoneSize.Height * ElementSizesScalars.ExtraSmall.Y;

                if (flexHeight < minHeight)
                {
                    flexHeight = minHeight;
                    //logging
                }

                foreach (var element in flexHeightElements)
                    uiElementService.UpdateElementHeight(element, flexHeight);
            }

            var insideWidth = 0f;
            var insideHeight = 0f;

            if (UiHorizontalJustificationType.SpaceBetween == this.HorizontalJustificationType)
                insideWidth = availableWidth;
            else
                insideWidth = this._elements.Sum(e => e.TotalWidth);

            if (UiVerticalJustificationType.SpaceBetween == this.VerticalJustificationType)
                insideHeight = availabelHeight;
            else
                insideHeight = this._elements.Max(e => e.TotalHeight);

            this.UpdateRowArea(gameServices, insideWidth, insideHeight);
        }

        /// <summary>
        /// Updates the row area.
        /// </summary>
        /// <param name="gameServices">The game service.</param>
        /// <param name="insideWidth">The inside width.</param>
        /// <param name="insideHeight">The inside height.</param>
        private void UpdateRowArea(GameServiceContainer gameServices, float insideWidth, float insideHeight)
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

            if (null != this.CursorConfiguration)
                this.CursorConfiguration.SubArea = this.UiLayoutCache.TotalArea;

            if ((null != this.Graphic) &&
                ((true == this.ResizeTexture) ||
                 (this.Graphic is CompositeImage)))
                if (this.ExtendBackgroundToMargin)
                    this.Graphic.SetDrawDimensions(this.UiLayoutCache.TotalArea);
                else
                    this.Graphic.SetDrawDimensions(this.UiLayoutCache.InsideArea);
        }

        /// <summary>
        /// Updates the element offsets.
        /// </summary>
        public void UpdateElementOffsets()
        {
            foreach (var layout in this.EnumerateElementPosition() ?? [])
                layout.Subject.CachedOffset = layout.Vector2;
        }

        /// <summary>
        /// Enumerates the element positions.
        /// </summary>
        /// <returns>The enumerated element positions.</returns>
        public IEnumerable<Vector2Extender<IAmAUiElement>> EnumerateElementPosition()
        {
            var contentWidth = this._elements.Sum(e => e.TotalWidth);
            var horizontalOffset = this.HorizontalJustificationType switch
            {
                UiHorizontalJustificationType.Center => (this.InsideWidth - contentWidth) / 2,
                UiHorizontalJustificationType.Right => this.InsideWidth - contentWidth,
                _ => 0
            };

            if (horizontalOffset < 0)
                horizontalOffset = 0;

            var spaceBetweenBlocks = 0f;
            var carryOverHorizontalOffset = 0f;

            if (UiHorizontalJustificationType.SpaceBetween == this.HorizontalJustificationType)
                if (1 < this._elements.Count)
                    spaceBetweenBlocks = (this.TotalWidth - contentWidth) / (this._elements.Count - 1);
                else
                    carryOverHorizontalOffset = (this.TotalWidth - contentWidth) / 2;

            foreach (var element in this._elements)
            {
                var verticalOffset = this.VerticalJustificationType switch
                {
                    UiVerticalJustificationType.Center => (element.TotalHeight - this.InsideHeight) / 2,
                    UiVerticalJustificationType.Bottom => element.TotalHeight - this.InsideHeight,
                    _ => 0
                };

                if (verticalOffset < 0)
                    verticalOffset = 0;

                var elementTop = verticalOffset + element.Margin.TopMargin;
                var elementLeft = horizontalOffset + carryOverHorizontalOffset + element.Margin.LeftMargin;
                var result = new Vector2Extender<IAmAUiElement>
                {
                    Vector2 = new Vector2
                    {
                        X = elementLeft,
                        Y = elementTop
                    },
                    Subject = element,
                };

                yield return result;

                if (UiHorizontalJustificationType.SpaceBetween == this.HorizontalJustificationType)
                    carryOverHorizontalOffset += spaceBetweenBlocks;

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
            var graphicOffset = offset + (this.UiLayoutCache?.Offset ?? default);

            foreach (var element in this._elements)
                element.DrawDebug(gameTime, gameServices, coordinates, color, graphicOffset);

            this.UiLayoutCache?.TotalArea.Draw(gameTime, gameServices, coordinates, color, graphicOffset);
        }

        /// <summary>
        /// Disposes of the user interface row.
        /// </summary>
        public void Dispose()
        {
            this.CursorConfiguration?.Dispose();

            foreach (var subElement in this._elements ?? [])
                subElement?.Dispose();
        }
    }
}

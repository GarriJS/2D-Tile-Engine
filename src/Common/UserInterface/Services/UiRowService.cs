using Common.Controls.Cursors.Models;
using Common.Controls.Cursors.Services.Contracts;
using Common.DiskModels.UserInterface;
using Common.UserInterface.Models;
using Common.UserInterface.Models.Contracts;
using Common.UserInterface.Services.Contracts;
using Engine.Graphics.Models.Contracts;
using Engine.Graphics.Services.Contracts;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Common.UserInterface.Services
{
    /// <summary>
    /// Represents a user interface row service.
    /// </summary>
    /// <remarks>
    /// Initializes the user interface row service.
    /// </remarks>
    /// <param name="gameServices">The game services.</param>
    sealed public class UiRowService(GameServiceContainer gameServices) : IUiRowService
    {
        readonly private GameServiceContainer _gameServices = gameServices;

        /// <summary>
        /// Gets the user interface row.
        /// </summary>
        /// <param name="uiRowModel">The user interface block row.</param>
        /// <returns>The user interface row.</returns>
        public UiRow GetUiRowFromModel(UiRowModel uiRowModel)
        {
            var uiElementService = this._gameServices.GetService<IUiElementService>();
            var imageService = this._gameServices.GetService<IImageService>();
            var cursorService = this._gameServices.GetService<ICursorService>();
            var uiZoneService = this._gameServices.GetService<IUiScreenService>();
            var uiMarginService = this._gameServices.GetService<IUiMarginService>();
            var subElements = new List<IAmAUiElement>();

            foreach (var subElementModel in uiRowModel.Elements ?? [])
            {
                var subElement = uiElementService.GetUiElement(subElementModel);
                subElements.Add(subElement);
            }

            var margin = uiMarginService.GetUiMarginFromModel(uiRowModel.Margin);
            IAmAGraphic background = null;

            if (uiRowModel.BackgroundTexture is not null)
                background = imageService.GetImageFromModel(uiRowModel.BackgroundTexture);

            Cursor hoverCursor = null;

            if ((false == string.IsNullOrEmpty(uiRowModel.HoverCursorName)) &&
                (false == cursorService.Cursors.TryGetValue(uiRowModel.HoverCursorName, out hoverCursor)))
            {
                // LOGGING
            }

            var result = new UiRow
            {
                Name = uiRowModel.Name,
                ResizeTexture = uiRowModel.ResizeTexture,
                ExtendBackgroundToMargin = uiRowModel.ExtendBackgroundToMargin,
                Margin = margin,
                HorizontalJustificationType = uiRowModel.HorizontalJustificationType,
                VerticalJustificationType = uiRowModel.VerticalJustificationType,
                Graphic = background,
                HoverCursor = hoverCursor
            };
            result._elements.AddRange(subElements);

            return result;
        }

        /// <summary>
        /// Splits the row to accommodate the max width.
        /// </summary>
        /// <param name="uiRow">The user interface row.</param>
        /// <param name="maxWidth">The max width.</param>
        /// <param name="targetWidth">The target width.</param>
        /// <returns>The split rows.</returns>
        public UiRow[] SplitRow(UiRow uiRow, float maxWidth, float? targetWidth = null)
        {
            if (false == targetWidth.HasValue)
                targetWidth = maxWidth;

            if (uiRow.TotalWidth <= targetWidth)
                return [uiRow];

            var rows = new List<UiRow>();
            var elementList = new List<IAmAUiElement>();

            foreach (var element in uiRow._elements)
            {
                var listWidth = elementList.Sum(e => e.TotalWidth) + uiRow.Margin.LeftMargin + uiRow.Margin.RightMargin;

                if (listWidth + element.TotalWidth > targetWidth)
                { 
                    var newRow = new UiRow
                    {
                        Name = $"{uiRow.Name}_{rows.Count + 1}",
                        ResizeTexture = uiRow.ResizeTexture,
                        ExtendBackgroundToMargin = uiRow.ExtendBackgroundToMargin,
                        Margin = uiRow.Margin.Copy(),
                        HorizontalJustificationType = uiRow.HorizontalJustificationType,
                        VerticalJustificationType = uiRow.VerticalJustificationType,
                        Graphic = uiRow.Graphic, //copy?
                        HoverCursor = uiRow.HoverCursor,
                        CursorConfiguration = uiRow.CursorConfiguration
                    };
                    newRow._elements.AddRange(elementList);
                    rows.Add(newRow);
                    elementList.Clear();
                }

                elementList.Add(element);
            }

            if (0 != elementList.Count)
            {
                var lastRow = new UiRow
                {
                    Name = $"{uiRow.Name}_{rows.Count + 1}",
                    ResizeTexture = uiRow.ResizeTexture,
                    ExtendBackgroundToMargin = uiRow.ExtendBackgroundToMargin,
                    Margin = uiRow.Margin.Copy(),
                    HorizontalJustificationType = uiRow.HorizontalJustificationType,
                    VerticalJustificationType = uiRow.VerticalJustificationType,
                    Graphic = uiRow.Graphic, //copy?
                    HoverCursor = uiRow.HoverCursor,
                    CursorConfiguration = uiRow.CursorConfiguration
                };
                lastRow._elements.AddRange(elementList);
                rows.Add(lastRow);
            }

            var result = rows.ToArray();

            return result;
        }
    }
}

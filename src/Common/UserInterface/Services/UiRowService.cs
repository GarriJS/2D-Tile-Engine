using Common.Controls.CursorInteraction.Services.Contracts;
using Common.Controls.Cursors.Models;
using Common.Controls.Cursors.Services.Contracts;
using Common.DiskModels.UserInterface;
using Common.UserInterface.Constants;
using Common.UserInterface.Enums;
using Common.UserInterface.Models;
using Common.UserInterface.Models.Contracts;
using Common.UserInterface.Services.Contracts;
using Engine.Graphics.Models;
using Engine.Graphics.Models.Contracts;
using Engine.Graphics.Services.Contracts;
using Engine.Physics.Models.SubAreas;
using Microsoft.Xna.Framework;
using System;
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
			var cursorInteractionService = this._gameServices.GetService<ICursorInteractionService>();
			var uiZoneService = this._gameServices.GetService<IUiScreenService>();
			var uiMarginService = this._gameServices.GetService<IUiMarginService>();
			var zoneArea = uiZoneService.ScreenZoneSize;
			var subElements = new List<IAmAUiElement>();

			foreach (var subElementModel in uiRowModel.Elements ?? [])
			{
				var subElement = uiElementService.GetUiElement(subElementModel);
				subElements.Add(subElement);
			}

			var contentWidth = subElements.Sum(e => e.TotalWidth);
			var contentHeight = subElements.Select(e => e.TotalHeight)
										   .OrderDescending()
										   .FirstOrDefault();
			var dynamicWidthElements = subElements.Where(e => UiElementSizeType.FlexMax == e.HorizontalSizeType)
												  .ToArray();
			var remainingWidth = zoneArea.Width - contentWidth;
			var dynamicWidth = remainingWidth / dynamicWidthElements.Length;

			if (zoneArea.Width * ElementSizesScalars.ExtraSmall.X > dynamicWidth)
				dynamicWidth = zoneArea.Width * ElementSizesScalars.ExtraSmall.X;

			foreach (var dynamicWidthElement in dynamicWidthElements)
				dynamicWidthElement.Area.Width = dynamicWidth;

			var margin = uiMarginService.GetUiMarginFromModel(uiRowModel.Margin);
			var rowArea = new SubArea
			{
				Width = contentWidth,
				Height = contentHeight
			};
			IAmAGraphic background = null;

			if (uiRowModel.BackgroundTexture is not null)
			{
				var graphicArea = rowArea;

				if (true == uiRowModel.ExtendBackgroundToMargin)
					graphicArea = new SubArea
					{
						Width = rowArea.Width + margin.LeftMargin + margin.RightMargin,
						Height = rowArea.Height + margin.TopMargin + margin.BottomMargin
					};

				background = imageService.GetImageFromModel(uiRowModel.BackgroundTexture);

				if ((true == uiRowModel.ResizeTexture) ||
					(background is CompositeImage))
					background.SetDrawDimensions(graphicArea);
			}

			Cursor hoverCursor = null;

			if ((false == string.IsNullOrEmpty(uiRowModel.HoverCursorName)) &&
				(false == cursorService.Cursors.TryGetValue(uiRowModel.HoverCursorName, out hoverCursor)))
			{
				// LOGGING
			}

			var cursorConfiguration = cursorInteractionService.GetCursorConfiguration<UiRow>(rowArea, null);
			var result = new UiRow
			{
				Name = uiRowModel.Name,
				Flex = true,
				ExtendBackgroundToMargin = uiRowModel.ExtendBackgroundToMargin,
				AvailableWidth = zoneArea.Width,
				Area = rowArea,
				Margin = margin,
				HorizontalJustificationType = uiRowModel.HorizontalJustificationType,
				VerticalJustificationType = uiRowModel.VerticalJustificationType,
				Graphic = background,
				HoverCursor = hoverCursor,
				CursorConfiguration = cursorConfiguration
			};
			result._elements.AddRange(subElements);

			return result;
		}

		/// <summary>
		/// Splits the block to accommodate  
		/// </summary>
		/// <param name="uiRow">The user interface block.</param>
		/// <param name="originalModel">The original row model.</param>
		/// <param name="maxWidth">The max width.</param>
		/// <returns>The user interface rows.</returns>
		public UiRow[] SplitRow(UiRow uiRow, UiRowModel originalModel, float maxWidth)
		{
			if (uiRow._elements.Count <= 1)
				return [uiRow];

			var imageService = this._gameServices.GetService<IImageService>();
			var splitEven = uiRow.HorizontalJustificationType == UiHorizontalJustificationType.Center;
			var newRows = new List<UiRow>();
			var currentRow = 1;
			var currentElements = new List<IAmAUiElement>();
			var currentWidth = 0f;
			var targetWidth = maxWidth;

			if (true == splitEven)
			{
				var totalWidth = uiRow._elements.Sum(e => e.TotalWidth);
				var rowCount = (int)MathF.Ceiling(totalWidth / maxWidth);
				targetWidth = (totalWidth / rowCount) + uiRow._elements.OrderBy(e => e.TotalWidth)
																	  .First().TotalWidth;
			}

			foreach (var element in uiRow._elements)
			{
				var elementWidth = element.TotalWidth;

				if (0 == currentElements.Count)
				{
					currentElements.Add(element);
					currentWidth = elementWidth;

					continue;
				}

				var wouldOverflowHard = currentWidth + elementWidth > maxWidth;
				var wouldExceedTarget = splitEven && currentWidth + elementWidth > targetWidth;

				if ((true == wouldOverflowHard) ||
					(true == wouldExceedTarget))
				{
					var contentWidth = currentElements.Sum(e => e.TotalWidth);
					var contentHeight = currentElements.Select(e => e.TotalHeight)
													   .OrderDescending()
													   .FirstOrDefault();
					var area = new SubArea
					{
						Width = contentWidth,
						Height = contentHeight,
					};
					var margin = uiRow.Margin.Copy();
					IAmAGraphic background = null;

					if (originalModel.BackgroundTexture is not null)
					{
						var graphicArea = area;

						if (true == uiRow.ExtendBackgroundToMargin)
							graphicArea = new SubArea
							{
								Width = area.Width + margin.LeftMargin + margin.RightMargin,
								Height = area.Height + margin.TopMargin + margin.BottomMargin
							};

						background = imageService.GetImageFromModel(originalModel.BackgroundTexture);

						if ((true == originalModel.ResizeTexture) ||
							(background is CompositeImage))
							background.SetDrawDimensions(graphicArea);
					}

					var newRow = new UiRow
					{
						Name = $"{uiRow.Name}_{currentRow}",
						Flex = uiRow.Flex,
						ExtendBackgroundToMargin = uiRow.ExtendBackgroundToMargin,
						AvailableWidth = uiRow.AvailableWidth,
						Margin = uiRow.Margin.Copy(),
						HorizontalJustificationType = uiRow.HorizontalJustificationType,
						VerticalJustificationType = uiRow.VerticalJustificationType,
						Area = area,
						Graphic = background,
						HoverCursor = uiRow.HoverCursor,
						CursorConfiguration = uiRow.CursorConfiguration
					};
					newRow._elements.AddRange(currentElements);
					currentRow++;
					newRows.Add(newRow);
					currentElements = [element];
					currentWidth = elementWidth;
				}
				else
				{
					currentElements.Add(element);
					currentWidth += elementWidth;
				}
			}

			if (0 < currentElements.Count)
			{
				var contentWidth = currentElements.Sum(e => e.TotalWidth);
				var contentHeight = currentElements.Select(e => e.TotalHeight)
												   .OrderDescending()
												   .FirstOrDefault();
				var area = new SubArea
				{
					Width = contentWidth,
					Height = contentHeight,
				};
				var margin = uiRow.Margin.Copy();
				IAmAGraphic background = null;

				if (originalModel.BackgroundTexture is not null)
				{
					var graphicArea = area;

					if (true == uiRow.ExtendBackgroundToMargin)
						graphicArea = new SubArea
						{
							Width = area.Width + margin.LeftMargin + margin.RightMargin,
							Height = area.Height + margin.TopMargin + margin.BottomMargin
						};

					background = imageService.GetImageFromModel(originalModel.BackgroundTexture);

					if ((true == originalModel.ResizeTexture) ||
						(background is CompositeImage))
						background.SetDrawDimensions(graphicArea);
				}

				var newRow = new UiRow
				{
					Name = $"{uiRow.Name}_{currentRow}",
					Flex = uiRow.Flex,
					ExtendBackgroundToMargin = uiRow.ExtendBackgroundToMargin,
					AvailableWidth = uiRow.AvailableWidth,
					Margin = margin,
					HorizontalJustificationType = uiRow.HorizontalJustificationType,
					VerticalJustificationType = uiRow.VerticalJustificationType,
					Area = area,
					Graphic = background,
					HoverCursor = uiRow.HoverCursor,
					CursorConfiguration = uiRow.CursorConfiguration
				};
				newRow._elements.AddRange(currentElements);
				newRows.Add(newRow);
			}

			var result = newRows.ToArray();

			return result;
		}

		/// <summary>
		/// Updates the block dynamic height.
		/// </summary>
		/// <param name="uiRow">The user interface block.</param>
		/// <param name="dynamicHeight">The dynamic height.</param>
		public void UpdateRowDynamicHeight(UiRow uiRow, float dynamicHeight)
		{
			var dimensions = new SubArea
			{
				Width = uiRow.InsideWidth,
				Height = dynamicHeight + uiRow.Margin.TopMargin + uiRow.Margin.BottomMargin
			};
			uiRow.Graphic?.SetDrawDimensions(dimensions);

			if (0 == uiRow._elements.Count)
				return;

			var uiElementService = this._gameServices.GetService<IUiElementService>();
			var dynamicHeightElements = uiRow._elements.Where(e => true == UiGroupService._dynamicSizedTypes.Contains(e.VerticalSizeType))
													   .ToList();

			foreach (var uiElement in dynamicHeightElements ?? [])
				uiElementService.UpdateElementHeight(uiElement, dynamicHeight);

			var contentHeight = uiRow._elements.Select(e => e.TotalHeight)
											  .OrderDescending()
											  .FirstOrDefault();
			uiRow.Area.Height = contentHeight;
		}
	}
}

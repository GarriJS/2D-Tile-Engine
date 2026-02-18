using Common.Controls.CursorInteraction.Services.Contracts;
using Common.Controls.Cursors.Models;
using Common.Controls.Cursors.Services.Contracts;
using Common.DiskModels.UserInterface;
using Common.UserInterface.Constants;
using Common.UserInterface.Models;
using Common.UserInterface.Services.Contracts;
using Engine.Graphics.Models;
using Engine.Graphics.Models.Contracts;
using Engine.Graphics.Services.Contracts;
using Engine.Physics.Models.SubAreas;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Common.UserInterface.Services
{
	/// <summary>
	/// Represents a user interface block service.
	/// </summary>
	/// <remarks>
	/// Initializes the user interface block service.
	/// </remarks>
	/// <param name="gameServices">The game services.</param>
	sealed public class UiBlockService(GameServiceContainer gameServices) : IUiBlockService
	{
		readonly private GameServiceContainer _gameServices = gameServices;

		/// <summary>
		/// Get the user interface block from the model.
		/// </summary>
		/// <param name="uiBlockModel">The user interface block model.</param>
		/// <param name="outterArea">The encapsulating area of the block.</param>
		/// <returns>The user interface block.</returns>
		public UiBlock GetUiBlockFromModel(UiBlockModel uiBlockModel, SubArea outterArea = null)
		{
			var uiElementService = this._gameServices.GetService<IUiElementService>();
			var imageService = this._gameServices.GetService<IImageService>();
			var cursorService = this._gameServices.GetService<ICursorService>();
			var cursorInteractionService = this._gameServices.GetService<ICursorInteractionService>();
			var uiScreenService = this._gameServices.GetService<IUiScreenService>();
			var scrollStateService = this._gameServices.GetService<IScrollStateService>();
			var uiMarginService = this._gameServices.GetService<IUiMarginService>();
			var uiRowService = this._gameServices.GetService<IUiRowService>();
			outterArea ??= uiScreenService.ScreenZoneSize;
			var rows = new List<UiRow>();

			foreach (var rowModel in uiBlockModel.Rows ?? [])
			{
				var row = uiRowService.GetUiRowFromModel(rowModel);

				if ((true == uiBlockModel.FlexRows) &&
					(row.TotalWidth > outterArea.Width))
				{
					var splitRows = uiRowService.SplitRow(row, rowModel, outterArea.Width);
					rows.AddRange(splitRows);
				}
				else
					rows.Add(row);
			}

			var contentWidth = rows.Select(e => e.TotalWidth).OrderDescending().FirstOrDefault();
			var contentHeight = rows.Sum(e => e.TotalHeight);
			var dynamicRows = rows.Where(r => r._elements.Any(e => UiGroupService._dynamicSizedTypes.Contains(e.VerticalSizeType))).ToArray();
			var remainingWidth = outterArea.Width - contentWidth;
			var dynamicWidth = remainingWidth / dynamicRows.Length;

			if (outterArea.Width * ElementSizesScalars.ExtraSmall.X > dynamicWidth)
			{
				// LOGGING
				dynamicWidth = outterArea.Width * ElementSizesScalars.ExtraSmall.X;
			}

			foreach (var dynamicRow in dynamicRows)
				dynamicRow.Area.Width = dynamicWidth;

			contentWidth = rows.Select(e => e.TotalWidth).OrderDescending().FirstOrDefault();

			if (contentWidth <= 0)
			{
				// LOGGING
				contentWidth = uiScreenService.ScreenZoneSize.Width * ElementSizesScalars.ExtraSmall.X;
			}

			if (contentHeight <= 0)
			{
				// LOGGING
				contentHeight = uiScreenService.ScreenZoneSize.Height * ElementSizesScalars.ExtraSmall.Y;
			}

			var margin = uiMarginService.GetUiMarginFromModel(uiBlockModel.Margin);
			var area = new SubArea
			{
				Width = contentWidth,
				Height = contentHeight
			};
			IAmAGraphic background = null;

			if (uiBlockModel.BackgroundTexture is not null)
			{
				var graphicArea = new SubArea
				{ 
					Width = area.Width,
					Height = area.Height
				};

				if (true == uiBlockModel.ExtendBackgroundToMargin)
					graphicArea = new SubArea
					{
						Width = outterArea.Width,
						Height = area.Height + margin.TopMargin + margin.BottomMargin
					};

				background = imageService.GetImageFromModel(uiBlockModel.BackgroundTexture);

				if ((true == uiBlockModel.ResizeTexture) ||
					(background is CompositeImage))
					background.SetDrawDimensions(graphicArea);
			}

			Cursor hoverCursor = null;

			if ((false == string.IsNullOrEmpty(uiBlockModel.HoverCursorName)) &&
				(false == cursorService.Cursors.TryGetValue(uiBlockModel.HoverCursorName, out hoverCursor)))
			{
				// LOGGING
			}

			var scrollState = scrollStateService.GetScrollStateFromModel(uiBlockModel.ScrollStateModel);
			var cursorConfiguration = cursorInteractionService.GetCursorConfiguration<UiBlock>(area, null);
			var result = new UiBlock
			{
				Name = uiBlockModel.Name,
				FlexRows = true,
				ExtendBackgroundToMargin = uiBlockModel.ExtendBackgroundToMargin,
				AvailableWidth = outterArea.Width,
				Area = area,
				Margin = margin,
				HorizontalJustificationType = uiBlockModel.HorizontalJustificationType,
				VerticalJustificationType = uiBlockModel.VerticalJustificationType,
				Graphic = background,
				ScrollState = scrollState,
				HoverCursor = hoverCursor,
				CursorConfiguration = cursorConfiguration
			};
			result._rows.AddRange(rows);

			return result;
		}
	}
}

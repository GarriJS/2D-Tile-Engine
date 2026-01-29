using Common.Controls.CursorInteraction.Services.Contracts;
using Common.Controls.Cursors.Models;
using Common.Controls.Cursors.Services.Contracts;
using Common.Core.Constants;
using Common.DiskModels.UserInterface;
using Common.UserInterface.Constants;
using Common.UserInterface.Enums;
using Common.UserInterface.Models;
using Common.UserInterface.Models.Contracts;
using Common.UserInterface.Models.Elements;
using Common.UserInterface.Services.Contracts;
using Engine.Core.Initialization.Services.Contracts;
using Engine.Graphics.Models;
using Engine.Graphics.Models.Contracts;
using Engine.Graphics.Services.Contracts;
using Engine.Physics.Models.SubAreas;
using Engine.RunTime.Services.Contracts;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.UserInterface.Services
{
	/// <summary>
	/// Represents a user interface service.
	/// </summary>
	/// <remarks>
	/// ConfigureService the user interface service.
	/// </remarks>
	/// <param name="gameServices">The game services.</param>
	public class UserInterfaceService(GameServiceContainer gameServices) : IUserInterfaceService
	{
		private readonly GameServiceContainer _gameServices = gameServices;

		/// <summary>
		/// Gets the active visibility group id.
		/// </summary>
		public int? ActiveVisibilityGroupId { get; private set; }

		/// <summary>
		/// Gets or sets the user interface groups.
		/// </summary>
		public List<UiGroup> UserInterfaceGroups { get; set; } = [];

		/// <summary>
		/// Gets the dynamic sized types.
		/// </summary>
		readonly static public List<UiElementSizeType> DynamicSizedTypes =
		[
			UiElementSizeType.FlexMax
		];

		/// <summary>
		/// Adds the user interface zone to the user interface group.
		/// </summary>
		/// <param name="visibilityGroupId">The visibility group id.</param>
		/// <param name="uiZone">The user interface zone.</param>
		public void AddUserInterfaceZoneToUserInterfaceGroup(int visibilityGroupId, UiZone uiZone)
		{
			var uiGroup = this.UserInterfaceGroups.FirstOrDefault(e => e.VisibilityGroupId == visibilityGroupId);

			if (null == uiGroup)
			{
				// LOGGING
				return;
			}

			var runTimeOverlaidDrawService = this._gameServices.GetService<IRuntimeOverlaidDrawService>();
			var existingZone = uiGroup.Zones.FirstOrDefault(e => e.UserInterfaceScreenZone.UiZoneType == uiZone.UserInterfaceScreenZone.UiZoneType);

			if (null != uiGroup.Zones.FirstOrDefault(e => e.UserInterfaceScreenZone.UiZoneType == uiZone.UserInterfaceScreenZone.UiZoneType))
			{
				runTimeOverlaidDrawService.RemoveDrawable(existingZone);
				uiGroup.Zones.Remove(existingZone);
				existingZone.Dispose();
			}

			uiGroup.Zones.Add(uiZone);
			runTimeOverlaidDrawService.AddDrawable(uiZone);
		}

		/// <summary>
		/// Toggles the user interface group visibility.
		/// </summary>
		/// <param name="visibilityGroupId">The visibility group id.</param>
		public void ToggleUserInterfaceGroupVisibility(int visibilityGroupId)
		{
			var userInterfaceGroup = this.UserInterfaceGroups.FirstOrDefault(e => e.VisibilityGroupId == visibilityGroupId);
			this.ToggleUserInterfaceGroupVisibility(userInterfaceGroup);
		}

		/// <summary>
		/// Toggles the user interface group visibility.
		/// </summary>
		/// <param name="uiGroup">The user interface group.</param>
		public void ToggleUserInterfaceGroupVisibility(UiGroup uiGroup)
		{
			if (null == uiGroup)
				return;

			var runTimeOverlaidDrawService = this._gameServices.GetService<IRuntimeOverlaidDrawService>();

			if (true == this.ActiveVisibilityGroupId.HasValue)
			{
				var activeGroup = this.UserInterfaceGroups.FirstOrDefault(e => e.VisibilityGroupId == this.ActiveVisibilityGroupId);

				foreach (var uiZoneContainer in activeGroup.Zones)
					runTimeOverlaidDrawService.RemoveDrawable(uiZoneContainer);
			}

			this.ActiveVisibilityGroupId = uiGroup.VisibilityGroupId;

			if (0 == uiGroup.Zones.Count)
				return;

			var animationService = this._gameServices.GetService<IAnimationService>();

			foreach (var uiZone in uiGroup.Zones)
			{
				runTimeOverlaidDrawService.AddDrawable(uiZone);
				var clickAnimations = uiZone.Blocks.SelectMany(e => e.Rows)
												   .SelectMany(e => e.Elements)
												   .OfType<UiButton>()
												   .Where(e => null != e.ClickAnimation)
												   .Select(e => e.ClickAnimation)
												   .ToArray();

				if (0 == clickAnimations.Length)
					continue;

				foreach (var clickAnimation in clickAnimations)
					clickAnimation.ResetTriggeredAnimation();
			}
		}

		/// <summary>
		/// Gets the user interface group.
		/// </summary>
		/// <param name="uiGroupModel">The user interface group model.</param>
		/// <returns>The user interface group.</returns>
		public UiGroup GetUiGroup(UiGroupModel uiGroupModel)
		{
			var uiZones = new List<UiZone>();

			foreach (var uiZoneElementModel in uiGroupModel.Zones)
			{
				var uiZoneElement = this.GetUiZone(uiZoneElementModel);

				if (null != uiZoneElement)
					uiZones.Add(uiZoneElement);
			}

			var uiGroup = new UiGroup
			{
				Name = uiGroupModel.Name,
				VisibilityGroupId = uiGroupModel.VisibilityGroupId,
				Zones = uiZones
			};

			this.UserInterfaceGroups.Add(uiGroup);

			if (true == uiGroupModel.IsVisible)
				this.ToggleUserInterfaceGroupVisibility(uiGroup.VisibilityGroupId);

			return uiGroup;
		}

		/// <summary>
		/// Gets the user interface zone.
		/// </summary>
		/// <param name="uiZoneModel">The user interface model.</param>
		/// <returns>The user interface zone.</returns>
		public UiZone GetUiZone(UiZoneModel uiZoneModel)
		{
			var uiZoneService = this._gameServices.GetService<IUserInterfaceScreenZoneService>();
			var uiElementService = this._gameServices.GetService<IUserInterfaceElementService>();
			var functionService = this._gameServices.GetService<IFunctionService>();
			var imageService = this._gameServices.GetService<IImageService>();
			var cursorService = this._gameServices.GetService<ICursorService>();
			var cursorInteractionService = this._gameServices.GetService<ICursorInteractionService>();

			if (false == uiZoneService.UserInterfaceScreenZones.TryGetValue(uiZoneModel.UiZonePositionType, out UiScreenZone uiScreenZone))
				uiScreenZone = uiZoneService.UserInterfaceScreenZones[UiZonePositionType.Unknown];

			var blocks = new List<UiBlock>();

			foreach (var uiBlockModel in uiZoneModel.Blocks ?? [])
			{
				var block = this.GetUiBlock(uiBlockModel);
				blocks.Add(block);
			}

			var contentHeight = blocks.Sum(e => e.TotalHeight);
			var rows = blocks.Where(e => e.Rows.Count != 0)
							 .SelectMany(e => e.Rows)
							 .ToArray();
			var dynamicRows = rows.Where(r => r.Elements.Any(e => DynamicSizedTypes.Contains(e.VerticalSizeType)))
								  .ToArray();
			var remainingHeight = uiScreenZone.Area.Height - contentHeight;
			var dynamicHeight = remainingHeight / dynamicRows.Length;

			if (uiScreenZone.Area.Height * ElementSizesScalars.ExtraSmall.Y > dynamicHeight)
			{
				// LOGGING

				dynamicHeight = uiScreenZone.Area.Height * ElementSizesScalars.ExtraSmall.Y;
			}

			foreach (var dynamicRow in dynamicRows ?? [])
			{
				this.UpdateRowDynamicHeight(dynamicRow, dynamicHeight);
			}

			IAmAGraphic background = null;

			if (null != uiZoneModel.BackgroundTexture)
			{
				background = imageService.GetImageFromModel(uiZoneModel.BackgroundTexture);

				if ((true == uiZoneModel.ResizeTexture) ||
					(background is CompositeImage))
					background.SetDrawDimensions(uiScreenZone.Area.ToSubArea);
			}

			Cursor hoverCursor = null;

			if ((false == string.IsNullOrEmpty(uiZoneModel.HoverCursorName)) &&
				(false == cursorService.Cursors.TryGetValue(uiZoneModel.HoverCursorName, out hoverCursor)))
			{
				// LOGGING
			}

			var cursorConfiguration = cursorInteractionService.GetCursorConfiguration<UiZone>(uiScreenZone.Area.ToSubArea, null);
			var uiZone = new UiZone
			{
				ResetCalculateCachedOffsets = true,
				Name = uiZoneModel.Name,
				DrawLayer = RunTimeConstants.BaseUiDrawLayer,
				VerticalJustificationType = uiZoneModel.VerticalJustificationType,
				Graphic = background,
				HoverCursor = hoverCursor,
				CursorConfiguration = cursorConfiguration,
				UserInterfaceScreenZone = uiScreenZone,
				Blocks = blocks
			};

			return uiZone;
		}

		/// <summary>
		/// Get the user interface uiBlock.
		/// </summary>
		/// <param name="uiBlockModel">The user interface uiBlock model.</param>
		/// <returns>The user interface uiBlock.</returns>
		public UiBlock GetUiBlock(UiBlockModel uiBlockModel)
		{
			var uiElementService = this._gameServices.GetService<IUserInterfaceElementService>();
			var imageService = this._gameServices.GetService<IImageService>();
			var cursorService = this._gameServices.GetService<ICursorService>();
			var cursorInteractionService = this._gameServices.GetService<ICursorInteractionService>();
			var uiZoneService = this._gameServices.GetService<IUserInterfaceScreenZoneService>();
			var zoneArea = uiZoneService.ScreenZoneSize;
			var rows = new List<UiRow>();

			foreach (var rowModel in uiBlockModel.Rows ?? [])
			{
				var row = this.GetUiRow(rowModel);

				if ((true == uiBlockModel.FlexRows) &&
					(row.TotalWidth > zoneArea.Width))
				{
					var splitRows = this.SplitRow(row, rowModel, zoneArea.Width);
					rows.AddRange(splitRows);
				}
				else
					rows.Add(row);
			}

			var contentWidth = rows.Sum(e => e.TotalWidth);
			var contentHeight = rows.Sum(e => e.TotalHeight);
			var dynamicRows = rows.Where(r => r.Elements.Any(e => DynamicSizedTypes.Contains(e.VerticalSizeType))).ToArray();
			var remainingWidth = zoneArea.Width - contentWidth;
			var dynamicWidth = remainingWidth / dynamicRows.Length;

			if (zoneArea.Width * ElementSizesScalars.ExtraSmall.X > dynamicWidth)
			{
				// LOGGING

				dynamicWidth = zoneArea.Width * ElementSizesScalars.ExtraSmall.X;
			}

			foreach (var dynamicRow in dynamicRows)
				dynamicRow.Area.Width = dynamicWidth;

			var margin = uiElementService.GetUiMarginFromModel(uiBlockModel.Margin);
			var blockArea = new SubArea
			{
				Width = zoneArea.Width,
				Height = contentHeight
			};
			IAmAGraphic background = null;

			if (null != uiBlockModel.BackgroundTexture)
			{
				var graphicArea = blockArea;

				if (true == uiBlockModel.ExtendBackgroundToMargin)
					graphicArea = new SubArea
					{
						Width = blockArea.Width,
						Height = blockArea.Height + margin.TopMargin + margin.BottomMargin
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

			var cursorConfiguration = cursorInteractionService.GetCursorConfiguration<UiBlock>(blockArea, null);
			var result = new UiBlock
			{
				Name = uiBlockModel.Name,
				FlexRows = true,
				ExtendBackgroundToMargin = uiBlockModel.ExtendBackgroundToMargin,
				AvailableWidth = zoneArea.Width,
				Area = blockArea,
				Margin = margin,
				HorizontalJustificationType = uiBlockModel.HorizontalJustificationType,
				VerticalJustificationType = uiBlockModel.VerticalJustificationType,
				Graphic = background,
				HoverCursor = hoverCursor,
				CursorConfiguration = cursorConfiguration,
				Rows = rows
			};

			return result;
		}

		/// <summary>
		/// Gets the user interface block.
		/// </summary>
		/// <param name="uiRowModel">The user interface block model.</param>
		/// <returns>The user interface block.</returns>
		public UiRow GetUiRow(UiRowModel uiRowModel)
		{
			var uiElementService = this._gameServices.GetService<IUserInterfaceElementService>();
			var imageService = this._gameServices.GetService<IImageService>();
			var cursorService = this._gameServices.GetService<ICursorService>();
			var cursorInteractionService = this._gameServices.GetService<ICursorInteractionService>();
			var uiZoneService = this._gameServices.GetService<IUserInterfaceScreenZoneService>();
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

			var margin = uiElementService.GetUiMarginFromModel(uiRowModel.Margin);
			var rowArea = new SubArea
			{
				Width = contentWidth,
				Height = contentHeight
			};
			IAmAGraphic background = null;

			if (null != uiRowModel.BackgroundTexture)
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
				CursorConfiguration = cursorConfiguration,
				Elements = subElements
			};

			return result;
		}

		/// <summary>
		/// Splits the block to accommodate  
		/// </summary>
		/// <param name="uiRow">The user interface block.</param>
		/// <param name="originalModel">The original row model.</param>
		/// <param name="maxWidth">The max width.</param>
		/// <returns>The user interface rows.</returns>
		private UiRow[] SplitRow(UiRow uiRow, UiRowModel originalModel, float maxWidth)
		{
			if (uiRow.Elements.Count <= 1)
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
				var totalWidth = uiRow.Elements.Sum(e => e.TotalWidth);
				var rowCount = (int)MathF.Ceiling(totalWidth / maxWidth);
				targetWidth = (totalWidth / rowCount) + uiRow.Elements.OrderBy(e => e.TotalWidth)
																	  .First().TotalWidth;
			}

			foreach (var element in uiRow.Elements)
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

					if (null != originalModel.BackgroundTexture)
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
						CursorConfiguration = uiRow.CursorConfiguration,
						Elements = currentElements
					};

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

				if (null != originalModel.BackgroundTexture)
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
					CursorConfiguration = uiRow.CursorConfiguration,
					Elements = currentElements
				};

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
		private void UpdateRowDynamicHeight(UiRow uiRow, float dynamicHeight)
		{
			var dimensions = new SubArea
			{
				Width = uiRow.InsideWidth,
				Height = dynamicHeight + uiRow.Margin.TopMargin + uiRow.Margin.BottomMargin
			};
			uiRow.Graphic?.SetDrawDimensions(dimensions);

			if (0 == uiRow.Elements.Count)
				return;

			var uiElementService = this._gameServices.GetService<IUserInterfaceElementService>();
			var dynamicHeightElements = uiRow.Elements.Where(e => true == DynamicSizedTypes.Contains(e.VerticalSizeType))
													  .ToList();

			foreach (var uiElement in dynamicHeightElements ?? [])
				uiElementService.UpdateElementHeight(uiElement, dynamicHeight);

			var contentHeight = uiRow.Elements.Select(e => e.TotalHeight)
										      .OrderDescending()
											  .FirstOrDefault();
			uiRow.Area.Height = contentHeight;
		}
	}
}

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
	/// Initialize the user interface service.
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

			var existingZone = uiGroup.UiZones.FirstOrDefault(e => e.UserInterfaceScreenZone.UiZoneType == uiZone.UserInterfaceScreenZone.UiZoneType);

			if (null != uiGroup.UiZones.FirstOrDefault(e => e.UserInterfaceScreenZone.UiZoneType == uiZone.UserInterfaceScreenZone.UiZoneType))
			{
				runTimeOverlaidDrawService.RemoveDrawable(existingZone);
				uiGroup.UiZones.Remove(existingZone);
				existingZone.Dispose();
			}

			uiGroup.UiZones.Add(uiZone);
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
			{
				return;
			}

			var runTimeOverlaidDrawService = this._gameServices.GetService<IRuntimeOverlaidDrawService>();

			if (true == this.ActiveVisibilityGroupId.HasValue)
			{
				var activeGroup = this.UserInterfaceGroups.FirstOrDefault(e => e.VisibilityGroupId == this.ActiveVisibilityGroupId);

				foreach (var uiZoneContainer in activeGroup.UiZones)
				{
					runTimeOverlaidDrawService.RemoveDrawable(uiZoneContainer);
				}
			}

			this.ActiveVisibilityGroupId = uiGroup.VisibilityGroupId;

			if (0 == uiGroup.UiZones.Count)
			{
				return;
			}

			var animationService = this._gameServices.GetService<IAnimationService>();

			foreach (var uiZone in uiGroup.UiZones)
			{
				runTimeOverlaidDrawService.AddDrawable(uiZone);

				if (0 == uiZone.Components.Count)
				{
					continue;
				}

				foreach (var uiComponent in uiZone.Components)
				{
					if (uiComponent is UiBlock uiBlock)
					{
						if (0 == uiBlock.Rows.Count)
						{
							continue;
						}

						foreach (var uiRow in uiBlock.Rows)
						{
							if (0 == uiRow.Elements.Count)
							{
								continue;
							}

							foreach (var element in uiRow.Elements)
							{
								if ((element is UiButton button) &&
									(null != button?.ClickAnimation))
								{
									button.ClickAnimation.ResetTriggeredAnimation();
								}
							}
						}
					}
					else if (uiComponent is UiRow uiRow)
					{
						if (0 == uiRow.Elements.Count)
						{
							continue;
						}

						foreach (var element in uiRow.Elements)
						{
							if ((element is UiButton button) &&
								(null != button?.ClickAnimation))
							{
								button.ClickAnimation.ResetTriggeredAnimation();
							}
						}
					}
				}
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

			foreach (var uiZoneElementModel in uiGroupModel.UiZoneElements)
			{
				var uiZoneElement = this.GetUiZone(uiZoneElementModel);

				if (null != uiZoneElement)
				{
					uiZones.Add(uiZoneElement);
				}
			}

			var uiGroup = new UiGroup
			{
				UiGroupName = uiGroupModel.UiGroupName,
				VisibilityGroupId = uiGroupModel.VisibilityGroupId,
				UiZones = uiZones
			};

			this.UserInterfaceGroups.Add(uiGroup);

			if (true == uiGroupModel.IsVisible)
			{
				this.ToggleUserInterfaceGroupVisibility(uiGroup.VisibilityGroupId);
			}

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

			if (false == uiZoneService.UserInterfaceScreenZones.TryGetValue((UiScreenZoneType)uiZoneModel.UiZoneType, out UiScreenZone uiScreenZone))
			{
				uiScreenZone = uiZoneService.UserInterfaceScreenZones[UiScreenZoneType.Unknown];
			}

			var rows = new List<IAmAUiZoneChild>();

			foreach (var elementRowModel in uiZoneModel.ElementRows ?? [])
			{
				var row = this.GetUiRow(elementRowModel);
				rows.Add(row);
			}

			var contentHeight = rows.Sum(e => e.TotalHeight);
			var dynamicRows = rows
				.OfType<UiRow>()
				.Where(r => r.Elements.Any(e => DynamicSizedTypes.Contains(e.VerticalSizeType)))
				.ToArray();
			var remainingHeight = uiScreenZone.Area.Height - contentHeight;
			var dynamicHeight = remainingHeight / dynamicRows.Count();

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
				{
					background.SetDrawDimensions(uiScreenZone.Area.ToSubArea);
				}
			}

			Cursor hoverCursor = null;

			if ((false == string.IsNullOrEmpty(uiZoneModel.ZoneHoverCursorName)) &&
				(false == cursorService.Cursors.TryGetValue(uiZoneModel.ZoneHoverCursorName, out hoverCursor)))
			{
				// LOGGING
			}

			var cursorConfiguration = cursorInteractionService.GetCursorConfiguration<UiZone>(uiScreenZone.Area.ToSubArea, null);
			var uiZone = new UiZone
			{
				ResetCalculateCachedOffsets = true,
				UiZoneName = uiZoneModel.UiZoneName,
				DrawLayer = RunTimeConstants.BaseUiDrawLayer,
				VerticalJustificationType = (UiVerticalJustificationType)uiZoneModel.VerticalJustificationType,
				Graphic = background,
				HoverCursor = hoverCursor,
				CursorConfiguration = cursorConfiguration,
				UserInterfaceScreenZone = uiScreenZone,
				Components = rows
			};

			return uiZone;
		}

		/// <summary>
		/// Gets the user interface row.
		/// </summary>
		/// <param name="uiRowModel">The user interface row model.</param>
		/// <returns>The user interface row.</returns>
		public UiRow GetUiRow(UiRowModel uiRowModel)
		{
			var uiElementService = this._gameServices.GetService<IUserInterfaceElementService>();
			var imageService = this._gameServices.GetService<IImageService>();
			var cursorService = this._gameServices.GetService<ICursorService>();
			var cursorInteractionService = this._gameServices.GetService<ICursorInteractionService>();
			var uiZoneService = this._gameServices.GetService<IUserInterfaceScreenZoneService>();

			var zoneArea = uiZoneService.ScreenZoneSize;
			var subElements = new List<IAmAUiElement>();

			foreach (var subElementModel in uiRowModel.SubElements ?? [])
			{
				var subElement = uiElementService.GetUiElement(subElementModel);
				subElements.Add(subElement);
			}

			var contentWidth = subElements.Sum(e => e.TotalWidth);
			var contentHeight = subElements.Select(e => e.TotalHeight)
										   .OrderDescending()
										   .FirstOrDefault();
			var dynamicWidthElements = subElements.Where(e => UiElementSizeType.FlexMax == e.HorizontalSizeType)
											   .ToList();
			var remainingWidth = zoneArea.Width - contentWidth;
			var dynamicWidth = remainingWidth / dynamicWidthElements.Count;

			if (zoneArea.Width * ElementSizesScalars.ExtraSmall.X > dynamicWidth)
			{
				// LOGGING

				dynamicWidth = zoneArea.Width * ElementSizesScalars.ExtraSmall.X;
			}

			foreach (var dynamicWidthElement in dynamicWidthElements)
			{
				dynamicWidthElement.Area.Width = dynamicWidth;
			}

			var rowArea = new SubArea
			{
				Width = zoneArea.Width,
				Height = contentHeight
			};
			IAmAGraphic background = null;

			if (null != uiRowModel.BackgroundTexture)
			{
				background = imageService.GetImageFromModel(uiRowModel.BackgroundTexture);

				if ((true == uiRowModel.ResizeTexture) ||
					(background is CompositeImage))
				{
					var dimensions = new SubArea
					{
						Width = zoneArea.Width,
						Height = rowArea.Height
					};
					background.SetDrawDimensions(dimensions);
				}
			}

			var margin = uiElementService.GetUiMarginFromModel(uiRowModel.Margin);
			Cursor hoverCursor = null;

			if ((false == string.IsNullOrEmpty(uiRowModel.RowHoverCursorName)) &&
				(false == cursorService.Cursors.TryGetValue(uiRowModel.RowHoverCursorName, out hoverCursor)))
			{
				// LOGGING
			}

			var cursorConfiguration = cursorInteractionService.GetCursorConfiguration<UiRow>(rowArea, null);
			var result = new UiRow
			{
				UiRowName = uiRowModel.UiRowName,
				Flex = true,
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
		/// Updates the row dynamic height.
		/// </summary>
		/// <param name="uiRow">The user interface row.</param>
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
			{
				return;
			}

			var uiElementService = this._gameServices.GetService<IUserInterfaceElementService>();

			var dynamicHeightElements = uiRow.Elements.Where(e => true == DynamicSizedTypes.Contains(e.VerticalSizeType))
														 .ToList();

			foreach (var uiElement in dynamicHeightElements ?? [])
			{
				uiElementService.UpdateElementHeight(uiElement, dynamicHeight);
			}

			var contentHeight = uiRow.Elements.Select(e => e.TotalHeight)
												 .OrderDescending()
												 .FirstOrDefault();
			uiRow.Area.Height = contentHeight;
		}
	}
}

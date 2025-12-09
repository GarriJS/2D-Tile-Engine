using Common.Controls.CursorInteraction.Models;
using Common.Controls.CursorInteraction.Models.Contracts;
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
using Engine.Physics.Models;
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
		static public List<UiElementSizeType> DynamicSizedTypes { get; } =
		[
			UiElementSizeType.Flex
		];

		/// <summary>
		/// Gets the fixed sized types.
		/// </summary>
		static public List<UiElementSizeType> FixedSizedTypes { get; } =
		[
			UiElementSizeType.FitContent,
			UiElementSizeType.Fixed,
			UiElementSizeType.ExtraSmall,
			UiElementSizeType.Small,
			UiElementSizeType.Medium,
			UiElementSizeType.Large,
			UiElementSizeType.ExtraLarge
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

				if (0 == uiZone.ElementRows.Count)
				{
					continue;
				}

				foreach (var uiRow in uiZone.ElementRows)
				{
					if (0 == uiRow.SubElements.Count)
					{
						continue;
					}

					foreach (var element in uiRow.SubElements)
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

		/// <summary>
		/// Gets the user interface hover state at the screen location.
		/// </summary>
		/// <param name="location">The location.</param>
		/// <returns>The user interface hover state at the location if one is found.</returns>
		public HoverState GetUiObjectAtScreenLocation(Vector2 location)
		{
			if (true != this.ActiveVisibilityGroupId.HasValue)
			{
				return null;
			}

			var activeUiGroup = this.UserInterfaceGroups.FirstOrDefault(e => e.VisibilityGroupId == this.ActiveVisibilityGroupId);
			var uiZone = activeUiGroup.UiZones.FirstOrDefault(e => true == e.Area.Contains(location));

			if (null == uiZone)
			{
				return null;
			}

			var result = new HoverState
			{
				TopCursorConfiguration = uiZone.BaseCursorConfiguration,
				TopHoverCursor = uiZone.HoverCursor,
				HoverObjectLocation = new LocationExtender<IHaveACursorConfiguration>
				{
					Location = uiZone.Position.Coordinates,
					Element = uiZone
				}
			};

			if (0 == uiZone.ElementRows.Count)
			{
				return result;
			}

			var height = uiZone.ElementRows.Sum(e => e.TotalHeight);
			var rowVerticalOffset = uiZone.JustificationType switch
			{
				UiZoneJustificationTypes.Center => (uiZone.Area.Height - height) / 2,
				UiZoneJustificationTypes.Bottom => uiZone.Area.Height - height,
				_ => 0,
			};

			foreach (var elementRow in uiZone.ElementRows)
			{
				var rowTop = 0f;
				var rowBottom = 0f;
				rowTop = uiZone.Position.Y + rowVerticalOffset;
				rowBottom = rowTop + elementRow.TotalHeight;

				if ((rowTop > location.Y) ||
					(rowBottom < location.Y))
				{
					rowVerticalOffset += (rowBottom - rowTop);

					continue;
				}

				rowVerticalOffset += rowBottom;
				result.HoverObjectLocation = new LocationExtender<IHaveACursorConfiguration>
				{
					Location = uiZone.Position.Coordinates + new Vector2
					{
						X = 0,
						Y = rowTop
					},
					Element = elementRow
				};

				if (null != elementRow.CursorConfiguration)
				{
					result.TopCursorConfiguration = elementRow.CursorConfiguration;
				}

				if (null != elementRow.HoverCursor)
				{
					result.TopHoverCursor = elementRow.HoverCursor;
				}

				var uiElementWithLocation = this.GetUiElementAtScreenLocationInRow(uiZone.Position, elementRow, rowTop + elementRow.Margin.TopMargin, location);

				if (true == uiElementWithLocation.HasValue)
				{
					result.HoverObjectLocation = new LocationExtender<IHaveACursorConfiguration>
					{
						Location = uiElementWithLocation.Value.Location,
						Element = uiElementWithLocation.Value.Element
					};

					if (null != uiElementWithLocation.Value.Element.CursorConfiguration)
					{
						result.TopCursorConfiguration = uiElementWithLocation.Value.Element.CursorConfiguration;
					}

					if (null != uiElementWithLocation.Value.Element.HoverCursor)
					{
						result.TopHoverCursor = uiElementWithLocation.Value.Element.HoverCursor;
					}
				}

				return result;
			}

			return result;
		}

		/// <summary>
		/// Gets the user interface element at the screen location in the row.
		/// </summary>
		/// <param name="position">The position.</param>
		/// <param name="uiRow">The user interface row.</param>
		/// <param name="heightOffset">The height offset.</param>
		/// <param name="location">The location.</param>
		/// <returns>The user interface element at the location if one is found.</returns>
		private LocationExtender<IAmAUiElement>? GetUiElementAtScreenLocationInRow(Position position, UiRow uiRow, float heightOffset, Vector2 location)
		{
			var width = uiRow.SubElements.Sum(e => e.TotalWidth);
			var elementHorizontalOffset = uiRow.HorizontalJustificationType switch
			{
				UiRowHorizontalJustificationType.Center => (uiRow.TotalWidth - width) / 2,
				UiRowHorizontalJustificationType.Right => uiRow.TotalWidth - width,
				_ => 0,
			};

			var largestHeight = uiRow.SubElements.Select(e => e.TotalHeight)
												 .OrderDescending()
												 .FirstOrDefault();

			foreach (var element in uiRow.SubElements)
			{
				var verticallyCenterOffset = 0f;

				switch (uiRow.VerticalJustificationType)
				{
					case UiRowVerticalJustificationType.Bottom:
						verticallyCenterOffset = (largestHeight - element.TotalHeight);
						break;
					case UiRowVerticalJustificationType.Center:
						verticallyCenterOffset = (largestHeight - element.TotalHeight) / 2;
						break;
					case UiRowVerticalJustificationType.Top:
						break;
				}

				var elementRight = 0f;
				var elementLeft = 0f;

				switch (uiRow.HorizontalJustificationType)
				{
					case UiRowHorizontalJustificationType.Right:
					case UiRowHorizontalJustificationType.Center:
					case UiRowHorizontalJustificationType.Left:
					default:
						elementHorizontalOffset += element.Margin.LeftMargin;
						elementLeft = elementHorizontalOffset + position.X;
						elementHorizontalOffset += element.Area.Width;
						elementRight = elementHorizontalOffset + position.X;
						elementHorizontalOffset += element.Margin.RightMargin;
						break;
				}

				if ((elementLeft <= location.X) &&
					(elementRight >= location.X))
				{
					var elementTop = verticallyCenterOffset + heightOffset;
					var elementBottom = elementTop + element.Area.Height;

					if ((elementTop <= location.Y) &&
						(elementBottom >= location.Y))
					{
						var result = new LocationExtender<IAmAUiElement>
						{
							Element = element,
							Location = new Vector2
							{
								X = elementLeft,
								Y = elementTop
							}
						};

						return result;
					}
				}
			}

			return null;
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

			if (false == uiZoneService.UserInterfaceScreenZones.TryGetValue((UiScreenZoneTypes)uiZoneModel.UiZoneType, out UiScreenZone uiScreenZone))
			{
				uiScreenZone = uiZoneService.UserInterfaceScreenZones[UiScreenZoneTypes.Unknown];
			}

			var rows = new List<UiRow>();

			foreach (var elementRowModel in uiZoneModel.ElementRows ?? [])
			{
				var row = this.GetUiRow(elementRowModel);
				rows.Add(row);
			}

			var contentHeight = rows.Sum(e => e.TotalHeight);
			var dynamicRows = rows.Where(r => r.SubElements.Any(e => true == DynamicSizedTypes.Contains(e.VerticalSizeType)));
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
				JustificationType = (UiZoneJustificationTypes)uiZoneModel.JustificationType,
				Graphic = background,
				HoverCursor = hoverCursor,
				CursorConfiguration = cursorConfiguration,
				UserInterfaceScreenZone = uiScreenZone,
				ElementRows = rows
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
			var dynamicWidthElements = subElements.Where(e => UiElementSizeType.Flex == e.HorizontalSizeType)
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
							   + (uiRowModel.Margin?.TopMargin ?? 0)
							   + (uiRowModel.Margin?.BottomMargin ?? 0)
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
				SubElements = subElements
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

			if (0 == uiRow.SubElements.Count)
			{
				return;
			}

			var uiElementService = this._gameServices.GetService<IUserInterfaceElementService>();

			var dynamicHeightElements = uiRow.SubElements.Where(e => true == DynamicSizedTypes.Contains(e.VerticalSizeType))
														 .ToList();

			foreach (var uiElement in dynamicHeightElements ?? [])
			{
				uiElementService.UpdateElementHeight(uiElement, dynamicHeight);
			}

			var contentHeight = uiRow.SubElements.Select(e => e.TotalHeight)
												 .OrderDescending()
												 .FirstOrDefault();
			uiRow.Area.Height = contentHeight;
		}
	}
}

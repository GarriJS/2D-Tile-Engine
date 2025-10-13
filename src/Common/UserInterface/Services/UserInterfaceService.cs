using Common.Controls.CursorInteraction.Models;
using Common.Controls.CursorInteraction.Models.Abstract;
using Common.Controls.CursorInteraction.Models.Contracts;
using Common.Controls.CursorInteraction.Services.Contracts;
using Common.Core.Constants;
using Common.DiskModels.UI;
using Common.UserInterface.Constants;
using Common.UserInterface.Enums;
using Common.UserInterface.Models;
using Common.UserInterface.Models.Contracts;
using Common.UserInterface.Models.Elements;
using Common.UserInterface.Services.Contracts;
using Engine.Core.Initialization.Contracts;
using Engine.Graphics.Models;
using Engine.Graphics.Services.Contracts;
using Engine.Physics.Models;
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

			BaseHoverConfiguration topHoverCursorConfiguration = uiZone.HoverConfig;

			if (0 == uiZone.ElementRows.Count)
			{
				return new HoverState
				{
					TopHoverCursorConfiguration = topHoverCursorConfiguration,
					HoverObjectLocation = new LocationExtender<IHaveAHoverConfiguration>
					{
						Location = uiZone.Position.Coordinates,
						Object = uiZone
					}
				};
			}

			var height = uiZone.ElementRows.Sum(e => e.InsideHeight);
			var rowVerticalOffset = uiZone.JustificationType switch
			{
				UiZoneJustificationTypes.None => 0,
				UiZoneJustificationTypes.Center => (uiZone.Area.Height - height) / 2,
				UiZoneJustificationTypes.Top => 0,
				UiZoneJustificationTypes.Bottom => uiZone.Area.Height - height,
				_ => 0,
			};

			foreach (var elementRow in uiZone.ElementRows)
			{
				var rowTop = 0f;
				var rowBottom = 0f;

				rowTop = uiZone.Position.Y + rowVerticalOffset;
				rowBottom = rowTop + elementRow.InsideHeight;

				if ((rowTop > location.Y) ||
					(rowBottom < location.Y))
				{
					continue;
				}

				rowVerticalOffset += rowBottom;

				if (null != elementRow.BaseHoverConfig?.HoverCursor)
				{
					topHoverCursorConfiguration = elementRow.BaseHoverConfig;
				}

				var uiElementWithLocation = this.GetUiElementAtScreenLocationInRow(uiZone.Position, elementRow, rowTop + elementRow.InsidePadding.TopPadding, location);

				if (true == uiElementWithLocation.HasValue)
				{
					if (null != uiElementWithLocation.Value.Object.BaseHoverConfig?.HoverCursor)
					{
						topHoverCursorConfiguration = uiElementWithLocation.Value.Object.BaseHoverConfig;
					}

					return new HoverState
					{
						TopHoverCursorConfiguration = topHoverCursorConfiguration,
						HoverObjectLocation = new LocationExtender<IHaveAHoverConfiguration>
						{
							Location = uiElementWithLocation.Value.Location,
							Object = uiElementWithLocation.Value.Object
						}
					};
				}

				return new HoverState
				{
					TopHoverCursorConfiguration = topHoverCursorConfiguration,
					HoverObjectLocation = new LocationExtender<IHaveAHoverConfiguration>
					{
						Location = uiZone.Position.Coordinates + new Vector2
						{
							X = 0,
							Y = rowTop
						},
						Object = elementRow
					}
				};
			}

			return new HoverState
			{
				TopHoverCursorConfiguration = topHoverCursorConfiguration,
				HoverObjectLocation = new LocationExtender<IHaveAHoverConfiguration>
				{
					Location = uiZone.Position.Coordinates,
					Object = uiZone
				}
			};
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
			var width = uiRow.SubElements.Sum(e => e.InsideWidth);
			var elementHorizontalOffset = uiRow.HorizontalJustificationType switch
			{
				UiRowHorizontalJustificationTypes.None => 0,
				UiRowHorizontalJustificationTypes.Center => (uiRow.InsideWidth - width) / 2,
				UiRowHorizontalJustificationTypes.Left => 0,
				UiRowHorizontalJustificationTypes.Right => uiRow.InsideWidth - width,
				_ => 0,
			};

			var largestHeight = uiRow.SubElements.Select(e => e.InsideHeight)
												 .OrderDescending()
												 .FirstOrDefault();

			foreach (var element in uiRow.SubElements)
			{
				var verticallyCenterOffset = 0f;

				switch (uiRow.VerticalJustificationType)
				{
					case UiRowVerticalJustificationTypes.Bottom:
						verticallyCenterOffset = (largestHeight - element.Area.Height);
						break;
					case UiRowVerticalJustificationTypes.Center:
						verticallyCenterOffset = (largestHeight - element.Area.Height) / 2;
						break;
					case UiRowVerticalJustificationTypes.None:
					case UiRowVerticalJustificationTypes.Top:
						break;
				}

				var elementRight = 0f;
				var elementLeft = 0f;

				switch (uiRow.HorizontalJustificationType)
				{
					case UiRowHorizontalJustificationTypes.Right:
					case UiRowHorizontalJustificationTypes.Center:
					case UiRowHorizontalJustificationTypes.None:
					case UiRowHorizontalJustificationTypes.Left:
					default:
						elementHorizontalOffset += element.InsidePadding.LeftPadding;
						elementLeft = elementHorizontalOffset + position.X;
						elementHorizontalOffset += element.Area.Width;
						elementRight = elementHorizontalOffset + position.X;
						elementHorizontalOffset += element.InsidePadding.RightPadding;
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
						return new LocationExtender<IAmAUiElement>
						{
							Object = element,
							Location = new Vector2
							{
								X = elementLeft,
								Y = elementTop
							}
						};
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
			var cursorInteractionService = this._gameServices.GetService<ICursorInteractionService>();

			if (false == uiZoneService.UserInterfaceScreenZones.TryGetValue((UiScreenZoneTypes)uiZoneModel.UiZoneType, out UiScreenZone uiScreenZone))
			{
				uiScreenZone = uiZoneService.UserInterfaceScreenZones[UiScreenZoneTypes.None];
			}

			Image background = null;

			if (null != uiZoneModel.BackgroundTexture)
			{
				background = imageService.GetImageFromModel(uiZoneModel.BackgroundTexture);

				if ((true == uiZoneModel.ResizeTexture) ||
					(background is FillImage))
				{
					background.SetDrawDimensions(uiScreenZone.Area.ToVectorDimensions);
				}
			}

			var hoverConfig = cursorInteractionService.GetHoverConfiguration<UiZone>(uiScreenZone.Area.ToSubArea, uiZoneModel.ZoneHoverCursorName);
			var uiZone = new UiZone
			{
				UiZoneName = uiZoneModel.UiZoneName,
				DrawLayer = RunTimeConstants.BaseUiDrawLayer,
				JustificationType = (UiZoneJustificationTypes)uiZoneModel.JustificationType,
				Image = background,
				HoverConfig = hoverConfig,
				UserInterfaceScreenZone = uiScreenZone,
				ElementRows = []
			};

			// LOGGING
			if (true == functionService.TryGetFunction<Action<UiZone, Vector2>>(uiZoneModel.ZoneHoverEventName, out var hoverAction))
			{
				uiZone.HoverConfig?.AddSubscription(hoverAction);
			}

			if (0 == uiZoneModel.ElementRows.Length)
			{
				return uiZone;
			}

			var elementRowsFirstPass = new List<UiRow>();

			foreach (var elementRowModel in uiZoneModel.ElementRows)
			{
				var elementRow = this.GetUiRow(elementRowModel, uiScreenZone);

				if (null != elementRow)
				{
					elementRowsFirstPass.Add(elementRow);
				}
			}

			var currentTotalHeight = elementRowsFirstPass.Sum(e => e.InsideHeight);
			var elementRowsSecondPass = new List<UiRow>();

			foreach (var elementRow in elementRowsFirstPass)
			{
				var rowRealWidth = elementRow.SubElements.Sum(e => e.InsideWidth);

				if ((true == elementRow.Flex) &&
					(currentTotalHeight < uiScreenZone.Area.Height) &&
					(elementRow.InsideWidth < rowRealWidth))
				{
					var flexedElementRows = GetFlexedUiRows(elementRow);
					var newHeight = (currentTotalHeight - elementRow.InsideHeight) + flexedElementRows.Sum(e => e.InsideHeight);

					if (newHeight < uiScreenZone.Area.Height)
					{
						currentTotalHeight = newHeight;
						elementRowsSecondPass.AddRange(flexedElementRows?.Where(e => null != e));

						continue;
					}
				}

				elementRowsSecondPass.Add(elementRow);
			}

			var fillRows = elementRowsSecondPass.Where(e => true == e.SubElements.Any(s => s.Area.Height == 0))
												.ToList();

			if (0 < fillRows.Count)
			{
				var fillHeight = (uiScreenZone.Area.Height - currentTotalHeight) / fillRows.Count();

				if (fillHeight < ElementSizesScalars.ExtraSmall.Y / 2)
				{
					fillHeight = ElementSizesScalars.ExtraSmall.Y / 2;
				}

				var fillElements = fillRows.SelectMany(e => e.SubElements)
										   .Where(e => e.Area.Height == 0);

				foreach (var fillElement in fillElements)
				{
					uiElementService.UpdateElementHeight(fillElement, fillHeight);
				}

				foreach (var fillRow in fillRows)
				{
					fillRow.InsideHeight = fillRow.SubElements.Select(e => e.InsideHeight)
															  .OrderDescending()
															  .FirstOrDefault();
				}
			}

			uiZone.ElementRows = elementRowsSecondPass;

			return uiZone;
		}

		/// <summary>
		/// Gets the user interface row.
		/// </summary>
		/// <param name="uiRowModel">The user interface row model.</param>
		/// <param name="uiZone">The user interface zone.</param>
		/// <returns>The user interface row.</returns>
		public UiRow GetUiRow(UiRowModel uiRowModel, UiScreenZone uiZone)
		{
			var uiElementService = this._gameServices.GetService<IUserInterfaceElementService>();
			var imageService = this._gameServices.GetService<IImageService>();
			var cursorInteractionService = this._gameServices.GetService<ICursorInteractionService>();

			var numberOfFillElements = 0;
			var subElements = new List<IAmAUiElement>();

			if (0 == uiRowModel.SubElements.Length)
			{
				return null;
			}

			var availableWidth = uiZone.Area.Width;

			foreach (var elementModel in uiRowModel.SubElements)
			{
				var elementMinWidth = elementModel.InsidePadding.LeftPadding + elementModel.InsidePadding.RightPadding;
				var elementSize = uiElementService.GetElementDimensions(uiZone, elementModel);

				if (0 < elementSize.X)
				{
					elementMinWidth += elementSize.X;
				}
				else
				{
					numberOfFillElements++;
				}

				availableWidth -= elementMinWidth;
			}

			var fillWidth = 0 < numberOfFillElements
				? availableWidth / numberOfFillElements
				: availableWidth / uiRowModel.SubElements.Length;

			foreach (var elementModel in uiRowModel.SubElements)
			{
				var element = uiElementService.GetUiElement(elementModel, uiZone, fillWidth);

				if (null != element)
				{
					subElements.Add(element);
				}
			}

			var height = subElements.Where(e => null != e)
									.Select(e => e.Area.Height)
									.OrderDescending()
									.FirstOrDefault();

			Image background = null;

			if (null != uiRowModel.BackgroundTexture)
			{
				background = imageService.GetImageFromModel(uiRowModel.BackgroundTexture);

				if ((true == uiRowModel.ResizeTexture) ||
					(background is FillImage))
				{
					var dimensions = new Vector2
					{
						X = uiZone.Area.Width,
						Y = height + uiRowModel.InsidePadding.TopPadding + uiRowModel.InsidePadding.BottomPadding
					};
					background.SetDrawDimensions(dimensions);
				}
			}

			var rowArea = new SubArea
			{
				Width = uiZone.Area.Width,
				Height = height
			};
			var hoverConfig = cursorInteractionService.GetHoverConfiguration<UiRow>(rowArea, uiRowModel.RowHoverCursorName);
			var area = new SubArea
			{
				Width = uiZone.Area.Width,
				Height = height,
			};
			var insidePadding = uiElementService.GetUiPaddingFromModel(uiRowModel.InsidePadding);

			return new UiRow
			{
				UiRowName = uiRowModel.UiRowName,
				Flex = true,
				Area = area,
				InsidePadding = insidePadding,
				HorizontalJustificationType = (UiRowHorizontalJustificationTypes)uiRowModel.HorizontalJustificationType,
				VerticalJustificationType = (UiRowVerticalJustificationTypes)uiRowModel.VerticalJustificationType,
				Image = background,
				HoverConfig = hoverConfig,
				SubElements = subElements
			};
		}

		/// <summary>
		/// Gets the flexed user interface rows.
		/// </summary>
		/// <param name="uiRow">The user interface rows.</param>
		/// <returns>The flexed user interface rows.</returns>
		private static List<UiRow> GetFlexedUiRows(UiRow uiRow)
		{
			if (0 == uiRow.SubElements.Count)
			{
				return [];
			}

			var flexedRows = new List<UiRow>();
			var currentWidth = 0f;
			var currentRow = new UiRow
			{
				UiRowName = uiRow.UiRowName,
				Flex = true,
				Area = new SubArea
				{
					Width = uiRow.InsideWidth,
					Height = 0
				},
				InsidePadding = new UiPadding
				{
					TopPadding = uiRow.InsidePadding.TopPadding,
					BottomPadding = 1
				},
				HorizontalJustificationType = uiRow.HorizontalJustificationType,
				VerticalJustificationType = uiRow.VerticalJustificationType,
				Image = uiRow.Image,
				SubElements = []
			};

			foreach (var element in uiRow.SubElements)
			{
				var elementWidth = element.InsideWidth;

				if (currentWidth + elementWidth > uiRow.InsideWidth)
				{
					currentRow.InsideHeight = currentRow.SubElements.Select(e => e.Area.Height)
																	.OrderDescending()
																	.FirstOrDefault();
					
					currentWidth = 0;
					flexedRows.Add(currentRow);
					currentRow = new UiRow
					{
						UiRowName = uiRow.UiRowName,
						Flex = true,
						Area = new SubArea
						{
							Width = uiRow.InsideWidth,
							Height = 0
						},
						InsidePadding = new UiPadding
						{
							TopPadding = uiRow.InsidePadding.TopPadding,
							BottomPadding = 1
						},
						HorizontalJustificationType = uiRow.HorizontalJustificationType,
						VerticalJustificationType = uiRow.VerticalJustificationType,
						Image = uiRow.Image,
						SubElements = []
					};
				}

				currentRow.SubElements.Add(element);
				currentWidth += elementWidth;
			}

			currentRow.InsidePadding.BottomPadding = uiRow.InsidePadding.BottomPadding;

			if (0 < currentRow.SubElements.Count)
			{
				currentRow.Area.Height = currentRow.SubElements.Select(e => e.Area.Height)
															   .OrderDescending()
														       .FirstOrDefault();

				flexedRows.Add(currentRow);
			}

			return flexedRows;
		}
	}
}

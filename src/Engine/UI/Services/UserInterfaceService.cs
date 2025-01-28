using Engine.DiskModels.UI;
using Engine.Drawing.Services.Contracts;
using Engine.Physics.Models;
using Engine.RunTime.Services.Contracts;
using Engine.UI.Models;
using Engine.UI.Models.Contracts;
using Engine.UI.Models.Elements;
using Engine.UI.Models.Enums;
using Engine.UI.Services.Contracts;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Engine.UI.Services
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

			var runtimeDrawService = this._gameServices.GetService<IRuntimeDrawService>();

			if (true == this.ActiveVisibilityGroupId.HasValue)
			{
				var activeGroup = this.UserInterfaceGroups.FirstOrDefault(e => e.VisibilityGroupId == this.ActiveVisibilityGroupId);

				foreach (var uiZoneContainer in activeGroup.UiZones)
				{
					runtimeDrawService.RemoveOverlaidDrawable(uiZoneContainer.DrawLayer, uiZoneContainer);
				}
			}

			if (true == uiGroup.UiZones?.Any())
			{
				var animationService = this._gameServices.GetService<IAnimationService>();

				foreach (var uiZoneContainer in uiGroup.UiZones)
				{
					runtimeDrawService.AddOverlaidDrawable(uiZoneContainer.DrawLayer, uiZoneContainer);

					if (true != uiZoneContainer.ElementRows?.Any())
					{
						continue;
					}

					foreach (var uiRow in uiZoneContainer.ElementRows)
					{
						if (true != uiRow.SubElements?.Any())
						{
							continue;
						}

						foreach (var element in uiRow.SubElements)
						{
							if ((element is UiButton button) &&
								(null != button?.ClickAnimation))
							{
								animationService.ResetTriggeredAnimation(button.ClickAnimation);
							}
						}
					}
				}
			}

			this.ActiveVisibilityGroupId = uiGroup.VisibilityGroupId;
		}

		/// <summary>
		/// Gets the user interface element at the screen location.
		/// </summary>
		/// <param name="location">The location.</param>
		/// <returns>The user interface element at the location if one is found.</returns>
		public UiElementWithLocation GetUiElementAtScreenLocation(Vector2 location)
		{
			if (true != this.ActiveVisibilityGroupId.HasValue)
			{
				return null;
			}

			var activeUiGroup = this.UserInterfaceGroups.FirstOrDefault(e => e.VisibilityGroupId == this.ActiveVisibilityGroupId);
			var uiZone = activeUiGroup.UiZones.FirstOrDefault(e => e.Area.Contains(location));

			if ((null == uiZone) ||
				(true != uiZone.ElementRows.Any()))
			{
				return null;
			}

			var height = uiZone.ElementRows.Sum(e => e.Height + e.BottomPadding + e.TopPadding);
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

				switch (uiZone.JustificationType)
				{
					case UiZoneJustificationTypes.Bottom:
					case UiZoneJustificationTypes.Center:
					case UiZoneJustificationTypes.None:
					case UiZoneJustificationTypes.Top:
					default:
						rowVerticalOffset += elementRow.TopPadding;
						rowTop = rowVerticalOffset + uiZone.Position.Y;
						rowVerticalOffset += elementRow.Height;
						rowBottom = rowVerticalOffset + uiZone.Position.Y;
						rowVerticalOffset += elementRow.BottomPadding;
						break;
				}

				if ((rowTop <= location.Y) &&
					(rowBottom >= location.Y))
				{
					return this.GetUiElementAtScreenLocationInRow(uiZone.Position, elementRow, rowTop, location);
				}
			}

			return null;
		}

		/// <summary>
		/// Gets the user interface element at the screen location in the row.
		/// </summary>
		/// <param name="position">The position.</param>
		/// <param name="uiRow">The user interface row.</param>
		/// <param name="heightOffset">The height offset.</param>
		/// <param name="location">The location.</param>
		/// <returns>The user interface element at the location if one is found.</returns>
		private UiElementWithLocation GetUiElementAtScreenLocationInRow(Position position, UiRow uiRow, float heightOffset, Vector2 location)
		{
			var width = uiRow.SubElements.Sum(e => e.Area.X + e.LeftPadding + e.RightPadding);
			var elementHorizontalOffset = uiRow.HorizontalJustificationType switch
			{
				UiRowHorizontalJustificationTypes.None => 0,
				UiRowHorizontalJustificationTypes.Center => (uiRow.Width - width) / 2,
				UiRowHorizontalJustificationTypes.Left => 0,
				UiRowHorizontalJustificationTypes.Right => uiRow.Width - width,
				_ => 0,
			};

			var largestHeight = uiRow.SubElements.OrderByDescending(e => e.Area.Y)
												 .FirstOrDefault().Area.Y;

			foreach (var element in uiRow.SubElements)
			{
				var verticallyCenterOffset = 0f;

				switch (uiRow.VerticalJustificationType)
				{
					case UiRowVerticalJustificationTypes.Bottom:
						verticallyCenterOffset = (largestHeight - element.Area.Y);
						break;
					case UiRowVerticalJustificationTypes.Center:
						verticallyCenterOffset = (largestHeight - element.Area.Y) / 2;
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
						elementHorizontalOffset += element.LeftPadding;
						elementLeft = elementHorizontalOffset + position.X;
						elementHorizontalOffset += element.Area.X;
						elementRight = elementHorizontalOffset + position.X;
						elementHorizontalOffset += element.RightPadding;
						break;
				}

				if ((elementLeft <= location.X) &&
					(elementRight >= location.X))
				{
					var elementTop = verticallyCenterOffset + heightOffset;
					var elementBottom = elementTop + element.Area.Y;

					if ((elementTop <= location.Y) &&
						(elementBottom >= location.Y))
					{
						return new UiElementWithLocation
						{
							Element = element,
							Location = new Vector2(elementLeft, elementTop),
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
				var uiZoneElement = this.GetUiZoneElement(uiZoneElementModel, uiGroupModel.VisibilityGroupId);

				if (null != uiZoneElement)
				{
					uiZones.Add(uiZoneElement);
				}
			}

			return new UiGroup
			{
				UiGroupName = uiGroupModel.UiGroupName,
				VisibilityGroupId = uiGroupModel.VisibilityGroupId,
				UiZones = uiZones
			};
		}

		/// <summary>
		/// Gets the user interface zone element.
		/// </summary>
		/// <param name="uiZoneElementModel">The user interface element model.</param>
		/// <param name="visibilityGroup">The visibility group of the user interface zone.</param>
		/// <returns>The user interface zone element.</returns>
		public UiZone GetUiZoneElement(UiZoneModel uiZoneElementModel, int visibilityGroup)
		{
			var uiZoneService = this._gameServices.GetService<IUserInterfaceScreenZoneService>();
			var uiElementService = this._gameServices.GetService<IUserInterfaceElementService>();
			var elementRows = new List<UiRow>();

			if (false == uiZoneService.UserInterfaceScreenZones.TryGetValue((UiScreenZoneTypes)uiZoneElementModel.UiZoneType, out UiScreenZone uiZone))
			{
				uiZone = uiZoneService.UserInterfaceScreenZones[UiScreenZoneTypes.None];
			}

			var imageService = this._gameServices.GetService<IImageService>();
			var background = imageService.GetImage(uiZoneElementModel.BackgroundTextureName, (int)uiZone.Area.Width, (int)uiZone.Area.Height);

			if (true == uiZoneElementModel.ElementRows?.Any())
			{
				var availableHeight = uiZone.Area.Height;
				var numberOfFillRows = 0;
				var rowElementBaseHeights = new Dictionary<UiRowModel, float>();

				foreach (var elementRowModel in uiZoneElementModel.ElementRows)
				{
					var rowMinHeight = 0f;

					if (true == elementRowModel.SubElements?.Any())
					{
						var rowElementsSizes = elementRowModel.SubElements.Select(e => uiElementService.GetElementDimensions(uiZone, e));

						if (true == rowElementsSizes?.Any(e => false == e.HasValue))
						{
							numberOfFillRows++;
						}

						rowMinHeight += rowElementsSizes.Where(e => true == e.HasValue)
														.Select(e => e.Value.Y)
														.OrderDescending()
														.FirstOrDefault();

						rowElementBaseHeights.Add(elementRowModel, rowMinHeight);
					}

					availableHeight -= (rowMinHeight + elementRowModel.TopPadding + elementRowModel.BottomPadding);
				}

				var fillHeight = 0 < numberOfFillRows
					? availableHeight / numberOfFillRows
					: availableHeight / uiZoneElementModel.ElementRows.Length;

				foreach (var elementRowModel in uiZoneElementModel.ElementRows)
				{
					rowElementBaseHeights.TryGetValue(elementRowModel, out var rowBaseHeight);
					var rowHeight = rowBaseHeight;

					if (true == elementRowModel.SubElements?.Any(e => ((int)UiElementSizeTypes.None == e.SizeType) ||
																	 ((int)UiElementSizeTypes.Fill == e.SizeType)))
					{
						rowHeight += fillHeight;
					}

					var elementRow = this.GetUiRow(elementRowModel, uiZone, rowHeight, visibilityGroup);

					if (null != elementRow)
					{
						elementRows.Add(elementRow);
					}
				}
			}

			return new UiZone
			{
				UiZoneName = uiZoneElementModel.UiZoneName,
				DrawLayer = 1,
				JustificationType = (UiZoneJustificationTypes)uiZoneElementModel.JustificationType,
				Image = background,
				UserInterfaceScreenZone = uiZone,
				ElementRows = elementRows
			};
		}

		/// <summary>
		/// Gets the user interface row.
		/// </summary>
		/// <param name="uiRowModel">The user interface row model.</param>
		/// <param name="uiZone">The user interface zone.</param>
		/// <param name="fillHeight">The fill height.</param>
		/// <param name="visibilityGroup">The visibility group of the user interface row.</param>
		/// <returns>The user interface row.</returns>
		public UiRow GetUiRow(UiRowModel uiRowModel, UiScreenZone uiZone, float fillHeight, int visibilityGroup)
		{
			var uiElementService = this._gameServices.GetService<IUserInterfaceElementService>();
			var imageService = this._gameServices.GetService<IImageService>();
			var numberOfFillElements = 0;
			var subElements = new List<IAmAUiElement>();

			if (true == uiRowModel.SubElements?.Any())
			{
				var availableWidth = uiZone.Area.Width;

				foreach (var elementModel in uiRowModel.SubElements)
				{
					var elementMinWidth = elementModel.LeftPadding + elementModel.RightPadding;
					var elementSize = uiElementService.GetElementDimensions(uiZone, elementModel);

					if (true == elementSize.HasValue)
					{
						elementMinWidth += elementSize.Value.X;
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
					var element = uiElementService.GetUiElement(elementModel, uiZone, fillWidth, fillHeight, visibilityGroup);

					if (null != element)
					{
						subElements.Add(element);
					}
				}
			}

			var height = subElements.Where(e => null != e)
									.Select(e => e.Area.Y)
									.OrderDescending()
									.FirstOrDefault();
			var image = imageService.GetImage(uiRowModel.BackgroundTextureName, (int)uiZone.Area.Width, (int)height + uiRowModel.TopPadding + uiRowModel.BottomPadding);

			return new UiRow
			{
				UiRowName = uiRowModel.UiRowName,
				Width = uiZone.Area.Width,
				Height = height,
				TopPadding = uiRowModel.TopPadding,
				BottomPadding = uiRowModel.BottomPadding,
				HorizontalJustificationType = (UiRowHorizontalJustificationTypes)uiRowModel.HorizontalJustificationType,
				VerticalJustificationType = (UiRowVerticalJustificationTypes)uiRowModel.VerticalJustificationType,
				Image = image,
				SubElements = subElements
			};
		}
	}
}

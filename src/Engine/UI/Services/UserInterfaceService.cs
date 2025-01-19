using DiscModels.Engine.UI;
using Engine.Drawing.Services.Contracts;
using Engine.RunTime.Services.Contracts;
using Engine.UI.Models;
using Engine.UI.Models.Contracts;
using Engine.UI.Models.Enums;
using Engine.UI.Services.Contracts;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.UI.Services
{
	/// <summary>
	/// Represents a user interface service.
	/// </summary>
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

				foreach (var uiZoneContainer in activeGroup.UiZoneElements)
				{
					runtimeDrawService.RemoveOverlaidDrawable(uiZoneContainer.DrawLayer, uiZoneContainer);
				}
			}

			foreach (var uiZoneContainer in uiGroup.UiZoneElements)
			{
				runtimeDrawService.AddOverlaidDrawable(uiZoneContainer.DrawLayer, uiZoneContainer);
			}

			this.ActiveVisibilityGroupId = uiGroup.VisibilityGroupId;
		}

		/// <summary>
		/// Gets the user interface group.
		/// </summary>
		/// <param name="uiGroupModel">The user interface group model.</param>
		/// <returns>The user interface group.</returns>
		public UiGroup GetUiGroup(UiGroupModel uiGroupModel)
		{
			var uiZoneElements = new List<UiZone>();

			foreach (var uiZoneElementModel in uiGroupModel.UiZoneElements)
			{
				var uiZoneElement = this.GetUiZoneElement(uiZoneElementModel);

				if (null != uiZoneElement)
				{
					uiZoneElements.Add(uiZoneElement);
				}
			}

			return new UiGroup
			{
				UiGroupName = uiGroupModel.UiGroupName,
				VisibilityGroupId = uiGroupModel.VisibilityGroupId,
				UiZoneElements = uiZoneElements,
				ActiveSignalSubscriptions = []
			};
		}

		/// <summary>
		/// Gets the user interface zone element.
		/// </summary>
		/// <param name="uiZoneElementModel">The user interface element model.</param>
		/// <returns>The user interface zone element.</returns>
		public UiZone GetUiZoneElement(UiZoneModel uiZoneElementModel)
		{
			var uiZoneService = this._gameServices.GetService<IUserInterfaceScreenZoneService>();
			var uiElementService = this._gameServices.GetService<IUserInterfaceElementService>();
			var elementRows = new List<UiRow>();

			if (false == uiZoneService.UserInterfaceScreenZones.TryGetValue((UiScreenZoneTypes)uiZoneElementModel.UiZoneType, out UiScreenZone uiZone))
			{
				uiZone = uiZoneService.UserInterfaceScreenZones[UiScreenZoneTypes.None];
			}

			var imageService = this._gameServices.GetService<IImageService>();
			var background = imageService.GetImage(uiZoneElementModel.Background, (int)uiZone.Area.Width, (int)uiZone.Area.Height);

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
						var rowElementsSizes = elementRowModel.SubElements.Select(
							e => uiElementService.GetElementDimensions(uiZone,
									true == Enum.IsDefined(typeof(UiElementSizeTypes), e.SizeType)
									? (UiElementSizeTypes)e.SizeType
									: UiElementSizeTypes.None)
						);

						if (true == rowElementsSizes?.Any(e => false == e.HasValue))
						{
							numberOfFillRows++;
						}

						rowMinHeight += rowElementsSizes.Where(e => true == e.HasValue)
														.Select(e => e.Value.Y)
														.OrderDescending()
														.First();

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

					var elementRow = this.GetUiRow(elementRowModel, uiZone, rowHeight);

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
		/// <returns>The user interface row.</returns>
		public UiRow GetUiRow(UiRowModel uiRowModel, UiScreenZone uiZone, float fillHeight)
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
					var elementSize = uiElementService.GetElementDimensions(uiZone,
										true == Enum.IsDefined(typeof(UiElementSizeTypes), elementModel.SizeType)
										? (UiElementSizeTypes)elementModel.SizeType
										: UiElementSizeTypes.None);

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
					var element = uiElementService.GetUiElement(elementModel, uiZone, fillWidth, fillHeight);

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
				JustificationType = (UiRowJustificationTypes)uiRowModel.JustificationType,
				Image = image,
				SubElements = subElements
			};
		}
	}
}

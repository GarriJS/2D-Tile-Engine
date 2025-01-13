using DiscModels.Engine.UI;
using Engine.Drawing.Services.Contracts;
using Engine.RunTime.Services.Contracts;
using Engine.UI.Models;
using Engine.UI.Models.Contracts;
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
			var uiZoneElements = new List<UiZoneElement>();

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
		public UiZoneElement GetUiZoneElement(UiZoneElementModel uiZoneElementModel)
		{
			var uiZoneService = this._gameServices.GetService<IUserInterfaceZoneService>();
			var elementRows = new List<UiRow>();

			if (false == uiZoneService.UserInterfaceZones.TryGetValue((UiZoneTypes)uiZoneElementModel.UiZoneType, out UiZone uiZone))
			{
				uiZone = uiZoneService.UserInterfaceZones[UiZoneTypes.None];
			}

			var imageService = this._gameServices.GetService<IImageService>();
			var background = imageService.GetImage(uiZoneElementModel.Background, (int)uiZone.Area.Width, (int)uiZone.Area.Height);

			if (true == uiZoneElementModel.ElementRows?.Any())
			{
				var totalRowPadding = uiZoneElementModel.ElementRows.Sum(e => e.TopPadding + e.BottomPadding);
				var rowHeight = (uiZone.Area.Height - totalRowPadding) / uiZoneElementModel.ElementRows.Length;

				foreach (var elementRowModel in uiZoneElementModel.ElementRows)
				{
					var elementRow = this.GetUiRow(elementRowModel, uiZone, rowHeight);

					if (null != elementRow)
					{
						elementRows.Add(elementRow);
					}
				}
			}

			return new UiZoneElement
			{
				UiZoneElementName = uiZoneElementModel.UiZoneElementName,
				DrawLayer = 1,
				JustificationType = (UiZoneElementJustificationTypes)uiZoneElementModel.JustificationType,
				Image = background,
				UserInterfaceZone = uiZone,
				ElementRows = elementRows
			};
		}

		/// <summary>
		/// Gets the user interface row.
		/// </summary>
		/// <param name="uiRowModel">The user interface row model.</param>
		/// <param name="uiZone">The user interface zone.</param>
		/// <returns>The user interface row.</returns>
		public UiRow GetUiRow(UiRowModel uiRowModel, UiZone uiZone, float height)
		{
			var uiElementService = this._gameServices.GetService<IUserInterfaceElementService>();
			var imageService = this._gameServices.GetService<IImageService>();
			var subElements = new List<IAmAUiElement>();
			var width = uiZone.Area.Width;

			if (true == uiRowModel.SubElements?.Any())
			{
				foreach (var elementRowModel in uiRowModel.SubElements)
				{
					width -= (elementRowModel.RightPadding + elementRowModel.LeftPadding);
				}

				var elementWidth = width / uiRowModel.SubElements.Length;

				foreach (var elementRowModel in uiRowModel.SubElements)
				{
					var uiElement = uiElementService.GetUiElement(elementRowModel, elementWidth, height);

					if (uiElement != null)
					{
						subElements.Add(uiElement);
					}
				}
			}

			var image = imageService.GetImage(uiRowModel.BackgroundTextureName, (int)uiZone.Area.Width, (int)height);

			return new UiRow
			{
				UiRowName = uiRowModel.UiRowName,
				Width = width,
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

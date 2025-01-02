using DiscModels.Engine.UI;
using DiscModels.Engine.UI.Contracts;
using Engine.Drawing.Services.Contracts;
using Engine.Physics.Models;
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
		/// Gets or sets the user interface elements.
		/// </summary>
		private List<IAmAUiElement> UserInterfaceElements { get; set; } = [];

		/// <summary>
		/// Gets or sets the user interface groups.
		/// </summary>
		private List<UiGroup> UserInterfaceGroups { get; set; } = [];

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
			if (null != uiGroup)
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
		/// Gets the user interface zone element.
		/// </summary>
		/// <param name="uiZoneElementModel">The user interface element model.</param>
		/// <returns>The user interface zone element.</returns>
		public UiZoneElement GetUiZoneElement(UiZoneElementModel uiZoneElementModel)
		{
			var spriteService = this._gameServices.GetService<ISpriteService>();
			var uiZoneService = this._gameServices.GetService<IUserInterfaceZoneService>();
			var background = spriteService.GetSprite(uiZoneElementModel.Background);
			var elementRows = new List<UiRow>();
			float width = 0f;
			float height = 0f;

			if (false == uiZoneService.UserInterfaceZones.TryGetValue((UiZoneTypes)uiZoneElementModel.UiZoneType, out UiZone uiZone))
			{
				uiZone = uiZoneService.UserInterfaceZones[UiZoneTypes.None];
			}

			if (true == uiZoneElementModel.ElementRows?.Any())
			{
				foreach (var elementRowModel in uiZoneElementModel.ElementRows)
				{
					var elementRow = this.GetUiRow(elementRowModel);
					elementRows.Add(elementRow);
					height += elementRow.Height + elementRow.TopPadding + elementRow.BottomPadding;

					if (elementRow.Width > width)
					{ 
						width = elementRow.Width;
					}
				}
			}

			var area = new SimpleArea
			{
				Position = uiZone.Position,
				Width = width,
				Height = height
			};

			return new UiZoneElement
			{
				UiZoneElementName = uiZoneElementModel.UiZoneElementName,
				DrawLayer = 1,
				JustificationType = (UiZoneElementJustificationTypes)uiZoneElementModel.JustificationType,
				Background = background,
				Area = area,
				UserInterfaceZone = uiZone,
				ElementRows = elementRows
			};
		}

		/// <summary>
		/// Gets the user interface row.
		/// </summary>
		/// <param name="uiRowModel">The user interface row model.</param>
		/// <returns>The user interface row.</returns>
		public UiRow GetUiRow(UiRowModel uiRowModel)
		{
			var spriteService = this._gameServices.GetService<ISpriteService>();
			var background = spriteService.GetSprite(uiRowModel.Background);
			var subElements = new List<IAmAUiElement>();
			float width = 0f;
			float height = uiRowModel.TopPadding + uiRowModel.BottomPadding;

			if (true == uiRowModel.SubElements?.Any())
			{
				foreach (var elementRowModel in uiRowModel.SubElements)
				{
					var uiElement = this.GetUiElement(elementRowModel);
					height += uiElement.Area.Height;
					width += uiElement.Area.Width + uiElement.LeftPadding + uiElement.RightPadding;
				}
			}

			return new UiRow
			{
				UiRowName = uiRowModel.UiRowName,
				Width = width,
				Height = height,
				TopPadding = uiRowModel.TopPadding,
				BottomPadding = uiRowModel.BottomPadding,
				JustificationType = (UiRowJustificationTypes)uiRowModel.JustificationType,
				Background = background,
				SubElements = subElements
			};
		}

		/// <summary>
		/// Gets the user interface element.
		/// </summary>
		/// <param name="uiElementModel">The user interface element model.</param>
		/// <returns>The user interface element.</returns>
		public IAmAUiElement GetUiElement(IAmAUiElementModel uiElementModel)
		{
			var uiElementType = (UiElementTypes)uiElementModel.ElementType;

			switch (uiElementType)
			{
				case UiElementTypes.Button:

					break;

				case UiElementTypes.None:
				default:

					break;
			}

			return null;
		}
	}
}

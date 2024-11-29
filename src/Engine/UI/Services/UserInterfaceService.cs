using DiscModels.Engine.UI;
using Engine.RunTime.Services.Contracts;
using Engine.UI.Models;
using Engine.UI.Models.Contracts;
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
		/// Gets the user interface element.
		/// </summary>
		/// <param name="uiElementModel">The UI element model.</param>
		/// <returns>The user interface element.</returns>
		public IAmAUiElement GetUserInterfaceElement(UserInterfaceElementModel uiElementModel, UiGroup userInterfaceGroup = null)
		{
			//var areaService = this._gameServices.GetService<IAreaService>();
			//var spriteService = this._gameServices.GetService<ISpriteService>();
			//var area = areaService.GetArea(uiElementModel.Area);
			//var sprite = spriteService.GetSprite(uiElementModel.Sprite);

			//var uiElement = new IAmAUserInterfaceElement
			//{ 
			//	UserInterfaceElementName = uiElementModel.UserInterfaceElementName,
			//	Sprite = sprite,
			//	Area = area
			//};
			
			//this.UserInterfaceElements.Add(uiElement);

			//if (null != userInterfaceGroup)
			//{ 
			//	userInterfaceGroup.UserInterfaceElements.Add(uiElement);

			//	if (false == this.UserInterfaceGroups.Contains(userInterfaceGroup))
			//	{
			//		this.UserInterfaceGroups.Add(userInterfaceGroup);
			//	}
			//}

			return null;
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
			var runtimeDrawService = this._gameServices.GetService<IRuntimeDrawService>();

			if (true == this.ActiveVisibilityGroupId.HasValue)
			{ 
				var activeGroup = this.UserInterfaceGroups.FirstOrDefault(e => e.VisibilityGroupId == this.ActiveVisibilityGroupId);

				foreach (var uiZoneContainer in activeGroup.UiZoneContainers)
				{
					runtimeDrawService.RemoveOverlaidDrawable(uiZoneContainer.DrawLayer, uiZoneContainer);
				}
			}

			foreach (var uiZoneContainer in uiGroup.UiZoneContainers)
			{
				runtimeDrawService.AddOverlaidDrawable(uiZoneContainer.DrawLayer, uiZoneContainer);
			}

			this.ActiveVisibilityGroupId = uiGroup.VisibilityGroupId;
		}
	}
}

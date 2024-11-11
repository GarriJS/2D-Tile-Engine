using DiscModels.Engine.UI;
using Engine.Drawing.Services.Contracts;
using Engine.Physics.Services.Contracts;
using Engine.RunTime.Services.Contracts;
using Engine.UI.Models;
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
		private List<UserInterfaceElement> UserInterfaceElements { get; set; } = [];

		/// <summary>
		/// Gets or sets the user interface groups.
		/// </summary>
		private List<UserInterfaceGroup> UserInterfaceGroups { get; set; } = [];

		/// <summary>
		/// Gets the user interface element.
		/// </summary>
		/// <param name="uiElementModel">The UI element model.</param>
		/// <returns>The user interface element.</returns>
		public UserInterfaceElement GetUserInterfaceElement(UserInterfaceElementModel uiElementModel, UserInterfaceGroup userInterfaceGroup = null)
		{
			var areaService = this._gameServices.GetService<IAreaService>();
			var spriteService = this._gameServices.GetService<ISpriteService>();
			var area = areaService.GetArea(uiElementModel.Area);
			var sprite = spriteService.GetSprite(uiElementModel.Sprite);

			var uiElement = new UserInterfaceElement
			{ 
				UserInterfaceElementName = uiElementModel.UserInterfaceElementName,
				Sprite = sprite,
				Area = area
			};
			
			this.UserInterfaceElements.Add(uiElement);

			if (null != userInterfaceGroup)
			{ 
				userInterfaceGroup.UserInterfaceElements.Add(uiElement);

				if (false == this.UserInterfaceGroups.Contains(userInterfaceGroup))
				{
					this.UserInterfaceGroups.Add(userInterfaceGroup);
				}
			}

			return uiElement;
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
		public void ToggleUserInterfaceGroupVisibility(UserInterfaceGroup uiGroup)
		{
			var runtimeDrawService = this._gameServices.GetService<IRuntimeDrawService>();

			if (true == this.ActiveVisibilityGroupId.HasValue)
			{ 
				var activeGroup = this.UserInterfaceGroups.FirstOrDefault(e => e.VisibilityGroupId == this.ActiveVisibilityGroupId);

				foreach (var uiElement in activeGroup.UserInterfaceElements)
				{
					runtimeDrawService.RemoveOverlaidDrawable(uiElement.DrawLayer, uiElement);
				}
			}

			foreach (var uiElement in uiGroup.UserInterfaceElements)
			{
				runtimeDrawService.AddOverlaidDrawable(uiElement.DrawLayer, uiElement);
			}

			this.ActiveVisibilityGroupId = uiGroup.VisibilityGroupId;
		}
	}
}

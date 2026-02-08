using Common.DiskModels.UserInterface;
using Common.UserInterface.Enums;
using Common.UserInterface.Models;
using Common.UserInterface.Models.Elements;
using Common.UserInterface.Services.Contracts;
using Engine.Graphics.Services.Contracts;
using Engine.RunTime.Services.Contracts;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Common.UserInterface.Services
{
	/// <summary>
	/// Represents a user group interface service.
	/// </summary>
	/// <remarks>
	/// Initializes the user group interface service.
	/// </remarks>
	/// <param name="gameServices">The game services.</param>
	sealed public class UiGroupService(GameServiceContainer gameServices) : IUiGroupService
	{
		readonly private GameServiceContainer _gameServices = gameServices;

		/// <summary>
		/// Gets the active visibility group id.
		/// </summary>
		public int? ActiveVisibilityGroupId { get; private set; }

		/// <summary>
		/// The user interface groups.
		/// </summary>
		readonly public List<UiGroup> _userInterfaceGroups = [];

		/// <summary>
		/// Gets the user interface groups.
		/// </summary>
		public List<UiGroup> UserInterfaceGroups { get => this._userInterfaceGroups; }

		//TODO move to constants file?
		/// <summary>
		/// Gets the dynamic sized types.
		/// </summary>
		readonly static public List<UiElementSizeType> _dynamicSizedTypes =
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
			var uiGroup = this._userInterfaceGroups.FirstOrDefault(e => e.VisibilityGroupId == visibilityGroupId);

			if (uiGroup is null)
			{
				// LOGGING
				return;
			}

			var runTimeOverlaidDrawService = this._gameServices.GetService<IRuntimeOverlaidDrawService>();
			var existingZone = uiGroup._zones.FirstOrDefault(e => e.UserInterfaceScreenZone.UiZoneType == uiZone.UserInterfaceScreenZone.UiZoneType);

			if (uiGroup._zones.FirstOrDefault(e => e.UserInterfaceScreenZone.UiZoneType == uiZone.UserInterfaceScreenZone.UiZoneType) is not null)
			{
				runTimeOverlaidDrawService.RemoveDrawable(existingZone);
				uiGroup._zones.Remove(existingZone);
				existingZone.Dispose();
			}

			uiGroup._zones.Add(uiZone);
			runTimeOverlaidDrawService.AddDrawable(uiZone);
		}

		/// <summary>
		/// Toggles the user interface group visibility.
		/// </summary>
		/// <param name="visibilityGroupId">The visibility group id.</param>
		public void ToggleUserInterfaceGroupVisibility(int visibilityGroupId)
		{
			var userInterfaceGroup = this._userInterfaceGroups.FirstOrDefault(e => e.VisibilityGroupId == visibilityGroupId);
			this.ToggleUserInterfaceGroupVisibility(userInterfaceGroup);
		}

		/// <summary>
		/// Toggles the user interface group visibility.
		/// </summary>
		/// <param name="uiGroup">The user interface group.</param>
		public void ToggleUserInterfaceGroupVisibility(UiGroup uiGroup)
		{
			if (uiGroup is null)
				return;

			var runTimeOverlaidDrawService = this._gameServices.GetService<IRuntimeOverlaidDrawService>();

			if (true == this.ActiveVisibilityGroupId.HasValue)
			{
				var activeGroup = this._userInterfaceGroups.FirstOrDefault(e => e.VisibilityGroupId == this.ActiveVisibilityGroupId);

				foreach (var uiZoneContainer in activeGroup._zones)
					runTimeOverlaidDrawService.RemoveDrawable(uiZoneContainer);
			}

			this.ActiveVisibilityGroupId = uiGroup.VisibilityGroupId;

			if (0 == uiGroup._zones.Count)
				return;

			var animationService = this._gameServices.GetService<IAnimationService>();

			foreach (var uiZone in uiGroup._zones)
			{
				runTimeOverlaidDrawService.AddDrawable(uiZone);
				var clickAnimations = uiZone.Blocks.SelectMany(e => e._rows)
												   .SelectMany(e => e._elements)
												   .OfType<UiButton>()
												   .Where(e => e.ClickAnimation is not null)
												   .Select(e => e.ClickAnimation)
												   .ToArray();

				if (0 == clickAnimations.Length)
					continue;

				foreach (var clickAnimation in clickAnimations)
					clickAnimation.ResetTriggeredAnimation();
			}
		}

		/// <summary>
		/// Gets the user interface group from the model.
		/// </summary>
		/// <param name="uiGroupModel">The user interface group model.</param>
		/// <returns>The user interface group.</returns>
		public UiGroup GetUiGroupFromModel(UiGroupModel uiGroupModel)
		{
			var uiZoneService = this._gameServices.GetService<IUiZoneService>();
			var uiZones = new List<UiZone>();

			foreach (var uiZoneModel in uiGroupModel.Zones)
			{
				var uiZone = uiZoneService.GetUiZoneFromModel(uiZoneModel);

				if (uiZone is not null)
					uiZones.Add(uiZone);
			}

			var uiGroup = new UiGroup
			{
				Name = uiGroupModel.Name,
				VisibilityGroupId = uiGroupModel.VisibilityGroupId
			};
			uiGroup._zones.AddRange(uiZones);
			this._userInterfaceGroups.Add(uiGroup);

			if (true == uiGroupModel.IsVisible)
				this.ToggleUserInterfaceGroupVisibility(uiGroup.VisibilityGroupId);

			return uiGroup;
		}
	}
}

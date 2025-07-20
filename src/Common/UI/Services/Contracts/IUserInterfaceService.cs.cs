using Common.DiskModels.UI;
using Common.UI.Models;
using Engine.Core.Contracts;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Common.UI.Services.Contracts
{
	/// <summary>
	/// Represents a user interface service.
	/// </summary>
	public interface IUserInterfaceService : INeedInitialization
	{
		/// <summary>
		/// Gets or sets the user interface groups.
		/// </summary>
		public List<UiGroup> UserInterfaceGroups { get; set; }

		/// <summary>
		/// Toggles the user interface group visibility.
		/// </summary>
		/// <param name="visibilityGroupId">The visibility group id.</param>
		public void ToggleUserInterfaceGroupVisibility(int visibilityGroupId);

		/// <summary>
		/// The basic user interface zone hover event processor.
		/// </summary>
		/// <param name="uiZone">The user interface zone..</param>
		/// <param name="zoneLocation">The zone location.</param>
		public void BasicUiZoneHoverEventProcessor(UiZone uiZone, Vector2 zoneLocation);

		/// <summary>
		/// Gets the user interface group.
		/// </summary>
		/// <param name="uiGroupModel">The user interface group model.</param>
		/// <returns>The user interface group.</returns>
		public UiGroup GetUiGroup(UiGroupModel uiGroupModel);

		/// <summary>
		/// Toggles the user interface group visibility.
		/// </summary>
		/// <param name="uiGroup">The user interface group.</param>
		public void ToggleUserInterfaceGroupVisibility(UiGroup uiGroup);

		/// <summary>
		/// Gets the user interface object at the screen location.
		/// </summary>
		/// <param name="location">The location.</param>
		/// <returns>The user interface object at the location if one is found.</returns>
		public object GetUiObjectAtScreenLocation(Vector2 location);

		/// <summary>
		/// Gets the user interface zone.
		/// </summary>
		/// <param name="uiZoneModel">The user interface model.</param>
		/// <param name="visibilityGroup">The visibility group of the user interface zone.</param>
		/// <returns>The user interface zone.</returns>
		public UiZone GetUiZone(UiZoneModel uiZoneModel, int visibilityGroup);

		/// <summary>
		/// Gets the user interface row.
		/// </summary>
		/// <param name="uiRowModel">The user interface row model.</param>
		/// <param name="uiZone">The user interface zone.</param>
		/// <param name="visibilityGroup">The visibility group of the user interface row.</param>
		/// <returns>The user interface row.</returns>
		public UiRow GetUiRow(UiRowModel uiRowModel, UiScreenZone uiZone, int visibilityGroup);
	}
}

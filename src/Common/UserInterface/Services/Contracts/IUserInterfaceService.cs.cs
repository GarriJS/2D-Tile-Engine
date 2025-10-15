using Common.Controls.CursorInteraction.Models;
using Common.DiskModels.UI;
using Common.UserInterface.Models;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Common.UserInterface.Services.Contracts
{
	/// <summary>
	/// Represents a user interface service.
	/// </summary>
	public interface IUserInterfaceService
	{
		/// <summary>
		/// Gets or sets the user interface groups.
		/// </summary>
		public List<UiGroup> UserInterfaceGroups { get; set; }

		/// <summary>
		/// Adds the user interface zone to the user interface group.
		/// </summary>
		/// <param name="visibilityGroupId">The visibility group id.</param>
		/// <param name="uiZone">The user interface zone.</param>
		public void AddUserInterfaceZoneToUserInterfaceGroup(int visibilityGroupId, UiZone uiZone);

		/// <summary>
		/// Toggles the user interface group visibility.
		/// </summary>
		/// <param name="visibilityGroupId">The visibility group id.</param>
		public void ToggleUserInterfaceGroupVisibility(int visibilityGroupId);

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
		/// Gets the user interface hover state at the screen location.
		/// </summary>
		/// <param name="location">The location.</param>
		/// <returns>The user interface hover state at the location if one is found.</returns>
		public HoverState GetUiObjectAtScreenLocation(Vector2 location);

		/// <summary>
		/// Gets the user interface zone.
		/// </summary>
		/// <param name="uiZoneModel">The user interface model.</param>
		/// <returns>The user interface zone.</returns>
		public UiZone GetUiZone(UiZoneModel uiZoneModel);

		/// <summary>
		/// Gets the user interface row.
		/// </summary>
		/// <param name="uiRowModel">The user interface row model.</param>
		/// <returns>The user interface row.</returns>
		public UiRow GetUiRow(UiRowModel uiRowModel);
	}
}

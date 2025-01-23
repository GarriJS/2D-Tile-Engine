using Engine.DiskModels.Engine.UI;
using Engine.UI.Models;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Engine.UI.Services.Contracts
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
		/// Gets the user interface element at the screen location.
		/// </summary>
		/// <param name="location">The location.</param>
		/// <returns>The user interface element at the location if one is found.</returns>
		public UiElementWithLocation GetUiElementAtScreenLocation(Vector2 location);

		/// <summary>
		/// Gets the user interface zone element.
		/// </summary>
		/// <param name="uiZoneElementModel">The user interface element model.</param>
		/// <param name="visibilityGroup">The visibility group of the user interface zone.</param>
		/// <returns>The user interface zone element.</returns>
		public UiZone GetUiZoneElement(UiZoneModel uiZoneElementModel, int? visibilityGroup);

		/// <summary>
		/// Gets the user interface row.
		/// </summary>
		/// <param name="uiRowModel">The user interface row model.</param>
		/// <param name="uiZone">The user interface zone.</param>
		/// <param name="fillHeight">The fill height.</param>
		/// <param name="visibilityGroup">The visibility group of the user interface row.</param>
		/// <returns>The user interface row.</returns>
		public UiRow GetUiRow(UiRowModel uiRowModel, UiScreenZone uiZone, float fillHeight, int? visibilityGroup);
	}
}

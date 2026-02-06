using Common.DiskModels.UserInterface;
using Common.UserInterface.Models;
using System.Collections.Generic;

namespace Common.UserInterface.Services.Contracts
{
	/// <summary>
	/// Represents a user interface service.
	/// </summary>
	public interface IUserInterfaceService
	{
		/// <summary>
		/// Gets the active visibility group id.
		/// </summary>
		public int? ActiveVisibilityGroupId { get; }

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
		public UiGroup GetUiGroupFromModel(UiGroupModel uiGroupModel);

		/// <summary>
		/// Toggles the user interface group visibility.
		/// </summary>
		/// <param name="uiGroup">The user interface group.</param>
		public void ToggleUserInterfaceGroupVisibility(UiGroup uiGroup);

		/// <summary>
		/// Gets the user interface zone from the model.
		/// </summary>
		/// <param name="uiZoneModel">The user interface model.</param>
		/// <returns>The user interface zone.</returns>
		public UiZone GetUiZoneFromModel(UiZoneModel uiZoneModel);

		/// <summary>
		/// Get the user interface block from the model.
		/// </summary>
		/// <param name="uiBlockModel">The user interface block model.</param>
		/// <returns>The user interface block.</returns>
		public UiBlock GetUiBlockFromModel(UiBlockModel uiBlockModel);

		/// <summary>
		/// Gets the user interface row from the model.
		/// </summary>
		/// <param name="uiRowModel">The user interface row model.</param>
		/// <returns>The user interface row.</returns>
		public UiRow GetUiRowFromModel(UiRowModel uiRowModel);
	}
}

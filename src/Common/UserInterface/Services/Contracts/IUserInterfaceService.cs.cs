using Common.DiskModels.UserInterface;
using Common.UserInterface.Models;
using System.Collections.Generic;

namespace Common.UserInterface.Services.Contracts
{
	/// <summary>
	/// Represents a user interface group service.
	/// </summary>
	public interface IUserInterfaceGroupService
	{
		/// <summary>
		/// Gets the active visibility group id.
		/// </summary>
		public int? ActiveVisibilityGroupId { get; }

		/// <summary>
		/// Gets the user interface groups.
		/// </summary>
		public List<UiGroup> UserInterfaceGroups { get; } 

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
	}
}

using DiscModels.Engine.UI;
using Engine.UI.Models;

namespace Engine.UI.Services.Contracts
{
	/// <summary>
	/// Represents a user interface service.
	/// </summary>
	public interface IUserInterfaceService
	{
		/// <summary>
		/// Toggles the user interface group visibility.
		/// </summary>
		/// <param name="visibilityGroupId">The visibility group id.</param>
		public void ToggleUserInterfaceGroupVisibility(int visibilityGroupId);

		/// <summary>
		/// Toggles the user interface group visibility.
		/// </summary>
		/// <param name="uiGroup">The user interface group.</param>
		public void ToggleUserInterfaceGroupVisibility(UiGroup uiGroup);

		/// <summary>
		/// Gets the user interface zone element.
		/// </summary>
		/// <param name="uiZoneElementModel">The user interface element model.</param>
		/// <returns>The user interface zone element.</returns>
		public UiZoneElement GetUiZoneElement(UiZoneElementModel uiZoneElementModel);

		/// <summary>
		/// Gets the user interface row.
		/// </summary>
		/// <param name="uiRowModel">The user interface row model.</param>
		/// <param name="uiZone">The user interface zone.</param>
		/// <param name="height">The height.</param>
		/// <returns>The user interface row.</returns>
		public UiRow GetUiRow(UiRowModel uiRowModel, UiZone uiZone, float height);
	}
}

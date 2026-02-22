using Common.UserInterface.Models;
using Engine.Core.Contracts;

namespace Common.Debugging.Services.Contracts
{
	/// <summary>
	/// Represents a common debug service.
	/// </summary>
	public interface ICommonDebugService : IPostLoadInitialize
	{
		/// <summary>
		/// Sets the performance rate counter activity.
		/// </summary>
		/// <param name="enable">A value indicating whether to enable the performance counters.</param>
		public void SetScreenAreaIndicatorsEnabled(bool enable);

		/// <summary>
		/// Adds the user interface zone rectangles.
		/// </summary>
		/// <param name="uiZone">The user interface zone.</param>
		public void AddDebugUserInterfaceZone(UiZone uiZone);

		/// <summary>
		/// Removes the user interface zone from debugging.
		/// </summary>
		/// <param name="uiZone">The user interface zone.</param>
		public void RemoveDebugUserInterfaceZone(UiZone uiZone);

		/// <summary>
		/// Clears the debug user interface zones.
		/// </summary>
		public void ClearDebugUserInterfaceZones();

		/// <summary>
		/// Adds the user interface modal from debugging.
		/// </summary>
		/// <param name="uiModal">The user interface modal.</param>
		public void AddDebugUserInterfaceModal(UiModal uiModal);

		/// <summary>
		/// Removes the user interface modal from debugging.
		/// </summary>
		/// <param name="uiModal">The user interface modal.</param>
		public void RemoveDebugUserInterfaceModal(UiModal uiModal);

		/// <summary>
		/// Clears the debug user interface modals.
		/// </summary>
		public void ClearDebugUserInterfaceModals();
	}
}

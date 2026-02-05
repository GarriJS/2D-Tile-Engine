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
		/// Adds the user interface zone rectangles.
		/// </summary>
		public void ClearDebugUserInterfaceZones();
	}
}

using Common.DiskModels.UserInterface;
using Common.UserInterface.Models;

namespace Common.UserInterface.Services.Contracts
{
	/// <summary>
	/// Represents a user interface zone service.
	/// </summary>
	public interface IUiZoneService
	{
		/// <summary>
		/// Gets the user interface zone from the model.
		/// </summary>
		/// <param name="uiZoneModel">The user interface model.</param>
		/// <returns>The user interface zone.</returns>
		public UiZone GetUiZoneFromModel(UiZoneModel uiZoneModel);
	}
}

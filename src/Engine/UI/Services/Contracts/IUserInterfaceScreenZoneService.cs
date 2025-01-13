using Engine.UI.Models.Enums;
using Engine.UI.Models;
using System.Collections.Generic;
using Engine.Core.Contracts;

namespace Engine.UI.Services.Contracts
{
	/// <summary>
	/// Represents a user interface screen zone service.
	/// </summary>
	public interface IUserInterfaceScreenZoneService : INeedInitialization
	{
		/// <summary>
		/// Gets or sets the user interface zones.
		/// </summary>
		public Dictionary<UiScreenZoneTypes, UiScreenZone> UserInterfaceScreenZones { get; set; }

		/// <summary>
		/// Initialize the user interface zones.
		/// </summary>
		public void InitializeUiScreenZones();
	}
}

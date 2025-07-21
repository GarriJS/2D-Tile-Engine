using Common.UserInterface.Models;
using Common.UserInterface.Enums;
using Engine.Core.Contracts;
using System.Collections.Generic;

namespace Common.UserInterface.Services.Contracts
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

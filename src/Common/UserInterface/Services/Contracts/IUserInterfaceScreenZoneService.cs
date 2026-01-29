using Common.UserInterface.Enums;
using Common.UserInterface.Models;
using Engine.Core.Contracts;
using Engine.Physics.Models.SubAreas;
using System.Collections.Generic;

namespace Common.UserInterface.Services.Contracts
{
	/// <summary>
	/// Represents a user interface screen zone service.
	/// </summary>
	public interface IUserInterfaceScreenZoneService : IDoConfiguration
	{
		/// <summary>
		/// Gets or sets the screen zone size.
		/// </summary>
		public SubArea ScreenZoneSize { get; set; }

		/// <summary>
		/// Gets or sets the user interface zones.
		/// </summary>
		public Dictionary<UiZonePositionType, UiScreenZone> UserInterfaceScreenZones { get; set; }

		/// <summary>
		/// ConfigureService the user interface zones.
		/// </summary>
		public void InitializeUiScreenZones();
	}
}

using Common.UserInterface.Enums;
using Common.UserInterface.Models;
using Engine.Core.Contracts;
using Engine.Physics.Models.SubAreas;
using System.Collections.Generic;

namespace Common.UserInterface.Services.Contracts
{
	/// <summary>
	/// Represents a user interface screen service.
	/// </summary>
	public interface IUiScreenService : IDoConfiguration
	{
		/// <summary>
		/// Gets or sets the screen zone size.
		/// </summary>
		public SubArea ScreenZoneSize { get; }

		/// <summary>
		/// Gets or sets the user interface zones.
		/// </summary>
		public Dictionary<UiZonePositionType, UiScreenZone> UiScreenZones { get; }

		/// <summary>
		/// Initializes the user interface zones.
		/// </summary>
		public void InitializeUiScreenZones();
	}
}

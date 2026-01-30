using Common.UserInterface.Enums;
using Engine.Physics.Models;
using Engine.Physics.Models.Contracts;
using Microsoft.VisualBasic;

namespace Common.UserInterface.Models
{
	/// <summary>
	/// Represents a user screen interface zone.
	/// </summary>
	public class UiScreenZone : IHaveArea
	{
		/// <summary>
		/// Gets or sets the user interface zone type.
		/// </summary>
		public UiZonePositionType UiZoneType { get; set; }

		/// <summary>
		/// Gets the position.
		/// </summary>
		public Position Position { get => this.Area.Position; }

		/// <summary>
		/// Gets or sets the area.
		/// </summary>
		public IAmAArea Area { get; set; }
	}
}

using Engine.Physics.Models;
using Engine.Physics.Models.Contracts;
using Engine.UI.Models.Enums;

namespace Engine.UI.Models
{
	/// <summary>
	/// Represents a user interface zone.
	/// </summary>
	public class UiZone : IHaveArea
	{
		/// <summary>
		/// Gets or sets the user interface zone type.
		/// </summary>
		public UiZoneTypes UserInterfaceZoneType { get; set; }

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

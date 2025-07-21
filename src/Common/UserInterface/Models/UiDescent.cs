using Common.UserInterface.Models.Contracts;
using Microsoft.Xna.Framework;

namespace Common.UserInterface.Models
{
	/// <summary>
	/// Represents a user interface descent.
	/// </summary>
	public class UiDescent
	{
		/// <summary>
		/// Gets or sets the location.
		/// </summary>
		public Vector2 Location { get; set; }

		/// <summary>
		/// The user interface group.
		/// </summary>
		public UiGroup Group { get; set; }

		/// <summary>
		/// The user interface zone.
		/// </summary>
		public UiZone Zone { get; set; }

		/// <summary>
		/// The user interface row.
		/// </summary>
		public UiRow Row { get; set; }

		/// <summary>
		/// The user interface element.
		/// </summary>
		public IAmAUiElement Element { get; set; }
	}
}

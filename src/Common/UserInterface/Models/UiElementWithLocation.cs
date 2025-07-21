using Common.UserInterface.Models.Contracts;
using Microsoft.Xna.Framework;

namespace Common.UserInterface.Models
{
	/// <summary>
	/// Represents user interface element with a location.
	/// </summary>
	public class UiElementWithLocation
	{
		/// <summary>
		/// Gets or sets the element.
		/// </summary>
		public IAmAUiElement Element { get; set; }

		/// <summary>
		/// Gets or sets the location.
		/// </summary>
		public Vector2 Location { get; set; }
	}
}

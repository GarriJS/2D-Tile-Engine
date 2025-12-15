using Common.UserInterface.Models.Contracts;
using Microsoft.Xna.Framework;

namespace Common.UserInterface.Models.LayoutInfo
{
	/// <summary>
	/// Represents a element layout info.
	/// </summary>
	public class ElementLayoutInfo
	{
		/// <summary>
		/// Gets or sets the element.
		/// </summary>
		public IAmAUiElement Element { get; init; }

		/// <summary>
		/// Gets or sets the location of the element.
		/// </summary>
		public Vector2 Offset { get; init; }
	}
}

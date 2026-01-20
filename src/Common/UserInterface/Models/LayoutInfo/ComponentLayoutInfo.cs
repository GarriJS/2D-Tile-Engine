using Common.UserInterface.Models.Contracts;
using Microsoft.Xna.Framework;

namespace Common.UserInterface.Models.LayoutInfo
{
	/// <summary>
	/// Represents a component layout info.
	/// </summary>
	public class ComponentLayoutInfo
	{
		/// <summary>
		/// Gets or sets the component.
		/// </summary>
		public IAmAUiZoneChild Component { get; init; }

		/// <summary>
		/// Gets or sets the location of the component.
		/// </summary>
		public Vector2 Offset { get; init; }
	}
}

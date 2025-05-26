using Microsoft.Xna.Framework;
using System;

namespace Engine.UI.Models.Contracts
{
	/// <summary>
	/// Represents something that can be hovered.
	/// </summary>
	public interface ICanBeHovered
	{
		/// <summary>
		/// Gets or set the press event.
		/// </summary>
		public event Action<IAmAUiElement, Vector2> HoverEvent;

		/// <summary>
		/// Raises the hover event.
		/// </summary>
		/// <param name="elementLocation">The element location.</param>
		public void RaiseHoverEvent(Vector2 elementLocation);
	}
}

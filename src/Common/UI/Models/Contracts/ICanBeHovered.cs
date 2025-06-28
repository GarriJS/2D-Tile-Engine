using Microsoft.Xna.Framework;
using System;

namespace Common.UI.Models.Contracts
{
	/// <summary>
	/// Represents something that can be hovered.
	/// </summary>
	/// <typeparam name="T">The type being hovered.</typeparam>
	public interface ICanBeHovered<T>
	{
		/// <summary>
		/// Gets or set the press event.
		/// </summary>
		public event Action<T, Vector2> HoverEvent;

		/// <summary>
		/// Raises the hover event.
		/// </summary>
		/// <param name="elementLocation">The element location.</param>
		public void RaiseHoverEvent(Vector2 elementLocation);
	}
}

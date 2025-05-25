using Microsoft.Xna.Framework;
using System;

namespace Engine.UI.Models.Contracts
{
	/// <summary>
	/// Represents something that can be hovered.
	/// </summary>
	/// <typeparam name="TDerived">The derived type implementing this interface.</typeparam>
	public interface ICanBeHovered<TDerived> where TDerived : ICanBeHovered<TDerived>
	{
		/// <summary>
		/// Gets or set the press event.
		/// </summary>
		public event Action<TDerived, Vector2> HoverEvent;

		/// <summary>
		/// Raises the hover event.
		/// </summary>
		/// <param name="elementLocation">The element location.</param>
		public void RaiseHoverEvent(Vector2 elementLocation);
	}
}

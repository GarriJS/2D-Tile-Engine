using Microsoft.Xna.Framework;
using System;

namespace Engine.UI.Models.Contracts
{
	/// <summary>
	/// Represents something that can be pressed.
	/// </summary>
	public interface ICanBePressed
	{
		/// <summary>
		/// Gets or set the press event.
		/// </summary>
		public event Action<IAmAUiElement, Vector2> PressEvent;

		/// <summary>
		/// Raises the press event.
		/// </summary>
		/// <param name="elementLocation">The element location.</param>
		public void RaisePressEvent(Vector2 elementLocation);
	}
}

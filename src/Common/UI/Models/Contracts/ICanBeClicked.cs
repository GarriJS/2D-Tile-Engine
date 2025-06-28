using Microsoft.Xna.Framework;
using System;

namespace Common.UI.Models.Contracts
{
	/// <summary>
	/// Represents something that can be clicked.
	/// </summary>
	public interface ICanBeClicked
	{
		/// <summary>
		/// Gets or sets the click event.
		/// </summary>
		public event Action<IAmAUiElement, Vector2> ClickEvent;

		/// <summary>
		/// Raises the click event.
		/// </summary>
		public void RaiseClickEvent(Vector2 elementLocation);
	}
}

using Microsoft.Xna.Framework;
using System;

namespace Engine.UI.Models.Contracts
{
	/// <summary>
	/// Represents something that can be clicked.
	/// </summary>
	/// <typeparam name="TDerived">The derived type implementing this interface.</typeparam>
	public interface ICanBeClicked<TDerived> where TDerived : ICanBeClicked<TDerived>
	{
		/// <summary>
		/// Gets or sets the click event.
		/// </summary>
		public event Action<TDerived, Vector2> ClickEvent;
	}
}

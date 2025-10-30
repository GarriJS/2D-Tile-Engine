using Common.Controls.CursorInteraction.Models.Abstract;
using Engine.Physics.Models;
using System;

namespace Common.Controls.CursorInteraction.Models
{
	/// <summary>
	/// Represents a click configuration.
	/// </summary>
	/// <typeparam name="T">The parent type.</typeparam>
	public class ClickConfiguration<T> : BaseClickConfiguration, IDisposable
	{
		/// <summary>
		/// Gets or set the click event.
		/// </summary>
		protected event Action<LocationExtender<T>> ClickEvent;

		/// <summary>
		/// Adds the subscription.
		/// </summary>
		/// <param name="action">The action.</param>
		public void AddSubscription(Action<LocationExtender<T>> action)
		{
			this.ClickEvent += action;
		}

		/// <summary>
		/// Removes the subscription.
		/// </summary>
		/// <param name="action">The action.</param>
		public void RemoveSubscription(Action<LocationExtender<T>> action)
		{
			this.ClickEvent -= action;
		}

		/// <summary>
		/// Raises the click event.
		/// </summary>
		/// <param name="objectWithClickLocation">The element with click location.</param>
		public void RaiseClickEvent(LocationExtender<T> objectWithClickLocation)
		{
			this.ClickEvent?.Invoke(objectWithClickLocation);
		}

		/// <summary>
		/// Disposes of the click configuration.
		/// </summary>
		public void Dispose()
		{
			this.ClickEvent = null;
		}
	}
}

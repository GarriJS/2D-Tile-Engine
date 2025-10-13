using Engine.Physics.Models;
using Engine.Physics.Models.Contracts;
using Microsoft.Xna.Framework;
using System;

namespace Common.Controls.CursorInteraction.Models
{
	/// <summary>
	/// Represents a click configuration.
	/// </summary>
	/// <typeparam name="T">The parent type.</typeparam>
	public class ClickConfiguration<T> : IHaveASubArea, IDisposable
	{
		/// <summary>
		/// Gets or sets the area.
		/// </summary>
		public SubArea Area { get; set; }

		/// <summary>
		/// Gets or sets the offset;
		/// </summary>
		public Vector2 Offset { get; set; }

		/// <summary>
		/// Gets or set the click event.
		/// </summary>
		protected event Action<T, Vector2> ClickEvent;

		/// <summary>
		/// Adds the subscription.
		/// </summary>
		/// <param name="action">The action.</param>
		public void AddSubscription(Action<T, Vector2> action)
		{
			this.ClickEvent += action;
		}

		/// <summary>
		/// Removes the subscription.
		/// </summary>
		/// <param name="action">The action.</param>
		public void RemoveSubscription(Action<T, Vector2> action)
		{
			this.ClickEvent -= action;
		}

		/// <summary>
		/// Raises the click event.
		/// </summary>
		/// <param name="parent">The parent object being clicked.</param>
		/// <param name="elementLocation">The element location.</param>
		public void RaiseClickEvent(T parent, Vector2 elementLocation)
		{
			this.ClickEvent?.Invoke(parent, elementLocation);
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

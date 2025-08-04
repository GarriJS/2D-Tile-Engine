using Common.Controls.CursorInteraction.Models.Abstract;
using Microsoft.Xna.Framework;
using System;

namespace Common.Controls.CursorInteraction.Models
{
	/// <summary>
	/// Represents a hover configuration.
	/// </summary>
	/// <typeparam name="T">The parent type.</typeparam>
	public class HoverConfiguration<T> : BaseHoverConfiguration
	{
		/// <summary>
		/// Gets or set the hover event.
		/// </summary>
		public event Action<T, Vector2> HoverEvent;

		/// <summary>
		/// Adds the subscription.
		/// </summary>
		/// <param name="action">The action.</param>
		public void AddSubscription(Action<T, Vector2> action)
		{
			this.HoverEvent += action;
		}

		/// <summary>
		/// Removes the subscription.
		/// </summary>
		/// <param name="action">The action.</param>
		public void RemoveSubscription(Action<T, Vector2> action)
		{
			this.HoverEvent -= action;
		}

		/// <summary>
		/// Raises the hover event.
		/// </summary>
		/// <param name="parent">The parent object being hovered.</param>
		/// <param name="elementLocation">The element location.</param>
		public void RaiseHoverEvent(T parent, Vector2 elementLocation)
		{
			this.HoverEvent?.Invoke(parent, elementLocation);
		}

		/// <summary>
		/// Disposes of the hover configuration.
		/// </summary>
		new public void Dispose()
		{
			this.HoverEvent = null;
			this.HoverCursor.Dispose();
		}
	}
}

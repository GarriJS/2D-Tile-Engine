using Common.Controls.Cursors.Models;
using Engine.Physics.Models.Contracts;
using Microsoft.Xna.Framework;
using System;

namespace Common.Controls.CursorInteraction.Models
{
	/// <summary>
	/// Represents a hover configuration.
	/// </summary>
	/// <typeparam name="T">The parent type.</typeparam>
	public class HoverConfiguration<T> : IHaveASubArea, IDisposable
	{
		/// <summary>
		/// Gets or sets the area.
		/// </summary>
		public Vector2 Area { get; set; }

		/// <summary>
		/// Gets or sets the offset;
		/// </summary>
		public Vector2 Offset { get; set; }

		/// <summary>
		/// Gets or sets the hover cursor.
		/// </summary>
		public Cursor HoverCursor { get; set; }

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
		public void Dispose()
		{
			this.HoverEvent = null;
			this.HoverCursor.Dispose();
		}
	}
}

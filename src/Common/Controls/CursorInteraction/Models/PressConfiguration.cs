using Common.Controls.CursorInteraction.Models.Abstract;
using Microsoft.Xna.Framework;
using System;

namespace Common.Controls.CursorInteraction.Models
{
	/// <summary>
	/// Represents a press configuration.
	/// </summary>
	/// <typeparam name="T">The parent type.</typeparam>
	public class PressConfiguration<T> : BasePressConfiguration, IDisposable
	{
		/// <summary>
		/// Gets or set the press event.
		/// </summary>
		private event Action<T, Vector2, Vector2> PressEvent;

		/// <summary>
		/// Adds the subscription.
		/// </summary>
		/// <param name="action">The action.</param>
		public void AddSubscription(Action<T, Vector2, Vector2> action)
		{
			this.PressEvent += action;
		}

		/// <summary>
		/// Removes the subscription.
		/// </summary>
		/// <param name="action">The action.</param>
		public void RemoveSubscription(Action<T, Vector2, Vector2> action)
		{ 
			this.PressEvent -= action;
		}

		/// <summary>
		/// Raises the press event.
		/// </summary>
		/// <param name="parent">The parent object being pressed.</param>
		/// <param name="elementLocation">The element location.</param>
		/// <param name="pressLocation">The press location.</param>
		public void RaisePressEvent(T parent, Vector2 elementLocation, Vector2 pressLocation)
		{
			this.PressEvent?.Invoke(parent, elementLocation, pressLocation);
		}

		/// <summary>
		/// Disposes of the press configuration.
		/// </summary>
		public void Dispose()
		{ 
			this.PressEvent = null;
		}
	}
}

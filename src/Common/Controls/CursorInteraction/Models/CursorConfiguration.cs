using Common.Controls.CursorInteraction.Models.Abstract;
using System;

namespace Common.Controls.CursorInteraction.Models
{
	/// <summary>
	/// Represents a cursor configuration.
	/// </summary>
	/// <typeparam name="T">The parent type.</typeparam>
	sealed public class CursorConfiguration<T> : BaseCursorConfiguration
	{
		/// <summary>
		/// Gets or set the hover event.
		/// </summary>
		protected event Action<CursorInteraction<T>> HoverEvent;

		/// <summary>
		/// Gets or set the click event.
		/// </summary>
		protected event Action<CursorInteraction<T>> ClickEvent;

		/// <summary>
		/// Gets or set the press event.
		/// </summary>
		protected event Action<CursorInteraction<T>> PressEvent;

		/// <summary>
		/// Adds the hover subscription.
		/// </summary>
		/// <param name="action">The action.</param>
		public void AddHoverSubscription(Action<CursorInteraction<T>> action)
		{
			this.HoverEvent += action;
		}

		/// <summary>
		/// Adds the press subscription.
		/// </summary>
		/// <param name="action">The action.</param>
		public void AddPressSubscription(Action<CursorInteraction<T>> action)
		{
			this.PressEvent += action;
		}

		/// <summary>
		/// Adds the click subscription.
		/// </summary>
		/// <param name="action">The action.</param>
		public void AddClickSubscription(Action<CursorInteraction<T>> action)
		{
			this.ClickEvent += action;
		}

		/// <summary>
		/// Removes the hover subscription.
		/// </summary>
		/// <param name="action">The action.</param>
		public void RemoveHoverSubscription(Action<CursorInteraction<T>> action)
		{
			this.HoverEvent -= action;
		}

		/// <summary>
		/// Removes the press subscription.
		/// </summary>
		/// <param name="action">The action.</param>
		public void RemovePressSubscription(Action<CursorInteraction<T>> action)
		{
			this.PressEvent -= action;
		}

		/// <summary>
		/// Removes the click subscription.
		/// </summary>
		/// <param name="action">The action.</param>
		public void RemoveClickSubscription(Action<CursorInteraction<T>> action)
		{
			this.ClickEvent -= action;
		}

		/// <summary>
		/// Raises the hover event.
		/// </summary>
		/// <param name="cursorInteraction">The hover interaction.</param>
		public void RaiseHoverEvent(CursorInteraction<T> cursorInteraction)
		{
			this.HoverEvent?.Invoke(cursorInteraction);
		}

		/// <summary>
		/// Raises press click event.
		/// </summary>
		/// <param name="cursorInteraction">The press interaction.</param>
		public void RaisePressEvent(CursorInteraction<T> cursorInteraction)
		{
			this.PressEvent?.Invoke(cursorInteraction);
		}

		/// <summary>
		/// Raises the click event.
		/// </summary>
		/// <param name="cursorInteraction">The cursor interaction.</param>
		public void RaiseClickEvent(CursorInteraction<T> cursorInteraction)
		{
			this.ClickEvent?.Invoke(cursorInteraction);
		}

		/// <summary>
		/// Disposes of the cursor configuration.
		/// </summary>
		public void Dispose()
		{
			this.HoverEvent = null;
			this.PressEvent = null;
			this.ClickEvent = null;
		}
	}
}

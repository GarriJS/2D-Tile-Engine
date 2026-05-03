using Common.Controls.CursorInteractions.Models.Abstract;
using System;

namespace Common.Controls.CursorInteractions.Models
{
	/// <summary>
	/// Represents a cursor configuration.
	/// </summary>
	sealed public class CursorConfiguration : BaseCursorConfiguration
	{
		/// <summary>
		/// Gets or set the hover event.
		/// </summary>
		private event Action<CursorInteraction> HoverEvent;

        /// <summary>
        /// Gets or set the press event.
        /// </summary>
        private event Action<CursorInteraction> PressEvent;

        /// <summary>
        /// Gets or set the click event.
        /// </summary>
        private event Action<CursorInteraction> ClickEvent;

		/// <summary>
		/// Adds the hover subscription.
		/// </summary>
		/// <param name="action">The action.</param>
		override public void AddHoverSubscription(Action<CursorInteraction> action)
		{
			this.HoverEvent += action;
		}

        /// <summary>
        /// Adds the press subscription.
        /// </summary>
        /// <param name="action">The action.</param>
        override public void AddPressSubscription(Action<CursorInteraction> action)
		{
			this.PressEvent += action;
		}

        /// <summary>
        /// Adds the click subscription.
        /// </summary>
        /// <param name="action">The action.</param>
        override public void AddClickSubscription(Action<CursorInteraction> action)
		{
			this.ClickEvent += action;
		}

        /// <summary>
        /// Removes the hover subscription.
        /// </summary>
        /// <param name="action">The action.</param>
        override public void RemoveHoverSubscription(Action<CursorInteraction> action)
		{
			this.HoverEvent -= action;
		}

        /// <summary>
        /// Removes the press subscription.
        /// </summary>
        /// <param name="action">The action.</param>
        override public void RemovePressSubscription(Action<CursorInteraction> action)
		{
			this.PressEvent -= action;
		}

        /// <summary>
        /// Removes the click subscription.
        /// </summary>
        /// <param name="action">The action.</param>
        override public void RemoveClickSubscription(Action<CursorInteraction> action)
		{
			this.ClickEvent -= action;
		}

		/// <summary>
		/// Raises the hover event.
		/// </summary>
		/// <param name="cursorInteraction">The hover interaction.</param>
		public void RaiseHoverEvent(CursorInteraction cursorInteraction)
		{
			this.HoverEvent?.Invoke(cursorInteraction);
		}

		/// <summary>
		/// Raises press click event.
		/// </summary>
		/// <param name="cursorInteraction">The press interaction.</param>
		public void RaisePressEvent(CursorInteraction cursorInteraction)
		{
			this.PressEvent?.Invoke(cursorInteraction);
		}

		/// <summary>
		/// Raises the click event.
		/// </summary>
		/// <param name="cursorInteraction">The cursor interaction.</param>
		public void RaiseClickEvent(CursorInteraction cursorInteraction)
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

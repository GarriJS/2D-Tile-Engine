using Engine.Physics.Models.Contracts;
using Engine.Physics.Models.SubAreas;
using Microsoft.Xna.Framework;
using System;

namespace Common.Controls.CursorInteractions.Models.Abstract
{
	/// <summary>
	/// Represents a base cursor configuration.
	/// </summary>
	abstract public class BaseCursorConfiguration : IHaveASubArea
	{
		/// Gets or sets the offset;
		/// </summary>
		public Vector2 Offset { get; set; }

		/// <summary>
		/// Gets or sets the click offset.
		/// </summary>
		public Vector2 ClickOffset { get; set; }

		/// <summary>
		/// Gets or sets the area.
		/// </summary>
		public SubArea SubArea { get; set; }

		/// <summary>
		/// Gets or sets the click area.
		/// </summary>
		public SubArea ClickArea { get; set; }

        /// <summary>
        /// Adds the hover subscription.
        /// </summary>
        /// <param name="action">The action.</param>
        abstract public void AddHoverSubscription(Action<CursorInteraction> action);

        /// <summary>
        /// Adds the press subscription.
        /// </summary>
        /// <param name="action">The action.</param>
        abstract public void AddPressSubscription(Action<CursorInteraction> action);

        /// <summary>
        /// Adds the click subscription.
        /// </summary>
        /// <param name="action">The action.</param>
        abstract public void AddClickSubscription(Action<CursorInteraction> action);

        /// <summary>
        /// Removes the hover subscription.
        /// </summary>
        /// <param name="action">The action.</param>
        abstract public void RemoveHoverSubscription(Action<CursorInteraction> action);

        /// <summary>
        /// Removes the press subscription.
        /// </summary>
        /// <param name="action">The action.</param>
        abstract public void RemovePressSubscription(Action<CursorInteraction> action);

        /// <summary>
        /// Removes the click subscription.
        /// </summary>
        /// <param name="action">The action.</param>
        abstract public void RemoveClickSubscription(Action<CursorInteraction> action);

    }
}

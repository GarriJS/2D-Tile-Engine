using Microsoft.Xna.Framework;
using System;

namespace Common.Controls.CursorInteraction.Models.Contracts
{
	/// <summary>
	/// Represents something that can be clicked.
	/// </summary>
	/// <typeparam name="T">The type being clicked.</typeparam>
	public interface ICanBeClicked<T> : IDisposable
    {
        /// <summary>
        /// Gets or sets the click configuration.
        /// </summary>
        public ClickConfiguration<T> ClickConfig { get; set; }

        /// <summary>
        /// Raises the click event.
        /// </summary>
        /// <param name="elementLocation">The element location.</param>
        public void RaiseClickEvent(Vector2 elementLocation);
    }
}

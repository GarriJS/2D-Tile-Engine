using Microsoft.Xna.Framework;
using System;

namespace Common.Controls.CursorInteraction.Models.Contracts
{
	/// <summary>
	/// Represents something that can be pressed.
	/// </summary>
	/// <typeparam name="T">The type being pressed.</typeparam>
	public interface ICanBePressed<T> : IDisposable
    {
        /// <summary>
        /// Gets the press configuration.
        /// </summary>
        public PressConfiguration<T> PressConfig { get; }

        /// <summary>
        /// Raises the press event.
        /// </summary>
        /// <param name="elementLocation">The element location.</param>
        /// <param name="pressLocation">The press location.</param>
        public void RaisePressEvent(Vector2 elementLocation, Vector2 pressLocation);
    }
}

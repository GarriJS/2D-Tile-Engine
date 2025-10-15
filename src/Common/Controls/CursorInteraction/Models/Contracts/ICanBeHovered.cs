using Common.Controls.CursorInteraction.Models.Abstract;
using Microsoft.Xna.Framework;
using System;

namespace Common.Controls.CursorInteraction.Models.Contracts
{
	/// <summary>
	/// Represents something that can be hovered.
	/// </summary>
	/// <typeparam name="T">The type being hovered.</typeparam>
	public interface ICanBeHovered<T> : IHaveAHoverConfiguration, IDisposable
	{
        /// <summary>
        /// Gets the base hover configuration.
        /// </summary>
        new public BaseHoverConfiguration BaseHoverConfig { get => this.HoverConfig; }

        /// <summary>
        /// Gets or sets the hover configuration.
        /// </summary>
        public HoverConfiguration<T> HoverConfig { get; set; }

        /// <summary>
        /// Raises the hover event.
        /// </summary>
        /// <param name="elementLocation">The element location.</param>
        public void RaiseHoverEvent(Vector2 elementLocation);
    }
}

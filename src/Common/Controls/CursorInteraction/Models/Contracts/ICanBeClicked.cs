using Engine.Graphics.Models;
using Engine.Physics.Models.Contracts;
using Microsoft.Xna.Framework;

namespace Common.Controls.CursorInteraction.Models.Contracts
{
	/// <summary>
	/// Represents something that can be clicked.
	/// </summary>
	/// <typeparam name="T">The type being clicked.</typeparam>
	public interface ICanBeClicked<T> : IHaveASubArea, IHaveATypedCursorConfiguration<T>
	{
		/// <summary>
		/// Gets or sets the clickable area scaler.
		/// </summary>
		public Vector2 ClickableAreaScaler { get; set; }

		/// <summary>
		/// Gets or sets the clickable animation.
		/// </summary>
		public TriggeredAnimation ClickAnimation { get; set; }

		/// <summary>
		/// Raises the click event.
		/// </summary>
		/// <param name="cursorInteraction">The cursor interaction.</param>
		public void RaiseClickEvent(CursorInteraction<T> cursorInteraction);
    }
}

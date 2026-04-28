using Engine.Physics.Models.Contracts;
using Microsoft.Xna.Framework;

namespace Common.Controls.CursorInteractions.Models.Contracts
{
	/// <summary>
	/// Represents something that can be clicked.
	/// </summary>
	public interface ICanBeClicked : IHaveASubArea, IHaveACursorConfiguration
	{
		/// <summary>
		/// Gets or sets the clickable area scaler.
		/// </summary>
		public Vector2 ClickableAreaScaler { get; set; }

		/// <summary>
		/// Raises the click event.
		/// </summary>
		/// <param name="cursorInteraction">The cursor interaction.</param>
		public void RaiseClickEvent(CursorInteraction cursorInteraction);
    }
}

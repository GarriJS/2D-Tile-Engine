using Common.Controls.CursorInteraction.Models;
using Engine.Controls.Models;
using Engine.Controls.Models.Contracts;
using Microsoft.Xna.Framework;

namespace Common.Controls.Models.Contracts
{
	/// <summary>
	/// Represents a cursor control context Component.
	/// </summary>
	public interface IAmACursorControlContextComponent : IAmAControlContextComponent
	{
		/// <summary>
		/// Consumes the control state.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="controlState">The control state.</param>
		/// <param name="priorControlState">The prior control state.</param>
		/// <param name="hoverState">The hover state.</param>
		public void ConsumeControlState(GameTime gameTime, ControlState controlState, ControlState priorControlState, HoverState hoverState);
	}
}

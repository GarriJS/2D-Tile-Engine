using Microsoft.Xna.Framework;

namespace Engine.Controls.Models.Contracts
{
	/// <summary>
	/// Represents a control context component.
	/// </summary>
	public interface IAmAControlContextComponent
	{
		/// <summary>
		/// Consumes the control state.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="controlState">The control state.</param>
		/// <param name="priorControlState">The prior control state.</param>
		public void ConsumeControlState(GameTime gameTime, ControlState controlState, ControlState priorControlState);
	}
}

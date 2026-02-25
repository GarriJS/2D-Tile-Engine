using Engine.Controls.Models.Contracts;
using Microsoft.Xna.Framework;
using System;

namespace Engine.Controls.Models
{
	/// <summary>
	/// Represents a control component for existing to the next control context.
	/// </summary>
	public class ExistToNextControlContextComponent : IAmAControlContextComponent
	{
		/// <summary>
		/// Gets or sets the next control context.
		/// </summary>
		required public ControlContext NextControlContext { get; set; }

		/// <summary>
		/// Gets or sets the exist function.
		/// </summary>
		required public Action<GameTime, ControlState, ControlState> ExistFunction { get; set; }

		/// <summary>
		/// Gets or sets the exist condition.
		/// </summary>
		required public Func<GameTime, ControlState, ControlState, bool> ExistCondition { get; set; }

		/// <summary>
		/// Consumes the control state.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="controlState">The control state.</param>
		/// <param name="priorControlState">The prior control state.</param>
		public void ConsumeControlState(GameTime gameTime, ControlState controlState, ControlState priorControlState)
		{
			if (true != this.ExistCondition?.Invoke(gameTime, controlState, priorControlState))
				return;

			this.ExistFunction.Invoke(gameTime, controlState, priorControlState);
		}
	}
}

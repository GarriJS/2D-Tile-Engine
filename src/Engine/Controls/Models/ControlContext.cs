using Engine.Controls.Models.Contracts;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Engine.Controls.Models
{
	/// <summary>
	/// Represents a control context.
	/// </summary>
	public abstract class ControlContext
	{
		/// <summary>
		/// Gets or sets the control context components.
		/// </summary>
		public List<IAmAControlContextComponent> ControlContextComponents { get; set; } = [];

		/// <summary>
		/// Initializes the control context.
		/// </summary>
		/// <param name="gameServices">The game services.</param>
		protected ControlContext(GameServiceContainer gameServices) 
		{
			this.Initialize(gameServices);
		}

		/// <summary>
		/// Initializes the control context.
		/// </summary>
		/// <param name="gameServices">The game services.</param>
		public abstract void Initialize(GameServiceContainer gameServices);

		/// <summary>
		/// Processes the control state.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game service.</param>
		/// <param name="controlState">The control state.</param>
		/// <param name="priorControlState">The prior control state.</param>
		public abstract void ProcessControlState(GameTime gameTime, GameServiceContainer gameServices, ControlState controlState, ControlState priorControlState);
	}
}

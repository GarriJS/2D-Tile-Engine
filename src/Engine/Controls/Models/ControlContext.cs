using Engine.Controls.Models.Contracts;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Engine.Controls.Models
{
	/// <summary>
	/// Represents a control context.
	/// </summary>
	abstract public class ControlContext
	{
		readonly protected GameServiceContainer _gameServices;

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
			this._gameServices = gameServices;
			this.Initialize();
		}

		/// <summary>
		/// Initializes the control context.
		/// </summary>
		abstract public void Initialize();

		/// <summary>
		/// Processes the control state.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="controlState">The control state.</param>
		/// <param name="priorControlState">The prior control state.</param>
		abstract public void ProcessControlState(GameTime gameTime, ControlState controlState, ControlState priorControlState);
	}
}

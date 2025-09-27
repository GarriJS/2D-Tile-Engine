using Common.Controls.Cursors.Services.Contracts;
using Engine.Controls.Models;
using Microsoft.Xna.Framework;

namespace LevelEditor.Controls.Contexts
{
	/// <summary>
	/// Represents a level editor control context.
	/// </summary>
	public class LevelEditorControlContext : ControlContext
	{
		/// <summary>
		/// Initializes the level editor control context.
		/// </summary>
		/// <param name="gameServices">The game services.</param>
		public LevelEditorControlContext(GameServiceContainer gameServices) : base(gameServices) { }

		/// <summary>
		/// Initializes the control context.
		/// </summary>
		/// <param name="gameServices">The game services.</param>
		public override void Initialize(GameServiceContainer gameServices)
		{
			var cursorService = gameServices.GetService<ICursorService>();

			this.ControlContextComponents.Add(cursorService.CursorState);
		}

		/// <summary>
		/// Processes the control state.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game service.</param>
		/// <param name="controlState">The control state.</param>
		/// <param name="priorControlState">The prior control state.</param>
		public override void ProcessControlState(GameTime gameTime, GameServiceContainer gameServices, ControlState controlState, ControlState priorControlState)
		{
			foreach (var component in this.ControlContextComponents)
			{
				component.ConsumeControlState(gameTime, controlState, priorControlState);
			}
		}
	}
}

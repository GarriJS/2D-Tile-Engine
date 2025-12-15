using Common.Controls.Cursors.Services.Contracts;
using Common.Controls.Models.Contracts;
using Engine.Controls.Models;
using Microsoft.Xna.Framework;

namespace LevelEditor.Controls.Contexts
{
	/// <summary>
	/// Represents a level editor start control context.
	/// </summary>
	/// <remarks>
	/// Initializes the level editor start control context.
	/// </remarks>
	/// <param name="gameServices">The game services.</param>
	public class LevelEditorStartControlContext(GameServiceContainer gameServices) : ControlContext(gameServices)
	{
		/// <summary>
		/// Initializes the control context.
		/// </summary>
		/// <param name="gameServices">The game services.</param>
		public override void Initialize(GameServiceContainer gameServices)
		{
			var cursorService = gameServices.GetService<ICursorService>();

			this.ControlContextComponents.Add(cursorService.CursorControlComponent);
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
			var cursorService = gameServices.GetService<ICursorService>();

			var hoverState = cursorService.GetCursorHoverState();

			foreach (var component in this.ControlContextComponents)
			{
				if (component is IAmACursorControlContextComponent cursorComponent)
				{
					cursorComponent.ConsumeControlState(gameTime, controlState, priorControlState, hoverState);

					continue;
				}

				component.ConsumeControlState(gameTime, controlState, priorControlState);
			}
		}
	}
}

using Common.Controls.Cursors.Services.Contracts;
using Common.Controls.Models.Contracts;
using Engine.Controls.Models;
using LevelEditor.Scenes.Services.Contracts;
using Microsoft.Xna.Framework;

namespace LevelEditor.Controls.Contexts
{
	/// <summary>
	/// Represents a scene edit control context.
	/// </summary>
	public class SceneEditControlContext : ControlContext
	{
		/// <summary>
		/// Initializes the scene edit control context.
		/// </summary>
		/// <param name="gameServices">The game services.</param>
		public SceneEditControlContext(GameServiceContainer gameServices) : base(gameServices) { }

		/// <summary>
		/// Initializes the control context.
		/// </summary>
		/// <param name="gameServices">The game services.</param>
		public override void Initialize(GameServiceContainer gameServices)
		{
			var cursorService = gameServices.GetService<ICursorService>();
			var sceneEditService = gameServices.GetService<ISceneEditService>();

			this.ControlContextComponents.Add(cursorService.CursorControlComponent);
			this.ControlContextComponents.Add(sceneEditService.AddTileComponent);
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

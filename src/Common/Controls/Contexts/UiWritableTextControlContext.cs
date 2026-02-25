using Common.Controls.Cursors.Services.Contracts;
using Common.Controls.Models.Contracts;
using Common.UserInterface.Models.Elements;
using Engine.Controls.Models;
using Engine.Controls.Services.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Common.Controls.Contexts
{
	/// <summary>
	/// Represents a user interface writable text control context.
	/// </summary>
	/// <remarks>
	/// Initializes the user interface writable text control context.
	/// </remarks>
	/// <param name="gameServices">The game services.</param>
	public class UiWritableTextControlContext(GameServiceContainer gameServices) : ControlContext(gameServices)
	{
		/// <summary>
		/// Gets the writable text.
		/// </summary>
		public UiWritableText WritableText { get; private set; }

		/// <summary>
		/// Gets or sets the initial control context.
		/// </summary>
		private ControlContext InitialControlContext { get; set; }

		/// <summary>
		/// Sets the writable text.
		/// </summary>
		/// <param name="writableText">The writable text.</param>
		public void SetWritableText(UiWritableText writableText)
		{
			this.WritableText = writableText;
			this.ControlContextComponents.Add(this.WritableText);
		}

		/// <summary>
		/// Initializes the control context.
		/// </summary>
		public override void Initialize()
		{
			var cursorService = this._gameServices.GetService<ICursorService>();
			var controlService = this._gameServices.GetService<IControlService>();
			this.InitialControlContext = controlService.ControlContext;
			var existToNextControlContextComponent = new ExistToNextControlContextComponent
			{
				NextControlContext = controlService.ControlContext,
				ExistFunction = this.ExistFunction,
				ExistCondition = this.ExistCondition
			};
			this.ControlContextComponents.Add(existToNextControlContextComponent);
			this.ControlContextComponents.Add(cursorService.CursorControlComponent);
		}

		/// <summary>
		/// Processes the control state.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="controlState">The control state.</param>
		/// <param name="priorControlState">The prior control state.</param>
		public override void ProcessControlState(GameTime gameTime, ControlState controlState, ControlState priorControlState)
		{
			var cursorService = this._gameServices.GetService<ICursorService>();
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

		/// <summary>
		/// The exist function.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="controlState">The control state.</param>
		/// <param name="priorControlState">The prior control state.</param>
		private void ExistFunction(GameTime gameTime, ControlState controlState, ControlState priorControlState)
		{
			var controlService = this._gameServices.GetService<IControlService>();
			controlService.SetControlContext(this.InitialControlContext);
			this.WritableText.Active = false;
		}

		/// <summary>
		/// Exists the condition.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="controlState">The control state.</param>
		/// <param name="priorControlState">The prior control state.</param>
		private bool ExistCondition(GameTime gameTime, ControlState controlState, ControlState priorControlState)
		{
			var result = controlState.PressedKeys.Contains(Keys.OemTilde);

			return result;
		}
	}
}
using Engine.Controls.Models;
using Engine.Controls.Models.Enums;
using Engine.Controls.Services.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Controls.Services
{
	/// <summary>
	/// Represents a control manager.
	/// </summary>
	/// <remarks>
	/// Initializes a new instance of the control manager.
	/// </remarks>
	/// <param name="game">The game.</param>
	public class ControlManager(Game game) : GameComponent(game), IControlService
	{
		/// <summary>
		/// Gets or sets the action controls.
		/// </summary>
		private List<ActionControl> ActionControls { get; set; }

		/// <summary>
		/// Gets or sets the old control state.
		/// </summary>
		private ControlState OldControlState { get; set; }

		/// <summary>
		/// Gets or sets the control state.
		/// </summary>
		public ControlState ControlState { get; private set; }

		/// <summary>
		/// Initializes the control manager.
		/// </summary>
		public override void Initialize()
		{
			var actionControlServices = this.Game.Services.GetService<IActionControlServices>();
			this.ActionControls = actionControlServices.GetActionControls();
			this.ControlState = new ControlState
			{
				Direction = null,
				MouseState = default,
				FreshActionTypes = [],
				ActionTypes = []
			};

			base.Initialize();
		}

		/// <summary>
		/// Updates the control manager.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		public override void Update(GameTime gameTime)
		{
			var mouseService = this.Game.Services.GetService<IMouseService>();
			
			this.OldControlState = this.ControlState;
			this.ControlState = this.GetCurrentControlState();

			mouseService.ProcessMouseState(this.ControlState);

			base.Update(gameTime);
		}

		/// <summary>
		/// Gets the current control state.
		/// </summary>
		/// <returns>The control state.</returns>
		private ControlState GetCurrentControlState()
		{
			var pressedKeys = Keyboard.GetState().GetPressedKeys();
			var mouseState = Mouse.GetState();
			var pressedMouseButtons = this.GetPressedMouseButtons(Mouse.GetState());
			var activeActionTypes = this.ActionControls.Where(e => (true == pressedKeys.Any(k => true == e.ControlKeys?.Contains(k))) ||
																   (true == pressedMouseButtons.Any(m => true == e.ControlMouseButtons?.Contains(m))))
													   .Select(e => e.ActionType)
													   .ToList();

			var direction = this.GetMovementDirection(activeActionTypes);
			var freshActionTypes = activeActionTypes;

			if (null != this.OldControlState?.ActionTypes)
			{
				freshActionTypes = activeActionTypes.Where(e => false == this.OldControlState.ActionTypes.Contains(e))
												    .ToList();
			}

			return new ControlState
			{
				Direction = direction,
				MouseState = mouseState,
				FreshActionTypes = freshActionTypes,
				ActionTypes = activeActionTypes
			};
		}

		/// <summary>
		/// Gets the pressed mouse buttons.
		/// </summary>
		/// <param name="mouseState">The mouse state.</param>
		/// <returns>The mouse buttons.</returns>
		private MouseButtonTypes[] GetPressedMouseButtons(MouseState mouseState)
		{
			var activeMouseButtons = new List<MouseButtonTypes>(5);

			if (ButtonState.Pressed == mouseState.LeftButton)
			{
				activeMouseButtons.Add(MouseButtonTypes.LeftButton);				
			}

			if (ButtonState.Pressed == mouseState.RightButton)
			{
				activeMouseButtons.Add(MouseButtonTypes.RightButton);
			}

			if (ButtonState.Pressed == mouseState.MiddleButton)
			{
				activeMouseButtons.Add(MouseButtonTypes.MiddleButton);
			}

			if (ButtonState.Pressed == mouseState.XButton1)
			{
				activeMouseButtons.Add(MouseButtonTypes.XButton1);
			}

			if (ButtonState.Pressed == mouseState.XButton2)
			{
				activeMouseButtons.Add(MouseButtonTypes.XButton2);
			}

			return activeMouseButtons.ToArray();
		}

		/// <summary>
		/// Gets the movement direction.
		/// </summary>
		/// <param name="actionTypes">The action types.</param>
		/// <returns>The movement direction.</returns>
		private float? GetMovementDirection(List<ActionTypes> actionTypes)
		{
			bool upMovement = actionTypes.Contains(ActionTypes.Up);
			bool downMovement = actionTypes.Contains(ActionTypes.Down);
			bool leftMovement = actionTypes.Contains(ActionTypes.Left);
			bool rightMovement = actionTypes.Contains(ActionTypes.Right);

			if ((true == upMovement) && 
				(true == downMovement))
			{
				upMovement = false;
				downMovement = false;
			}

			if ((true == leftMovement) && 
				(true == rightMovement))
			{
				leftMovement = false;
				rightMovement = false;
			}

			if (true == upMovement)
			{
				if (true == leftMovement)
				{
					return (float)(3 * Math.PI) / 4f;
				}
				else if (true == rightMovement)
				{
					return (float)Math.PI / 4f;
				}
				else
				{
					return (float)Math.PI / 2f;
				}
			}
			else if (true == downMovement)
			{
				if (true == leftMovement)
				{
					return (float)(5 * Math.PI) / 4f;
				}
				else if (true == rightMovement)
				{
					return (float)(7 * Math.PI) / 4f;
				}
				else
				{
					return (float)(3 * Math.PI) / 2f;
				}
			}
			else if (true == leftMovement)
			{
				return (float)Math.PI;
			}
			else if (true == rightMovement)
			{
				return 0f;
			}
			else
			{
				return null;
			}
		}
	}
}

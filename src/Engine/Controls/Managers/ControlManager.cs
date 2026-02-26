using Engine.Controls.Enums;
using Engine.Controls.Models;
using Engine.Controls.Services.Contracts;
using Engine.Physics.Models;
using Engine.RunTime.Constants;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Controls.Managers
{
    /// <summary>
    /// Represents a control manager.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the control manager.
    /// </remarks>
    /// <param name="game">The game.</param>
    sealed public class ControlManager(Game game) : GameComponent(game), IControlService
    {
        /// <summary>
        /// Gets or sets the action control configurations.
        /// </summary>
        private List<ActionControlConfiguration> ActionControlConfigurations { get; set; }

        /// <summary>
        /// Gets or sets the control context.
        /// </summary>
        public ControlContext ControlContext { get; private set; }

        /// <summary>
        /// Gets or sets the prior control state.
        /// </summary>
        public ControlState PriorControlState { get; private set; }

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
			this.UpdateOrder = ManagerOrderConstants.ControlManagerUpdateOrder;
			this.ActionControlConfigurations = actionControlServices.GetActionControls();
			this.ControlState = new ControlState
            {
                Direction = null,
                MouseState = default,
                MouseVerticalScrollDelta = default,
                FreshPressedKeys = [],
                PressedKeys = [],
                FreshActionControls = [],
                ActiveActionControls = []
            };
            base.Initialize();
        }

        /// <summary>
        /// Sets the control context.
        /// </summary>
        /// <param name="controlContext">The control context.</param>
        public void SetControlContext(ControlContext controlContext)
        {
            this.ControlContext = controlContext;
        }

        /// <summary>
        /// Updates the control manager.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        public override void Update(GameTime gameTime)
        {
            this.PriorControlState = this.ControlState;
            this.ControlState = this.GetCurrentControlState(gameTime);
            this.ControlContext?.ProcessControlState(gameTime, this.ControlState, this.PriorControlState);

            base.Update(gameTime);
        }

		/// <summary>
		/// Gets the current control state.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <returns>The control state.</returns>
		private ControlState GetCurrentControlState(GameTime gameTime)
        {
            //TODO reduce lingQ usage 
            var activeKeys = Keyboard.GetState()
                                     .GetPressedKeys()
									 .ToList();
            var mouseState = Mouse.GetState();
            var pressedMouseButtons = GetPressedMouseButtons(mouseState);
            var mouseVerticalScrollDelta = GetMouseVerticalScrollDelta(mouseState, this.PriorControlState.MouseState);
            var activeActionControls = new List<ElaspedTimeExtender<ActionControlConfiguration>>();
			var pressedKeys = new List<ElaspedTimeExtender<Keys>>();

			foreach (var priorActiveActionControl in this.PriorControlState.ActiveActionControls)
            {
                if ((false == activeKeys.Any(k => true == priorActiveActionControl.Subject.ControlKeys?.Contains(k))) &&
                    (false == pressedMouseButtons.Any(m => true == priorActiveActionControl.Subject.ControlMouseButtons?.Contains(m))))
                    continue;

                var activeActionControl = new ElaspedTimeExtender<ActionControlConfiguration>()
                {
                    ElaspedTime = priorActiveActionControl.ElaspedTime + gameTime.ElapsedGameTime.TotalMilliseconds,
                    Subject = priorActiveActionControl.Subject
                };
				activeActionControls.Add(activeActionControl);
			}

			var freshActionControls = this.ActionControlConfigurations.Where(e => (false == activeActionControls.Any(a => a.Subject == e)) &&
                                                                                  ((true == activeKeys.Any(k => true == e.ControlKeys?.Contains(k))) ||
                                                                                   (true == pressedMouseButtons.Any(m => true == e.ControlMouseButtons?.Contains(m)))))
                                                                      .ToList();
            activeActionControls.AddRange(freshActionControls.Select(e => new ElaspedTimeExtender<ActionControlConfiguration>
            {
                ElaspedTime = 0,
                Subject = e,
            }));

            foreach (var priorPressedKey in this.PriorControlState.PressedKeys)
            {
                if (false == activeKeys.Contains(priorPressedKey.Subject))
                    continue;

				var activeActionControl = new ElaspedTimeExtender<Keys>()
				{
					ElaspedTime = priorPressedKey.ElaspedTime + gameTime.ElapsedGameTime.TotalMilliseconds,
					Subject = priorPressedKey.Subject
				};
				pressedKeys.Add(activeActionControl);
			}

            var freshPressedKeys = activeKeys.Where(e => false == pressedKeys.Any(k => k.Subject == e))
                                             .ToList();
            pressedKeys.AddRange(freshPressedKeys.Select(e => new ElaspedTimeExtender<Keys>
            {
                ElaspedTime = 0,
                Subject = e
            }));

            var result = new ControlState
            {
                Direction = null,
                MouseState = mouseState,
                MouseVerticalScrollDelta = mouseVerticalScrollDelta,
                FreshPressedKeys = freshPressedKeys,
                PressedKeys = pressedKeys,
				FreshActionControls = freshActionControls,
				ActiveActionControls = activeActionControls
			};

            return result;
        }

        /// <summary>
        /// Gets the pressed mouse buttons.
        /// </summary>
        /// <param name="mouseState">The mouse state.</param>
        /// <returns>The mouse buttons.</returns>
        static private MouseButtonTypes[] GetPressedMouseButtons(MouseState mouseState)
        {
            var activeMouseButtons = new List<MouseButtonTypes>(5);

            if (ButtonState.Pressed == mouseState.LeftButton)
                activeMouseButtons.Add(MouseButtonTypes.LeftButton);

            if (ButtonState.Pressed == mouseState.RightButton)
                activeMouseButtons.Add(MouseButtonTypes.RightButton);

            if (ButtonState.Pressed == mouseState.MiddleButton)
                activeMouseButtons.Add(MouseButtonTypes.MiddleButton);

            if (ButtonState.Pressed == mouseState.XButton1)
                activeMouseButtons.Add(MouseButtonTypes.XButton1);

            if (ButtonState.Pressed == mouseState.XButton2)
                activeMouseButtons.Add(MouseButtonTypes.XButton2);

            return [.. activeMouseButtons];
        }

		/// <summary>
		/// Gets the mouse vertical scroll delta.
		/// </summary>
        /// <param name="newState">The new state.</param>
		/// <param name="oldState">The old state.</param>
		/// <returns>The vertical scroll delta.</returns>
		static private float GetMouseVerticalScrollDelta(MouseState newState, MouseState oldState)
		{
			var delta = newState.ScrollWheelValue - oldState.ScrollWheelValue;
			var result = delta / 120f; //normalize for 120 monogame scroll wheel notches.

			return result;
		}

		/// <summary>
		/// Gets the movement direction.
		/// </summary>
		/// <param name="actionTypes">The action types.</param>
		/// <returns>The movement direction.</returns>
		//     private float? GetMovementDirection(List<int> actionTypes)
		//     {
		//         var upMovement = actionTypes.Contains(ActionTypes.Up);
		//var downMovement = actionTypes.Contains(ActionTypes.Down);
		//var leftMovement = actionTypes.Contains(ActionTypes.Left);
		//var rightMovement = actionTypes.Contains(ActionTypes.Right);

		//         if ((true == upMovement) &&
		//             (true == downMovement))
		//         {
		//             upMovement = false;
		//             downMovement = false;
		//         }

		//         if ((true == leftMovement) &&
		//             (true == rightMovement))
		//         {
		//             leftMovement = false;
		//             rightMovement = false;
		//         }

		//         if (true == upMovement)
		//         {
		//             if (true == leftMovement)
		//             {
		//                 return (float)(3 * Math.PI) / 4f;
		//             }
		//             else if (true == rightMovement)
		//             {
		//                 return (float)Math.PI / 4f;
		//             }
		//             else
		//             {
		//                 return (float)Math.PI / 2f;
		//             }
		//         }
		//         else if (true == downMovement)
		//         {
		//             if (true == leftMovement)
		//             {
		//                 return (float)(5 * Math.PI) / 4f;
		//             }
		//             else if (true == rightMovement)
		//             {
		//                 return (float)(7 * Math.PI) / 4f;
		//             }
		//             else
		//             {
		//                 return (float)(3 * Math.PI) / 2f;
		//             }
		//         }
		//         else if (true == leftMovement)
		//         {
		//             return (float)Math.PI;
		//         }
		//         else if (true == rightMovement)
		//         {
		//             return 0f;
		//         }
		//         else
		//         {
		//             return null;
		//         }
		//     }
	}
}

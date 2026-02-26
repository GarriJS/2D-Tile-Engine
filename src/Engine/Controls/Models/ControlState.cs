using Engine.Physics.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Controls.Models
{
	/// <summary>
	/// Represents a control state.
	/// </summary>
	sealed public class ControlState
	{
		/// <summary>
		/// Gets or sets the direction.
		/// </summary>
		required public float? Direction { get; set; }

		/// <summary>
		/// Gets the mouse position.
		/// </summary>
		public Vector2 MousePosition { get => this.MouseState.Position.ToVector2(); }

		/// <summary>
		/// Gets or sets the mouse state.
		/// </summary>
		required public MouseState MouseState { get; set; }

		/// <summary>
		/// Gets or sets the mouse vertical scroll delta.
		/// </summary>
		required public float MouseVerticalScrollDelta { get; set; }

		/// <summary>
		/// Gets or sets the fresh pressed keys.
		/// </summary>
		required public List<Keys> FreshPressedKeys { get; set; }

		/// <summary>
		/// Gets or sets the pressed keys.
		/// </summary>
		required public List<ElaspedTimeExtender<Keys>> PressedKeys { get; set; }

		/// <summary>
		/// Gets or sets the fresh action names. 
		/// </summary>
		required public List<ActionControlConfiguration> FreshActionControls { get; set; }

		/// <summary>
		/// Gets or sets the active action names.
		/// </summary>
		required public List<ElaspedTimeExtender<ActionControlConfiguration>> ActiveActionControls { get; set; }

		/// <summary>
		/// Determines if the key is fresh.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>A value indicating whether the key is fresh.</returns>
		public bool KeyIsFresh(Keys key)
		{ 
			var result = true == this.FreshPressedKeys?.Contains(key);

			return result;
		}

		/// <summary>
		/// Determines if the key is active.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <returns>A value indicating whether the key is active.</returns>
		public bool KeyIsActive(Keys key)
		{
			var result = true == this.PressedKeys?.Any(e => e.Subject == key);

			return result;
		}

		/// <summary>
		/// Determines if the key is active for at least the given amount of time.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="time">The time in milliseconds.</param>
		/// <returns>A value indicating whether the key has been active for the given amount of time.</returns>
		public bool KeyIsActiveForGiveTime(Keys key, double time)
		{ 
			var result = true == this.PressedKeys?.Any(e => (e.Subject == key) &&
															(e.ElaspedTime >= time));

			return result;
		}

		/// <summary>
		/// Determines if the action name is fresh.
		/// </summary>
		/// <param name="actionName">The action name.</param>
		/// <returns>A value indicating whether the action name is fresh.</returns>
		public bool ActionNameIsFresh(string actionName)
		{ 
			var result = true == this.FreshActionControls?.Any(e => e.ActionName == actionName);

			return result;
		}

		/// <summary>
		/// Determines if the action name is active.
		/// </summary>
		/// <param name="actionName">The action name.</param>
		/// <returns>A value indicating whether the action name is active.</returns>
		public bool ActionNameIsActive(string actionName)
		{
			var result = true == this.ActiveActionControls?.Any(e => e.Subject.ActionName == actionName);

			return result;
		}

		/// <summary>
		/// Determines if the action name is active for at least the given amount of time.
		/// </summary>
		/// <param name="actionName">The action name.</param>
		/// <param name="time">The time in milliseconds.</param>
		/// <returns>A value indicating whether the action name has been active for the given amount of time.</returns>
		public bool ActionNameIsActiveForGiveTime(string actionName, double time)
		{
			var result = true == this.ActiveActionControls?.Any(e => (e.Subject.ActionName == actionName) &&
																	 (e.ElaspedTime >= time));

			return result;
		}
	}
}

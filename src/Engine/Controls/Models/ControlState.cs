using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

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
		/// Gets or sets the fresh action names. 
		/// </summary>
		required public List<string> FreshActionNames { get; set; }

		/// <summary>
		/// Gets or sets the active action names.
		/// </summary>
		required public List<string> ActiveActionNames { get; set; }

		/// <summary>
		/// Determines if the action name is fresh.
		/// </summary>
		/// <param name="actionName">The action name.</param>
		/// <returns>A value indicating whether the action name is fresh.</returns>
		public bool ActionNameIsFresh(string actionName)
		{ 
			var result = true == this.FreshActionNames?.Contains(actionName);

			return result;
		}

		/// <summary>
		/// Determines if the action name is active.
		/// </summary>
		/// <param name="actionName">The action name.</param>
		/// <returns>A value indicating whether the action name is active.</returns>
		public bool ActionNameIsActive(string actionName)
		{
			var result = true == this.ActiveActionNames?.Contains(actionName);

			return result;
		}
	}
}

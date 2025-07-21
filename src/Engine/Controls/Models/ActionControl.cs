using Engine.Controls.Enums;
using Microsoft.Xna.Framework.Input;

namespace Engine.Controls.Models
{
    /// <summary>
    /// Represents a action control.
    /// </summary>
    public class ActionControl
	{
		/// <summary>
		/// Gets or sets the action control description.
		/// </summary>
		public string ActionControlDescription { get; set; }

		/// <summary>
		/// Gets or sets the action type.
		/// </summary>
		public ActionTypes ActionType { get; set; } 

		/// <summary>
		/// Gets or sets the control keys.
		/// </summary>
		public Keys[] ControlKeys { get; set; }

		/// <summary>
		/// Gets or sets the control mouse buttons.
		/// </summary>
		public MouseButtonTypes[] ControlMouseButtons { get; set; }
	}
}

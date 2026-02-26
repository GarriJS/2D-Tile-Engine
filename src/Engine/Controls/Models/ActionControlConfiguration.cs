using Engine.Controls.Enums;
using Microsoft.Xna.Framework.Input;

namespace Engine.Controls.Models
{
    /// <summary>
    /// Represents a action control configuration.
    /// </summary>
    public class ActionControlConfiguration
	{
		/// <summary>
		/// Gets or sets the action name.
		/// </summary>
		required public string ActionName { get; set; }

		/// <summary>
		/// Gets or sets the control keys.
		/// </summary>
		required public Keys[] ControlKeys { get; set; }

		/// <summary>
		/// Gets or sets the control mouse buttons.
		/// </summary>
		required public MouseButtonTypes[] ControlMouseButtons { get; set; }
	}
}

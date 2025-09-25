using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Engine.Controls.Models
{
	/// <summary>
	/// Represents a control state.
	/// </summary>
	public class ControlState
	{
		/// <summary>
		/// Gets or sets the direction.
		/// </summary>
		public float? Direction { get; set; }

		/// <summary>
		/// Gets the mouse position.
		/// </summary>
		public Vector2 MousePosition { get => this.MouseState.Position.ToVector2(); }

		/// <summary>
		/// Gets or sets the mouse state.
		/// </summary>
		public MouseState MouseState { get; set; }

		/// <summary>
		/// Gets or sets the fresh action names. 
		/// </summary>
		public List<string> FreshActionNames { get; set; }

		/// <summary>
		/// Gets or sets the active action names.
		/// </summary>
		public List<string> ActiveActionNames { get; set; }
	}
}

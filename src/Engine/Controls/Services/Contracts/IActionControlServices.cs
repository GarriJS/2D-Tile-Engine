using Engine.Controls.Models;
using System.Collections.Generic;

namespace Engine.Controls.Services.Contracts
{
	/// <summary>
	/// Represents a action control service.
	/// </summary>
	public interface IActionControlServices
	{
		/// <summary>
		/// Gets the action controls.
		/// </summary>
		public List<ActionControl> GetActionControls();
	}
}

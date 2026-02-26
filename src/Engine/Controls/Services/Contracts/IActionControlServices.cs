using Engine.Controls.Models;
using Engine.DiskModels.Controls;
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
		public List<ActionControlConfiguration> GetActionControls();

		/// <summary>
		/// Gets the action control from the model.
		/// </summary>
		/// <param name="actionControlModel">The action control model.</param>
		/// <returns>The action control.</returns>
		public ActionControlConfiguration GetActionControlFromModel(ActionControlModel actionControlModel);
	}
}

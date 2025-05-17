using Engine.Controls.Models;

namespace Engine.Controls.Services.Contracts
{
	/// <summary>
	/// Represents a mouse service.
	/// </summary>
	public interface IMouseService
	{
		/// <summary>
		/// Processes the mouse state.
		/// </summary>
		/// <param name="controlState">The control state.</param>
		public void ProcessMouseState(ControlState controlState = null);
	}
}

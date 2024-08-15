using Engine.Controls.Models;

namespace Engine.Controls.Services.Contracts
{
	public interface IControlService
	{
		/// <summary>
		/// Gets or sets the control state.
		/// </summary>
		public ControlState ControlState { get; }
	}
}

using Engine.Controls.Models;

namespace Engine.Controls.Services.Contracts
{
	/// <summary>
	/// Represents a control service.
	/// </summary>
	public interface IControlService
	{
		/// <summary>
		/// Gets or sets the control context.
		/// </summary>
		public ControlContext ControlContext { get; }

		/// <summary>
		/// Gets or sets the prior control state.
		/// </summary>
		public ControlState PriorControlState { get; }

		/// <summary>
		/// Gets or sets the control state.
		/// </summary>
		public ControlState ControlState { get; }

		/// <summary>
		/// Sets the control context.
		/// </summary>
		/// <param name="controlContext">The control context.</param>
		public void SetControlContext(ControlContext controlContext);
	}
}

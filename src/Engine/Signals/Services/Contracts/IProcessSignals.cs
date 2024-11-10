using Engine.Signals.Models;
using Engine.Signals.Models.Contracts;

namespace Engine.Signals.Services.Contracts
{
	/// <summary>
	/// Represents something that processes signals.
	/// </summary>
	public interface IProcessSignals
	{
		/// <summary>
		/// Processes the signal.
		/// </summary>
		/// <param name="receiver">The receiver.</param>
		/// <param name="signal">The signal.</param>
		/// <param name="allowSignalResponses">A value indicating whether to allow for signal responses or not.</param>
		public void ProcessSignal(IReceiveSignals receiver, Signal signal, bool allowSignalResponses = true);
	}
}

using Engine.Signals.Models;
using Engine.Signals.Models.Contracts;

namespace Engine.Signals.Services.Contracts
{
	/// <summary>
	/// Represents a signal service.
	/// </summary>
	public interface ISignalService
	{
		/// <summary>
		/// Adds the signals.
		/// </summary>
		/// <param name="signal">The signal.</param>
		public void AddSignal(Signal signal);

		/// <summary>
		/// Adds the subscription.
		/// </summary>
		/// <param name="signalType">The signal type.</param>
		/// <param name="subscription">The subscription.</param>
		public void SubscribeToSignalType(string signalType, SignalSubscription subscription);

		/// <summary>
		/// Removes the subscription.
		/// </summary>
		/// <param name="signalType">The signal type.</param>
		/// <param name="subscription">The subscription.</param>
		public void UnsubscribeToSignalType(string signalType, SignalSubscription subscription);

		/// <summary>
		/// Changes the subscription.
		/// </summary>
		/// <param name="signalType">The signal type.</param>
		/// <param name="subscription">The subscription.</param>
		public void ChangeSubscription(string signalType, SignalSubscription subscription);

		/// <summary>
		/// Processes the signal responses.
		/// </summary>
		/// <param name="emitter"></param>
		public void ProcessSignalResponses(IEmitSignals emitter);
	}
}

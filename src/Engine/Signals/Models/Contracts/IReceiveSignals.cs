using System.Collections.Generic;

namespace Engine.Signals.Models.Contracts
{
	/// <summary>
	/// Represents something that can receive signals.
	/// </summary>
	public interface IReceiveSignals
	{
		/// <summary>
		/// Gets the active signal subscriptions.
		/// </summary>
		public IList<SignalSubscription> ActiveSignalSubscriptions { get; set; }
	}
}

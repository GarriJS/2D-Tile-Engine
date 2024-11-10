using Engine.Signals.Models.Contracts;
using System;

namespace Engine.Signals.Models
{
	/// <summary>
	/// Represents a signal subscriptions.
	/// </summary>
	public class SignalSubscription
	{
		/// <summary>
		/// Gets or sets the subscriber.
		/// </summary>
		public IReceiveSignals Subscriber { get; set; }

		/// <summary>
		/// Gets or sets the signal processor type.
		/// </summary>
		public Type SignalProcessorType { get; set; }
	}
}

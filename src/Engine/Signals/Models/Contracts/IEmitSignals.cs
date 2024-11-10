using Engine.RunTime.Models.Contracts;
using System;
using System.Collections.Generic;

namespace Engine.Signals.Models.Contracts
{
	/// <summary>
	/// Represents something that emits signals.
	/// </summary>
	public interface IEmitSignals : ICanBeUpdated
	{
		/// <summary>
		/// Gets or sets the signal response processor type.
		/// </summary>
		public Type SignalResponseProcessorType { get; set; }

		/// <summary>
		/// Gets the queue of received signals.
		/// </summary>
		public Queue<Signal> SignalResponses { get; set; }
	}
}

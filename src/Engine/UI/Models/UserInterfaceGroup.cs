using Engine.Signals.Models;
using Engine.Signals.Models.Contracts;
using System;
using System.Collections.Generic;

namespace Engine.UI.Models
{
	/// <summary>
	/// Represents a user interface group.
	/// </summary>
	public class UserInterfaceGroup : IEmitSignals, IReceiveSignals
	{
		/// <summary>
		/// Gets or sets the visibility group id.
		/// </summary>
		public int? VisibilityGroupId { get; set; }

		/// <summary>
		/// Gets or sets the user interface elements.
		/// </summary>
		public IList<UserInterfaceElement> UserInterfaceElements { get; set; }

		/// <summary>
		/// Gets or sets the signal response processor type.
		/// </summary>
		public Type SignalResponseProcessorType { get; set; }

		/// <summary>
		/// Gets the queue of received signals.
		/// </summary>
		public Queue<Signal> SignalResponses { get; set; }

		/// <summary>
		/// Gets the active signal subscriptions.
		/// </summary>
		public IList<SignalSubscription> ActiveSignalSubscriptions { get; set; }
	}
}

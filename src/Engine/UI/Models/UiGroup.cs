using Engine.Signals.Models;
using Engine.Signals.Models.Contracts;
using System.Collections.Generic;

namespace Engine.UI.Models
{
	/// <summary>
	/// Represents a user interface group.
	/// </summary>
	public class UiGroup : IReceiveSignals
	{
		/// <summary>
		/// Gets or sets the visibility group id.
		/// </summary>
		public int? VisibilityGroupId { get; set; }

		/// <summary>
		/// Gets or sets the user interface elements.
		/// </summary>
		public IList<UiZoneElement> UiZoneElements { get; set; }

		/// <summary>
		/// Gets the active signal subscriptions.
		/// </summary>
		public IList<SignalSubscription> ActiveSignalSubscriptions { get; set; }
	}
}

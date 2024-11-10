using Engine.Signals.Models.Contracts;
using Engine.Signals.Services.Contracts;

namespace Engine.Signals.Models
{
	/// <summary>
	/// Represents a signal. 
	/// </summary>
	public class Signal
	{
		/// <summary>
		/// Gets or sets the signal type.
		/// </summaryr>
		public string SignalType { get; set; }

		/// <summary>
		/// Gets or sets the actor.
		/// </summary>
		public IEmitSignals Actor { get; set; }

		/// <summary>
		/// Gets or sets the param.
		/// </summary>
		public object Param { get; set; }
	}
}

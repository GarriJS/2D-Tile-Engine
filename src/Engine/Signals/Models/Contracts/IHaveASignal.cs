namespace Engine.Signals.Models.Contracts
{
	/// <summary>
	/// Represents something with a signal.
	/// </summary>
	public interface IHaveASignal
	{
		/// <summary>
		/// Gets or sets the signal.
		/// </summary>
		public Signal Signal { get; set; }
	}
}

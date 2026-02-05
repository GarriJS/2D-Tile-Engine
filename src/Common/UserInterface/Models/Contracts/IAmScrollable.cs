using Engine.RunTime.Models.Contracts;

namespace Common.UserInterface.Models.Contracts
{
	/// <summary>
	/// Represents something that is scrollable.
	/// </summary>
	public interface IAmScrollable
	{
		/// <summary>
		/// Gets the scroll state.
		/// </summary>
		public ScrollState ScrollState { get; }
	}
}

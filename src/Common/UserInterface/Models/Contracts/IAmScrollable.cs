using Engine.RunTime.Models.Contracts;

namespace Common.UserInterface.Models.Contracts
{
	/// <summary>
	/// Represents something that is scrollable.
	/// </summary>
	public interface IAmScrollable : IRequirePreRender
	{
		/// <summary>
		/// Gets a value indicating whether to disable scrolling.
		/// </summary>
		public bool DisableScrolling { get; }

		/// <summary>
		/// Gets the scroll state.
		/// </summary>
		public ScrollState ScrollState { get; }
	}
}

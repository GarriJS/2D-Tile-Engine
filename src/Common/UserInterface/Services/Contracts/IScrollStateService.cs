using Common.DiskModels.UserInterface;
using Common.UserInterface.Models;

namespace Common.UserInterface.Services.Contracts
{
	/// <summary>
	/// Represents a scroll state service.
	/// </summary>
	public interface IScrollStateService
	{
		/// <summary>
		/// Gets the scroll state from the model.
		/// </summary>
		/// <param name="scrollStateModel">The scroll state model.</param>
		/// <returns>The scroll state.</returns>
		public ScrollState GetScrollStateFromModel(ScrollStateModel scrollStateModel);
	}
}

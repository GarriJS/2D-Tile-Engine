using Common.DiskModels.UserInterface;
using Common.UserInterface.Models;
using Common.UserInterface.Services.Contracts;
using Microsoft.Xna.Framework;

namespace Common.UserInterface.Services
{
	/// <summary>
	/// Represents a scroll state service.
	/// </summary>
	/// <remarks>
	/// Initializes the scroll state service.
	/// </remarks>
	/// <param name="gameServices">The game services.</param>
	public class ScrollStateService(GameServiceContainer gameServices) : IScrollStateService
	{
		private readonly GameServiceContainer _gameServices = gameServices;

		/// <summary>
		/// Gets the scroll state from the model.
		/// </summary>
		/// <param name="scrollStateModel">The scroll state model.</param>
		/// <returns>The scroll state.</returns>
		public ScrollState GetScrollStateFromModel(ScrollStateModel scrollStateModel)
		{
			if (null == scrollStateModel)
				return null;

			var result = new ScrollState
			{
				DisableScrolling = scrollStateModel.DisableScrolling,
				DrawScrollWheel = scrollStateModel.DrawScrollWheel,
				VerticalScrollOffset = scrollStateModel.VerticalScrollOffset,
				ScrollSpeed = scrollStateModel.ScrollSpeed,
				MaxVisibleHeight = scrollStateModel.MaxVisibleHeight,
				ScrollBarWidth = scrollStateModel.ScrollBarWidth,
				ScrollStateJustificationType = scrollStateModel.ScrollStateJustificationType,
				ScrollBackgroundColor = scrollStateModel.ScrollBackgroundColor,
				ScrollNotchColor = scrollStateModel.ScrollNotchColor
			};

			return result;
		}
	}
}

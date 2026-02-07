using Common.DiskModels.UserInterface;
using Common.UserInterface.Models;
using Common.UserInterface.Services.Contracts;
using Microsoft.Xna.Framework;

namespace Common.UserInterface.Services
{
	/// <summary>
	/// Represents a user interface margin service.
	/// </summary>
	/// <remarks>
	/// Initializes the user interface margin service.
	/// </remarks>
	/// <param name="gameServices">The game service.</param>
	sealed public class UserInterfaceMarginService(GameServiceContainer gameServices) : IUserInterfaceMarginService
	{
		readonly private GameServiceContainer _gameServices = gameServices;

		/// <summary>
		/// Gets the user interface margin from the model.
		/// </summary>
		/// <param name="model">The user interface margin model.</param>
		/// <returns>The user interface margin.</returns>
		public UiMargin GetUiMarginFromModel(UiMarginModel model)
		{
			if (model is null)
				return default;

			var result = new UiMargin
			{
				TopMargin = model.TopMargin,
				BottomMargin = model.BottomMargin,
				LeftMargin = model.LeftMargin,
				RightMargin = model.RightMargin,
			};

			return result;
		}
	}
}

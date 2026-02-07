using Common.DiskModels.UserInterface;
using Common.UserInterface.Models;

namespace Common.UserInterface.Services.Contracts
{
	/// <summary>
	/// Represents a user interface margin service.
	/// </summary>
	public interface IUiMarginService
	{
		/// <summary>
		/// Gets the user interface margin from the model.
		/// </summary>
		/// <param name="model">The user interface margin model.</param>
		/// <returns>The user interface margin.</returns>
		public UiMargin GetUiMarginFromModel(UiMarginModel model);
	}
}

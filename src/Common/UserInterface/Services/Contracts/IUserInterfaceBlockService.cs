using Common.DiskModels.UserInterface;
using Common.UserInterface.Models;

namespace Common.UserInterface.Services.Contracts
{
	/// <summary>
	/// Represents a user interface block service.
	/// </summary>
	public interface IUserInterfaceBlockService
	{
		/// <summary>
		/// Get the user interface block from the model.
		/// </summary>
		/// <param name="uiBlockModel">The user interface block model.</param>
		/// <returns>The user interface block.</returns>
		public UiBlock GetUiBlockFromModel(UiBlockModel uiBlockModel);
	}
}

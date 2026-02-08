using Common.DiskModels.UserInterface;
using Common.UserInterface.Models;
using Engine.Physics.Models.SubAreas;

namespace Common.UserInterface.Services.Contracts
{
	/// <summary>
	/// Represents a user interface block service.
	/// </summary>
	public interface IUiBlockService
	{
		/// <summary>
		/// Get the user interface block from the model.
		/// </summary>
		/// <param name="uiBlockModel">The user interface block model.</param>
		/// <param name="outterArea">The encapsulating area of the block.</param>
		/// <returns>The user interface block.</returns>
		public UiBlock GetUiBlockFromModel(UiBlockModel uiBlockModel, SubArea outterArea = null);
	}
}

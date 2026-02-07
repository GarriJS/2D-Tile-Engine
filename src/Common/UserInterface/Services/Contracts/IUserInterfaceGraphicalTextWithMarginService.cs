using Common.DiskModels.UserInterface;
using Common.UserInterface.Models;

namespace Common.UserInterface.Services.Contracts
{
	/// <summary>
	/// Represents a user interface graphic text with margin service.
	/// </summary>
	public interface IUserInterfaceGraphicalTextWithMarginService
	{
		/// <summary>
		/// Gets the graphical text with margin from the model.
		/// </summary>
		/// <param name="model">The graphical text with margin.</param>
		/// <returns>The graphical text with margin.</returns>
		public GraphicalTextWithMargin GetGraphicTextWithMarginFromModel(GraphicalTextWithMarginModel model);
	}
}

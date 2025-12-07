using Common.DiskModels.UserInterface;
using Common.DiskModels.UserInterface.Contracts;
using Common.UserInterface.Models;
using Common.UserInterface.Models.Contracts;

namespace Common.UserInterface.Services.Contracts
{
	/// <summary>
	/// Represents a user interface element service.
	/// </summary>
	public interface IUserInterfaceElementService
	{
		/// <summary>
		/// Gets the user interface margin from the model.
		/// </summary>
		/// <param name="model">The user interface margin model.</param>
		/// <returns>The user interface margin model.</returns>
		public UiMargin GetUiMarginFromModel(UiMarginModel model);

		/// <summary>
		/// Gets the graphical text with margin from the model.
		/// </summary>
		/// <param name="model">The graphical text with margin.</param>
		/// <returns>The graphical text with margin.</returns>
		public GraphicalTextWithMargin GetGraphicTextWithMarginFromModel(GraphicalTextWithMarginModel model);

		/// <summary>
		/// Gets the user interface element.
		/// </summary>
		/// <param name="uiElementModel">The user interface element model.</param>
		/// <returns>The user interface element.</returns>
		public IAmAUiElement GetUiElement(IAmAUiElementModel uiElementModel);

		/// <summary>
		/// Updates the element height.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <param name="height">The height.</param>
		public void UpdateElementHeight(IAmAUiElement element, float height);
	}
}

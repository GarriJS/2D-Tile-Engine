using Common.DiskModels.UI;
using Common.DiskModels.UI.Contracts;
using Common.UserInterface.Models;
using Common.UserInterface.Models.Contracts;
using Microsoft.Xna.Framework;

namespace Common.UserInterface.Services.Contracts
{
	/// <summary>
	/// Represents a user interface element service.
	/// </summary>
	public interface IUserInterfaceElementService
	{
		/// <summary>
		/// Checks the user interface element for a click.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <param name="elementLocation">The element location.</param>
		/// <param name="pressLocation">The press location.</param>
		public void CheckForUiElementClick(IAmAUiElement element, Vector2 elementLocation, Vector2 pressLocation);

		/// <summary>
		/// Gets the user interface padding from the model.
		/// </summary>
		/// <param name="model">The model.</param>
		/// <returns>The user interface padding model.</returns>
		public UiPadding GetUiPaddingFromModel(UiPaddingModel model);

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

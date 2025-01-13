using DiscModels.Engine.UI.Contracts;
using Engine.UI.Models.Contracts;

namespace Engine.UI.Services.Contracts
{
	/// <summary>
	/// Represents a user interface element service.
	/// </summary>
	public interface IUserInterfaceElementService
	{
		/// <summary>
		/// Gets the user interface element.
		/// </summary>
		/// <param name="uiElementModel">The user interface element model.</param>
		/// <param name="width">The width of the user interface element.</param>
		/// <param name="height">The height of the user interface element model.</param>
		/// <returns>The user interface element.</returns>
		public IAmAUiElement GetUiElement(IAmAUiElementModel uiElementModel, float width, float height);
	}
}

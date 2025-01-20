using DiscModels.Engine.UI.Contracts;
using Engine.UI.Models;
using Engine.UI.Models.Contracts;
using Microsoft.Xna.Framework;

namespace Engine.UI.Services.Contracts
{
	/// <summary>
	/// Represents a user interface element service.
	/// </summary>
	public interface IUserInterfaceElementService
	{
		/// <summary>
		/// Gets the element dimensions.
		/// </summary>
		/// <param name="uiScreenZone">The user interface screen zone.</param>
		/// <param name="elementModel">The user interface element model.</param>
		/// <returns>The element dimensions.</returns>
		public Vector2? GetElementDimensions(UiScreenZone uiScreenZone, IAmAUiElementModel elementModel);

		/// <summary>
		/// Gets the user interface element.
		/// </summary>
		/// <param name="uiElementModel">The user interface element model.</param>
		/// <param name="uiZone">The user interface zone.</param>
		/// <param name="fillWidth">The fill width of the user interface element.</param>
		/// <param name="fillHeight">The fill height of the user interface element model.</param>
		/// <returns>The user interface element.</returns>
		public IAmAUiElement GetUiElement(IAmAUiElementModel uiElementModel, UiScreenZone uiZone, float fillWidth, float fillHeight);
	}
}

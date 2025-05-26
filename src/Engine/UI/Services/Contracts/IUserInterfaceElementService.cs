using Engine.DiskModels.UI.Contracts;
using Engine.UI.Models;
using Engine.UI.Models.Contracts;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Engine.UI.Services.Contracts
{
	/// <summary>
	/// Represents a user interface element service.
	/// </summary>
	public interface IUserInterfaceElementService
	{
		/// <summary>
		/// Gets or sets the element hover event processors.
		/// </summary>
		public Dictionary<string, Action<IAmAUiElement, Vector2>> ElementHoverEventProcessors { get; set; }

		/// <summary>
		/// Gets or sets the element press event processors.
		/// </summary>
		public Dictionary<string, Action<IAmAUiElement, Vector2>> ElementPressEventProcessors { get; set; }

		/// <summary>
		/// Gets or sets the element click event processors.
		/// </summary>
		public Dictionary<string, Action<IAmAUiElement, Vector2>> ElementClickEventProcessors { get; set; }

		/// <summary>
		/// Gets the element dimensions.
		/// </summary>
		/// <param name="uiScreenZone">The user interface screen zone.</param>
		/// <param name="elementModel">The user interface element model.</param>
		/// <returns>The element dimensions.</returns>
		public Vector2? GetElementDimensions(UiScreenZone uiScreenZone, IAmAUiElementModel elementModel);

		/// <summary>
		/// Updates the element height.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <param name="height">The height.</param>
		public void UpdateElementHeight(IAmAUiElement element, float height);

		/// <summary>
		/// Checks the user interface element for a click.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <param name="elementLocation">The element location.</param>
		public void CheckForUiElementClick(IAmAUiElement element, Vector2 elementLocation);

		/// <summary>
		/// Gets the user interface element.
		/// </summary>
		/// <param name="uiElementModel">The user interface element model.</param>
		/// <param name="uiZone">The user interface zone.</param>
		/// <param name="fillWidth">The fill width of the user interface element.</param>
		/// <param name="visibilityGroup">The visibility group of the user interface element.</param>
		/// <returns>The user interface element.</returns>
		public IAmAUiElement GetUiElement(IAmAUiElementModel uiElementModel, UiScreenZone uiZone, float fillWidth, int visibilityGroup);
	}
}

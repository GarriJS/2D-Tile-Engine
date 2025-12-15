using Common.Controls.CursorInteraction.Models;
using Common.UserInterface.Models;
using Common.UserInterface.Models.Contracts;
using Engine.Physics.Models;
using Microsoft.Xna.Framework;

namespace Common.UserInterface.Services.Contracts
{
	public interface IUserInterfaceLocationService
	{
		/// <summary>
		/// Gets the user interface hover state at the screen location.
		/// </summary>
		/// <param name="location">The location.</param>
		/// <returns>The user interface hover state at the location if one is found.</returns>
		/// <remarks>TODO could be optimized further.</remarks>
		public HoverState GetUiObjectAtScreenLocation(Vector2 location);

		/// <summary>
		/// Tries to get the user interface zone at the given location.
		/// </summary>
		/// <param name="location">The location.</param>
		/// <param name="uiZone">The user interface zone.</param>
		/// <returns>A value indicating whether the user interface zone was found at the location.</returns>
		public bool TryGetUiZoneAtLocation(Vector2 location, out UiZone uiZone);

		/// <summary>
		/// Tries to get the user interface row at the given location.
		/// </summary>
		/// <param name="location">The location.</param>
		/// <param name="locatedUiRow">The located user interface row.</param>
		/// <returns>A value indicating whether a user interface row was found at the location.</returns>
		public bool TryGetUiRowAtLocation(Vector2 location, out LocationExtender<UiRow> locatedUiRow);

		/// <summary>
		/// Tries to get the user interface element at the location.
		/// </summary>
		/// <param name="location">The location.</param>
		/// <param name="locatedUiElement">The located user interface element.</param>
		/// <returns>A value indicated whether a user interface element was found at the location.</returns>
		public bool TryGetUiElementAtLocation(Vector2 location, out LocationExtender<IAmAUiElement> locatedUiElement);
	}
}

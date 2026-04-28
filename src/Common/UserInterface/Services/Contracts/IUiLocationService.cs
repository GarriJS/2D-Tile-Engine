using Common.UserInterface.Models;
using Common.UserInterface.Models.Contracts;
using Engine.Physics.Models;
using Microsoft.Xna.Framework;

namespace Common.UserInterface.Services.Contracts
{
    /// <summary>
    /// Represents a user interface location service.
    /// </summary>
    public interface IUiLocationService
    {
        /// <summary>
        /// Gets the user interface location descent.
        /// </summary>
        /// <param name="uiParent">The user interface parent.</param>
        /// <param name="locatedUiBlock">The located user interface block.</param>
        /// <param name="locatedUiRow">The located user interface row.</param>
        /// <param name="locatedUiElement">The located user interface element.</param>
        /// <returns>The user interface location descent.</returns>
        public UiLocationDescent GetUiLocationDescent(IAmAUiParent uiParent, Vector2Extender<UiBlock> locatedUiBlock, Vector2Extender<UiRow> locatedUiRow, Vector2Extender<IAmAUiElement> locatedUiElement);

        /// <summary>
        /// Gets the user interface descent at the screen location.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <returns>The user interface descent at the location.</returns>
        public UiLocationDescent GetUiDescentAtScreenLocation(Vector2 location);

        /// <summary>
        /// Tries to get the user interface modal at the given location.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="uiModal">The user interface modal.</param>
        /// <returns>A value indicating whether the user interface modal was found at the location.</returns>
        public bool TryGetUiModalAtLocation(Vector2 location, out UiModal uiModal);

        /// <summary>
        /// Tries to get the user interface zone at the given location.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="uiZone">The user interface zone.</param>
        /// <returns>A value indicating whether the user interface zone was found at the location.</returns>
        public bool TryGetUiZoneAtLocation(Vector2 location, out UiZone uiZone);

        /// <summary>
        /// Tries to get the user interface block at the given location.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="uiBlock">The located user interface block.</param>
        /// <param name="uiParent">The user interface parent.</param>
        /// <returns>A value indicating whether a user interface block was found at the location.</returns>
        public bool TryGetUiBlockAtLocation(Vector2 location, out Vector2Extender<UiBlock> uiBlock, IAmAUiParent uiParent = null);

        /// <summary>
        /// Tries to get the user interface row at the given location.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="uiRow">The located user interface row.</param>
        /// <param name="locatedUiBlock">The user interface block.</param>
        /// <returns>A value indicating whether a user interface row was found at the location.</returns>
        public bool TryGetUiRowAtLocation(Vector2 location, out Vector2Extender<UiRow> uiRow, Vector2Extender<UiBlock> locatedUiBlock = default);

        /// <summary>
        /// Tries to get the user interface element at the location.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="locatedUiElement">The located user interface element.</param>
        /// <returns>A value indicated whether a user interface element was found at the location.</returns>
        public bool TryGetUiElementAtLocation(Vector2 location, out Vector2Extender<IAmAUiElement> locatedUiElement, Vector2Extender<UiRow> locatedUiRow = default);
    }
}

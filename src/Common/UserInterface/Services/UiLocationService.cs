using Common.UserInterface.Models;
using Common.UserInterface.Models.Contracts;
using Common.UserInterface.Services.Contracts;
using Engine.Physics.Models;
using Microsoft.Xna.Framework;
using System.Linq;

namespace Common.UserInterface.Services
{
	/// <summary>
	/// Represents a user interface location service.
	/// </summary>
	/// <remarks>
	/// Initializes the user interface location service.
	/// </remarks>
	/// <param name="gameServices">The game services.</param>
	sealed public class UiLocationService(GameServiceContainer gameServices) : IUiLocationService
	{
		readonly private GameServiceContainer _gameServices = gameServices;

        /// <summary>
        /// Gets the user interface location descent.
        /// </summary>
        /// <param name="uiParent">The user interface parent.</param>
        /// <param name="locatedUiBlock">The located user interface block.</param>
        /// <param name="locatedUiRow">The located user interface row.</param>
        /// <param name="locatedUiElement">The located user interface element.</param>
        /// <returns>The user interface location descent.</returns>
        public UiLocationDescent GetUiLocationDescent(IAmAUiParent uiParent, Vector2Extender<UiBlock> locatedUiBlock, Vector2Extender<UiRow> locatedUiRow, Vector2Extender<IAmAUiElement> locatedUiElement)
        {
            var uiLocationDescent = new UiLocationDescent
            {
                UiParent = uiParent,
                UiLocatedBlock = locatedUiBlock,
                UiLocatedRow = locatedUiRow,
                UiLocatedElement = locatedUiElement
            };

            return uiLocationDescent;
        }

        /// <summary>
        /// Gets the user interface descent at the screen location.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <returns>The user interface descent at the location.</returns>
        public UiLocationDescent GetUiDescentAtScreenLocation(Vector2 location)
		{
			this.TryGetUiParentAtLocation(location, out var uiParent);
			this.TryGetUiBlockAtLocation(location, out var locatedBlock, uiParent);
			this.TryGetUiRowAtLocation(location, out var locatedRow, locatedBlock);
            this.TryGetUiElementAtLocation(location, out var locatedElement);
			var uiDescent = this.GetUiLocationDescent(uiParent, locatedBlock, locatedRow, locatedElement);

			return uiDescent;
		}

        /// <summary>
        /// Tries to get the user interface parent at the given location.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="uiParent">The user interface parent.</param>
        /// <returns>A value indicating whether the user interface parent was found at the location.</returns>
        private bool TryGetUiParentAtLocation(Vector2 location, out IAmAUiParent uiParent)
        {
            if (true == this.TryGetUiModalAtLocation(location, out var uiModal))
            {
                uiParent = uiModal;

                return true;
            }

            if (true == this.TryGetUiZoneAtLocation(location, out var uiZone))
            {
                uiParent = uiZone;

                return true;
            }

            uiParent = null;

            return false;
        }

        /// <summary>
        /// Tries to get the user interface modal at the given location.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="uiModal">The user interface modal.</param>
        /// <returns>A value indicating whether the user interface modal was found at the location.</returns>
        public bool TryGetUiModalAtLocation(Vector2 location, out UiModal uiModal)
		{
			var uiModalService = this._gameServices.GetService<IUiModalService>();
			uiModal = uiModalService.ActiveUiModals.FirstOrDefault(e => true == e.Area.Contains(location));

			return uiModal is not null;
		}

        /// <summary>
        /// Tries to get the user interface zone at the given location.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <param name="uiZone">The user interface zone.</param>
        /// <returns>A value indicating whether the user interface zone was found at the location.</returns>
        public bool TryGetUiZoneAtLocation(Vector2 location, out UiZone uiZone)
		{
			var uiGroupService = this._gameServices.GetService<IUiGroupService>();
			var activeUiGroup = uiGroupService.UserInterfaceGroups.FirstOrDefault(e => e.VisibilityGroupId == uiGroupService.ActiveVisibilityGroupId);
			uiZone = activeUiGroup?._zones.FirstOrDefault(e => true == e.Area.Contains(location));

			return uiZone is not null;
		}

		/// <summary>
		/// Tries to get the user interface block at the given location.
		/// </summary>
		/// <param name="location">The location.</param>
		/// <param name="uiBlock">The located user interface block.</param>
		/// <param name="uiParent">The user interface parent.</param>
		/// <returns>A value indicating whether a user interface block was found at the location.</returns>
		public bool TryGetUiBlockAtLocation(Vector2 location, out Vector2Extender<UiBlock> uiBlock, IAmAUiParent uiParent = null)
		{
			uiBlock = default;

			if ((uiParent is null) &&
				((false == this.TryGetUiParentAtLocation(location, out uiParent)) ||
				 (0 == uiParent.Blocks.Count)))
				return false;

			var uiZoneLocation = uiParent.Position.Coordinates;

			foreach (var blockLayout in uiParent.EnumerateBlockPositions(includeScrollOffset: true) ?? [])
			{
				var blockTop = uiZoneLocation.Y + blockLayout.Vector2.Y;
				var blockBottom = blockTop + blockLayout.Subject.InsideHeight;

				if ((location.Y < blockTop) ||
					(location.Y > blockBottom))
					continue;

				uiBlock = new Vector2Extender<UiBlock>
				{
					Vector2 = uiZoneLocation + blockLayout.Vector2,
					Subject = blockLayout.Subject
				};

				return true;
			}

			return false;
		}

		/// <summary>
		/// Tries to get the user interface row at the given location.
		/// </summary>
		/// <param name="location">The location.</param>
		/// <param name="uiRow">The located user interface row.</param>
		/// <param name="locatedUiBlock">The user interface block.</param>
		/// <returns>A value indicating whether a user interface row was found at the location.</returns>
		public bool TryGetUiRowAtLocation(Vector2 location, out Vector2Extender<UiRow> uiRow, Vector2Extender<UiBlock> locatedUiBlock = default)
		{
			uiRow = default;

			if ((locatedUiBlock.Subject is null) &&
				((false == this.TryGetUiBlockAtLocation(location, out locatedUiBlock)) ||
				 (0 == locatedUiBlock.Subject._rows.Count)))
				return false;

			foreach (var rowLayout in locatedUiBlock.Subject.EnumerateRowPosition(includeScrollOffset: true) ?? [])
			{
				var rowTop = locatedUiBlock.Vector2.Y + rowLayout.Vector2.Y;
				var rowBottom = rowTop + rowLayout.Subject.InsideHeight;

				if ((location.Y < rowTop) ||
					(location.Y > rowBottom))
					continue;

				uiRow = new Vector2Extender<UiRow>
				{
					Vector2 = locatedUiBlock.Vector2 + rowLayout.Vector2,
					Subject = rowLayout.Subject
				};

				return true;
			}

			return false;
		}

		/// <summary>
		/// Tries to get the user interface element at the location.
		/// </summary>
		/// <param name="location">The location.</param>
		/// <param name="locatedUiElement">The located user interface element.</param>
		/// <returns>A value indicated whether a user interface element was found at the location.</returns>
		public bool TryGetUiElementAtLocation(Vector2 location, out Vector2Extender<IAmAUiElement> locatedUiElement, Vector2Extender<UiRow> locatedUiRow = default)
		{
			locatedUiElement = default;

			if ((locatedUiRow.Subject is null) &&
				((false == this.TryGetUiRowAtLocation(location, out locatedUiRow)) ||
				 (0 == locatedUiRow.Subject._elements.Count)))
				return false;

			foreach (var elementLayout in locatedUiRow.Subject.EnumerateElementPosition() ?? [])
			{
				var elementLeft = locatedUiRow.Vector2.X + elementLayout.Vector2.X;
				var elementRight = elementLeft + elementLayout.Subject.InsideWidth;

				if ((elementLeft > location.X) ||
					(elementRight < location.X))
					continue;

				var elementTop = locatedUiRow.Vector2.Y + elementLayout.Vector2.Y;
				var elementBottom = elementTop + elementLayout.Subject.InsideHeight;

				if ((elementTop > location.Y) &&
					(elementBottom < location.Y))
					continue;

				locatedUiElement = new Vector2Extender<IAmAUiElement>
				{
					Vector2 = locatedUiRow.Vector2 + elementLayout.Vector2,
					Subject = elementLayout.Subject,
				};

				return true;
			}

			return false;
		}
	}
}

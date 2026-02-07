using Common.Controls.CursorInteraction.Models;
using Common.Controls.CursorInteraction.Models.Contracts;
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
		/// Gets the user interface hover state at the screen location.
		/// </summary>
		/// <param name="location">The location.</param>
		/// <returns>The user interface hover state at the location if one is found.</returns>
		/// <remarks>TODO could be optimized further.</remarks>
		public HoverState GetUiObjectAtScreenLocation(Vector2 location)
		{
			var hoverState = new HoverState();

			if (true == this.TryGetUiZoneAtLocation(location, out var uiZone))
			{
				if ((uiZone.ScrollState is not null) &&
					(false == uiZone.ScrollState.DisableScrolling))
					hoverState.BottomScrollable = uiZone;

				hoverState.TopCursorConfiguration = uiZone.CursorConfiguration;
				hoverState.TopHoverCursor = uiZone.HoverCursor;
				hoverState.HoverObjectLocation = new Vector2Extender<IHaveACursorConfiguration>
				{
					Vector = uiZone.Position.Coordinates,
					Subject = uiZone
				};
				hoverState._hoveredObjects[typeof(UiZone)] = uiZone;
			}

			if (true == this.TryGetUiBlockAtLocation(location, out var locatedBlock))
			{
				if (locatedBlock.Subject.CursorConfiguration is not null)
					hoverState.TopCursorConfiguration = locatedBlock.Subject.CursorConfiguration;

				if (locatedBlock.Subject.HoverCursor is not null)
					hoverState.TopHoverCursor = locatedBlock.Subject.HoverCursor;

				if ((locatedBlock.Subject.ScrollState is not null) &&
					(false == locatedBlock.Subject.ScrollState.DisableScrolling))
					hoverState.BottomScrollable = locatedBlock.Subject;

				hoverState.HoverObjectLocation = new Vector2Extender<IHaveACursorConfiguration>
				{
					Vector = locatedBlock.Vector,
					Subject = locatedBlock.Subject
				};
				hoverState._hoveredObjects[typeof(UiBlock)] = locatedBlock.Subject;
			}

			if (true == this.TryGetUiRowAtLocation(location, out var locatedRow))
			{
				if (locatedRow.Subject.CursorConfiguration is not null)
					hoverState.TopCursorConfiguration = locatedRow.Subject.CursorConfiguration;

				if (locatedRow.Subject.HoverCursor is not null)
					hoverState.TopHoverCursor = locatedRow.Subject.HoverCursor;

				hoverState.HoverObjectLocation = new Vector2Extender<IHaveACursorConfiguration>
				{
					Vector = locatedRow.Vector,
					Subject = locatedRow.Subject
				};
				hoverState._hoveredObjects[typeof(UiRow)] = locatedRow.Subject;
			}

			if (true == this.TryGetUiElementAtLocation(location, out var locatedElement))
			{
				if (locatedElement.Subject.CursorConfiguration is not null)
					hoverState.TopCursorConfiguration = locatedElement.Subject.CursorConfiguration;

				if (locatedElement.Subject.HoverCursor is not null)
					hoverState.TopHoverCursor = locatedElement.Subject.HoverCursor;

				hoverState.HoverObjectLocation = new Vector2Extender<IHaveACursorConfiguration>
				{
					Vector = locatedElement.Vector,
					Subject = locatedElement.Subject
				};
				hoverState._hoveredObjects[typeof(IAmAUiElement)] = locatedElement.Subject;
			}

			if (hoverState.HoverObjectLocation is null)
				return null;

			return hoverState;
		}

		/// <summary>
		/// Tries to get the user interface zone at the given location.
		/// </summary>
		/// <param name="location">The location.</param>
		/// <param name="uiZone">The user interface zone.</param>
		/// <returns>A value indicating whether the user interface zone was found at the location.</returns>
		public bool TryGetUiZoneAtLocation(Vector2 location, out UiZone uiZone)
		{
			var uiGroupService = this._gameServices.GetService<IUserInterfaceGroupService>();
			var activeUiGroup = uiGroupService.UserInterfaceGroups.FirstOrDefault(e => e.VisibilityGroupId == uiGroupService.ActiveVisibilityGroupId);
			uiZone = activeUiGroup?._zones.FirstOrDefault(e => true == e.Area.Contains(location));

			return uiZone is not null;
		}

		/// <summary>
		/// Tries to get the user interface block at the given location.
		/// </summary>
		/// <param name="location">The location.</param>
		/// <param name="uiBlock">The located user interface block.</param>
		/// <returns>A value indicating whether a user interface block was found at the location.</returns>
		public bool TryGetUiBlockAtLocation(Vector2 location, out Vector2Extender<UiBlock> uiBlock)
		{
			uiBlock = default;

			if ((false == this.TryGetUiZoneAtLocation(location, out var uiZone)) ||
				(0 == uiZone._blocks.Count))
				return false;

			var uiZoneLocation = uiZone.Position.Coordinates;

			foreach (var blockLayout in uiZone.EnumerateLayout(includeScrollOffset: true) ?? [])
			{
				var blockTop = uiZoneLocation.Y + blockLayout.Vector.Y;
				var blockBottom = blockTop + blockLayout.Subject.InsideHeight;

				if ((location.Y < blockTop) ||
					(location.Y > blockBottom))
					continue;

				uiBlock = new Vector2Extender<UiBlock>
				{
					Vector = uiZoneLocation + blockLayout.Vector,
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
		/// <returns>A value indicating whether a user interface row was found at the location.</returns>
		public bool TryGetUiRowAtLocation(Vector2 location, out Vector2Extender<UiRow> uiRow)
		{
			uiRow = default;

			if ((false == this.TryGetUiBlockAtLocation(location, out var locatedUiBlock)) ||
				(0 == locatedUiBlock.Subject._rows.Count))
				return false;

			foreach (var rowLayout in locatedUiBlock.Subject.EnumerateLayout(includeScrollOffset: true) ?? [])
			{
				var rowTop = locatedUiBlock.Vector.Y + rowLayout.Vector.Y;
				var rowBottom = rowTop + rowLayout.Subject.InsideHeight;

				if ((location.Y < rowTop) ||
					(location.Y > rowBottom))
					continue;

				uiRow = new Vector2Extender<UiRow>
				{
					Vector = locatedUiBlock.Vector + rowLayout.Vector,
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
		public bool TryGetUiElementAtLocation(Vector2 location, out Vector2Extender<IAmAUiElement> locatedUiElement)
		{
			locatedUiElement = default;

			if ((false == this.TryGetUiRowAtLocation(location, out var locatedUiRow)) ||
				(0 == locatedUiRow.Subject._elements.Count))
				return false;

			foreach (var elementLayout in locatedUiRow.Subject.EnumerateLayout() ?? [])
			{
				var elementLeft = locatedUiRow.Vector.X + elementLayout.Vector.X;
				var elementRight = elementLeft + elementLayout.Subject.InsideWidth;

				if ((elementLeft > location.X) ||
					(elementRight < location.X))
					continue;

				var elementTop = locatedUiRow.Vector.Y + elementLayout.Vector.Y;
				var elementBottom = elementTop + elementLayout.Subject.InsideHeight;

				if ((elementTop > location.Y) &&
					(elementBottom < location.Y))
					continue;

				locatedUiElement = new Vector2Extender<IAmAUiElement>
				{
					Vector = locatedUiRow.Vector + elementLayout.Vector,
					Subject = elementLayout.Subject,
				};

				return true;
			}

			return false;
		}
	}
}

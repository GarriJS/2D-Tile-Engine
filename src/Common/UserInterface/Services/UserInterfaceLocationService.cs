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
	/// Initialize the user interface location service.
	/// </remarks>
	/// <param name="gameServices">The game services.</param>
	public class UserInterfaceLocationService(GameServiceContainer gameServices) : IUserInterfaceLocationService
	{
		private readonly GameServiceContainer _gameServices = gameServices;

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
				hoverState.TopCursorConfiguration = uiZone.CursorConfiguration;
				hoverState.TopHoverCursor = uiZone.HoverCursor;
				hoverState.HoverObjectLocation = new LocationExtender<IHaveACursorConfiguration>
				{
					Location = uiZone.Position.Coordinates,
					Subject = uiZone
				};
			}

			if (true == this.TryGetUiRowAtLocation(location, out var locatedRow))
			{
				if (locatedRow.Subject.CursorConfiguration is not null)
				{
					hoverState.TopCursorConfiguration = locatedRow.Subject.CursorConfiguration;
				}

				if (locatedRow.Subject.HoverCursor is not null)
				{
					hoverState.TopHoverCursor = locatedRow.Subject.HoverCursor;
				}

				hoverState.HoverObjectLocation = new LocationExtender<IHaveACursorConfiguration>
				{
					Location = locatedRow.Location,
					Subject = locatedRow.Subject
				};
			}

			if (true == this.TryGetUiElementAtLocation(location, out var locatedElement))
			{
				if (locatedElement.Subject.CursorConfiguration is not null)
				{
					hoverState.TopCursorConfiguration = locatedElement.Subject.CursorConfiguration;
				}

				if (locatedElement.Subject.HoverCursor is not null)
				{
					hoverState.TopHoverCursor = locatedElement.Subject.HoverCursor;
				}

				hoverState.HoverObjectLocation = new LocationExtender<IHaveACursorConfiguration>
				{
					Location = locatedElement.Location,
					Subject = locatedElement.Subject
				};
			}

			if (hoverState.HoverObjectLocation is null)
			{
				return null;
			}

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
			var uiService = this._gameServices.GetService<IUserInterfaceService>();

			var activeUiGroup = uiService.UserInterfaceGroups.FirstOrDefault(e => e.VisibilityGroupId == uiService.ActiveVisibilityGroupId);
			uiZone = activeUiGroup.UiZones.FirstOrDefault(e => true == e.Area.Contains(location));

			return uiZone is not null;
		}

		/// <summary>
		/// Tries to get the user interface zone child at the given location.
		/// </summary>
		/// <param name="location">The location.</param>
		/// <param name="uiZoneChild">The user interface child.</param>
		/// <returns>A value indicating whether the user interface zone child was found at the location.</returns>
		public bool TryGetUiZoneChildAtLocation(Vector2 location, out LocationExtender<IAmAUiZoneChild> uiZoneChild)
		{
			uiZoneChild = null;

			if ((false == this.TryGetUiZoneAtLocation(location, out var uiZone)) ||
				(0 == uiZone.Components.Count))
			{
				return false;
			}

			foreach (var componentLayout in uiZone.EnumerateLayout() ?? [])
			{
				var rowTop = uiZone.Position.Y + componentLayout.Offset.Y;
				var rowBottom = rowTop + componentLayout.Component.InsideHeight;

				if ((location.Y < rowTop) ||
					(location.Y > rowBottom))
					continue;

				uiZoneChild = new LocationExtender<IAmAUiZoneChild>
				{
					Location = uiZone.Position.Coordinates + componentLayout.Offset,
					Subject = componentLayout.Component
				};

				break;
			}

			return uiZoneChild is not null;
		}

		/// <summary>
		/// Tries to get the user interface block at the given location.
		/// </summary>
		/// <param name="location">The location.</param>
		/// <param name="uiBlock">The located user interface block.</param>
		/// <returns>A value indicating whether a user interface block was found at the location.</returns>
		public bool TryGetUiBlockAtLocation(Vector2 location, out LocationExtender<UiBlock> uiBlock)
		{
			uiBlock = null;

			if ((false == this.TryGetUiZoneAtLocation(location, out var uiZone)) ||
				(0 == uiZone.Components.Count))
			{
				return false;
			}

			foreach (var componentLayout in uiZone.EnumerateLayout() ?? [])
			{
				var rowTop = uiZone.Position.Y + componentLayout.Offset.Y;
				var rowBottom = rowTop + componentLayout.Component.InsideHeight;

				if ((location.Y < rowTop) ||
					(location.Y > rowBottom))
					continue;

				if (componentLayout.Component is not UiBlock uiBlockComponent)
				{
					return false;
				}

				uiBlock = new LocationExtender<UiBlock>
				{
					Location = uiZone.Position.Coordinates + componentLayout.Offset,
					Subject = uiBlockComponent
				};

				break;
			}

			return uiBlock is not null;
		}

		/// <summary>
		/// Tries to get the user interface row at the given location.
		/// </summary>
		/// <param name="location">The location.</param>
		/// <param name="uiRow">The located user interface row.</param>
		/// <returns>A value indicating whether a user interface row was found at the location.</returns>
		public bool TryGetUiRowAtLocation(Vector2 location, out LocationExtender<UiRow> uiRow)
		{
			uiRow = null;

			if ((false == this.TryGetUiZoneAtLocation(location, out var uiZone)) ||
				(0 == uiZone.Components.Count))
			{
				return false;
			}

			foreach (var componentLayout in uiZone.EnumerateLayout() ?? [])
			{
				var rowTop = uiZone.Position.Y + componentLayout.Offset.Y;
				var rowBottom = rowTop + componentLayout.Component.InsideHeight;

				if ((location.Y < rowTop) ||
					(location.Y > rowBottom))
					continue;

				if (componentLayout.Component is UiBlock uiBlockComponent)
				{

				}
				else if (componentLayout.Component is UiRow uiRowComponent)
				{
					uiRow = new LocationExtender<UiRow>
					{
						Location = uiZone.Position.Coordinates + componentLayout.Offset,
						Subject = uiRowComponent
					};

					break;
				}
			}

			return uiRow is not null;
		}

		/// <summary>
		/// Tries to get the user interface element at the location.
		/// </summary>
		/// <param name="location">The location.</param>
		/// <param name="locatedUiElement">The located user interface element.</param>
		/// <returns>A value indicated whether a user interface element was found at the location.</returns>
		public bool TryGetUiElementAtLocation(Vector2 location, out LocationExtender<IAmAUiElement> locatedUiElement)
		{
			locatedUiElement = null;

			if ((false == this.TryGetUiRowAtLocation(location, out var locatedUiRow)) ||
				(0 == locatedUiRow.Subject.Elements.Count))
			{
				return false;
			}

			foreach (var elementLayout in locatedUiRow.Subject.EnumerateLayout() ?? [])
			{
				var elementLeft = locatedUiRow.Location.X + elementLayout.Offset.X;
				var elementRight = elementLeft + elementLayout.Element.InsideWidth;

				if ((elementLeft > location.X) ||
					(elementRight < location.X))
					continue;


				var elementTop = locatedUiRow.Location.Y + elementLayout.Offset.Y;
				var elementBottom = elementTop + elementLayout.Element.InsideHeight;

				if ((elementTop > location.Y) &&
					(elementBottom < location.Y))
					continue;

				locatedUiElement = new LocationExtender<IAmAUiElement>
				{
					Location = locatedUiRow.Location + elementLayout.Offset,
					Subject = elementLayout.Element,
				};

				break;
			}

			return locatedUiElement is not null;
		}
	}
}

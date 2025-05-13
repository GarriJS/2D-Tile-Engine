using Engine.Physics.Models;
using Engine.UI.Models;
using Engine.UI.Models.Enums;
using Engine.UI.Services.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Engine.UI.Services
{
	/// <summary>
	/// Represents a user interface screen zone service.
	/// </summary>
	/// <remarks>
	/// Initializes the user interface screen zone service.
	/// </remarks>
	/// <param name="gameServices">The game service.</param>
	public class UserInterfaceScreenZoneService(GameServiceContainer gameServices) : IUserInterfaceScreenZoneService
	{
		private readonly GameServiceContainer _gameServices = gameServices;

		/// <summary>
		/// Gets or sets the user interface zones.
		/// </summary>
		public Dictionary<UiScreenZoneTypes, UiScreenZone> UserInterfaceScreenZones { get; set; } = [];

		/// <summary>
		/// Gets or sets the zone type mapper.
		/// </summary>
		private Dictionary<(int row, int col), UiScreenZoneTypes> ZoneTypeMapper { get; } = new()
		{
			{ (1, 1), UiScreenZoneTypes.Row1Col1 },
			{ (2, 1), UiScreenZoneTypes.Row2Col1 },
			{ (3, 1), UiScreenZoneTypes.Row3Col1 },
			{ (1, 2), UiScreenZoneTypes.Row1Col2 },
			{ (2, 2), UiScreenZoneTypes.Row2Col2 },
			{ (3, 2), UiScreenZoneTypes.Row3Col2 },
			{ (1, 3), UiScreenZoneTypes.Row1Col3 },
			{ (2, 3), UiScreenZoneTypes.Row2Col3 },
			{ (3, 3), UiScreenZoneTypes.Row3Col3 },
			{ (1, 4), UiScreenZoneTypes.Row1Col4 },
			{ (2, 4), UiScreenZoneTypes.Row2Col4 },
			{ (3, 4), UiScreenZoneTypes.Row3Col4 }
		};

		/// <summary>
		/// Performs initialization.
		/// </summary>
		public void Initialize()
		{
			this.InitializeUiScreenZones();
		}

		/// <summary>
		/// Initialize the user interface zones.
		/// </summary>
		public void InitializeUiScreenZones()
		{
			var graphicsDeviceService = this._gameServices.GetService<IGraphicsDeviceService>();
			var screenWidth = graphicsDeviceService.GraphicsDevice.PresentationParameters.BackBufferWidth;
			var screenHeight = graphicsDeviceService.GraphicsDevice.PresentationParameters.BackBufferHeight;

			for (int y = 0; y < screenHeight; y += screenHeight / 3)
			{
				for (int x = 0; x < screenWidth; x += screenWidth / 4)
				{
					var row = y / (screenHeight / 3) + 1;
					var col = x / (screenWidth / 4) + 1;

					if ((false == this.ZoneTypeMapper.TryGetValue((row, col), out var zoneType)) ||
						(true == this.UserInterfaceScreenZones.ContainsKey(zoneType)))
					{
						continue;
					}

					var zone = new UiScreenZone
					{
						UiZoneType = zoneType,
						Area = new SimpleArea
						{
							Width = screenWidth / 4,
							Height = screenHeight / 3,
							Position = new Position
							{
								Coordinates = new Vector2(x, y)
							}
						}
					};

					this.UserInterfaceScreenZones.TryAdd(zoneType, zone);
				}
			}

			var noneZone = new UiScreenZone
			{
				UiZoneType = UiScreenZoneTypes.None,
				Area = new SimpleArea
				{
					Width = screenWidth / 4,
					Height = screenHeight / 3,
					Position = new Position
					{
						Coordinates = default
					}
				}
			};

			this.UserInterfaceScreenZones.TryAdd(UiScreenZoneTypes.None, noneZone);
		}
	}
}

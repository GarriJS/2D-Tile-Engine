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
			{ (1, 1), UiScreenZoneTypes.Col1Row1 },
			{ (2, 1), UiScreenZoneTypes.Col2Row1 },
			{ (3, 1), UiScreenZoneTypes.Col3Row1 },
			{ (1, 2), UiScreenZoneTypes.Col1Row2 },
			{ (2, 2), UiScreenZoneTypes.Col2Row2 },
			{ (3, 2), UiScreenZoneTypes.Col3Row2 },
			{ (1, 3), UiScreenZoneTypes.Col1Row3 },
			{ (2, 3), UiScreenZoneTypes.Col2Row3 },
			{ (3, 3), UiScreenZoneTypes.Col3Row3 },
			{ (1, 4), UiScreenZoneTypes.Col1Row4 },
			{ (2, 4), UiScreenZoneTypes.Col2Row4 },
			{ (3, 4), UiScreenZoneTypes.Col3Row4 }
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
						Coordinates = new Vector2(0, 0)
					}
				}
			};

			this.UserInterfaceScreenZones.TryAdd(UiScreenZoneTypes.None, noneZone);
		}
	}
}

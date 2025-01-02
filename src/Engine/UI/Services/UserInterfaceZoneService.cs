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
	/// Represents a user interface zone service.
	/// </summary>
	/// <remarks>
	/// Initializes the user interface zone service.
	/// </remarks>
	/// <param name="gameServices">The game service.</param>
	public class UserInterfaceZoneService(GameServiceContainer gameServices) : IUserInterfaceZoneService
	{
		private readonly GameServiceContainer _gameServices = gameServices;

		/// <summary>
		/// Gets or sets the user interface zones.
		/// </summary>
		public Dictionary<UiZoneTypes, UiZone> UserInterfaceZones { get; set; } = [];

		/// <summary>
		/// Gets or sets the zone type mapper.
		/// </summary>
		private Dictionary<(int row, int col), UiZoneTypes> ZoneTypeMapper { get; } = new()
		{
			{ (1, 1), UiZoneTypes.Col1Row1 },
			{ (2, 1), UiZoneTypes.Col2Row1 },
			{ (3, 1), UiZoneTypes.Col3Row1 },
			{ (1, 2), UiZoneTypes.Col1Row2 },
			{ (2, 2), UiZoneTypes.Col2Row2 },
			{ (3, 2), UiZoneTypes.Col3Row2 },
			{ (1, 3), UiZoneTypes.Col1Row3 },
			{ (2, 3), UiZoneTypes.Col2Row3 },
			{ (3, 3), UiZoneTypes.Col3Row3 },
			{ (1, 4), UiZoneTypes.Col1Row4 },
			{ (2, 4), UiZoneTypes.Col2Row4 },
			{ (3, 4), UiZoneTypes.Col3Row4 }
		};

		/// <summary>
		/// Performs initialization.
		/// </summary>
		public void Initialize()
		{
			this.InitializeUiZones();
		}

		/// <summary>
		/// Initialize the user interface zones.
		/// </summary>
		public void InitializeUiZones()
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
						(true == this.UserInterfaceZones.ContainsKey(zoneType)))
					{
						continue;
					}

					var zone = new UiZone
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

					this.UserInterfaceZones.TryAdd(zoneType, zone);
				}
			}

			var noneZone = new UiZone
			{
				UiZoneType = UiZoneTypes.None,
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

			this.UserInterfaceZones.TryAdd(UiZoneTypes.None, noneZone);
		}
	}
}

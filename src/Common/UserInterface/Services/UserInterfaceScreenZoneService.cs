using Common.UserInterface.Enums;
using Common.UserInterface.Models;
using Common.UserInterface.Services.Contracts;
using Engine.DiskModels.Physics;
using Engine.Physics.Models.SubAreas;
using Engine.Physics.Services.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Common.UserInterface.Services
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
		/// Gets or sets the screen zone size.
		/// </summary>
		public SubArea ScreenZoneSize { get; set; }

		/// <summary>
		/// Gets or sets the user interface zones.
		/// </summary>
		public Dictionary<UiScreenZoneType, UiScreenZone> UserInterfaceScreenZones { get; set; } = [];

		/// <summary>
		/// Gets or sets the zone type mapper.
		/// </summary>
		private Dictionary<(int row, int col), UiScreenZoneType> ZoneTypeMapper { get; } = new()
		{
			{ (1, 1), UiScreenZoneType.Row1Col1 },
			{ (2, 1), UiScreenZoneType.Row2Col1 },
			{ (3, 1), UiScreenZoneType.Row3Col1 },
			{ (1, 2), UiScreenZoneType.Row1Col2 },
			{ (2, 2), UiScreenZoneType.Row2Col2 },
			{ (3, 2), UiScreenZoneType.Row3Col2 },
			{ (1, 3), UiScreenZoneType.Row1Col3 },
			{ (2, 3), UiScreenZoneType.Row2Col3 },
			{ (3, 3), UiScreenZoneType.Row3Col3 },
			{ (1, 4), UiScreenZoneType.Row1Col4 },
			{ (2, 4), UiScreenZoneType.Row2Col4 },
			{ (3, 4), UiScreenZoneType.Row3Col4 }
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
			var areaService = this._gameServices.GetService<IAreaService>();

			this.ScreenZoneSize = new SubArea
			{
				Width = graphicsDeviceService.GraphicsDevice.PresentationParameters.BackBufferWidth / 4,
				Height = graphicsDeviceService.GraphicsDevice.PresentationParameters.BackBufferHeight / 3
			};

			for (int y = 0; y < this.ScreenZoneSize.Height * 3; y += (int)this.ScreenZoneSize.Height)
			{
				for (int x = 0; x < this.ScreenZoneSize.Width * 4; x += (int)this.ScreenZoneSize.Width)
				{
					var row = (y / (int)this.ScreenZoneSize.Height) + 1;
					var col = (x / (int)this.ScreenZoneSize.Width) + 1;

					if ((false == this.ZoneTypeMapper.TryGetValue((row, col), out var zoneType)) ||
						(true == this.UserInterfaceScreenZones.ContainsKey(zoneType)))
					{
						continue;
					}

					var areaModel = new AreaModel
					{
						Position = new PositionModel
						{
							X = x,
							Y = y,
						},
						Width = this.ScreenZoneSize.Width,
						Height = this.ScreenZoneSize.Height,
					};

					var area = areaService.GetAreaFromModel(areaModel);
					var zone = new UiScreenZone
					{
						UiZoneType = zoneType,
						Area = area
					};

					this.UserInterfaceScreenZones.TryAdd(zoneType, zone);
				}
			}

			var noneAreaModel = new AreaModel
			{
				Position = new PositionModel
				{
					X = default,
					Y = default,
				},
				Width = this.ScreenZoneSize.Width,
				Height = this.ScreenZoneSize.Height,
			};

			var noneArea = areaService.GetAreaFromModel(noneAreaModel);
			var noneZone = new UiScreenZone
			{
				UiZoneType = UiScreenZoneType.Unknown,
				Area = noneArea
			};

			this.UserInterfaceScreenZones.TryAdd(UiScreenZoneType.Unknown, noneZone);
		}
	}
}

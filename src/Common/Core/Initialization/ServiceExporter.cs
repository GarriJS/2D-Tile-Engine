using Common.Controls.Services;
using Common.Controls.Services.Contracts;
using Common.Tiling.Services;
using Common.Tiling.Services.Contracts;
using Common.UI.Services.Contracts;
using Common.UI.Services;
using Microsoft.Xna.Framework;
using System;
using Engine.Controls.Services.Contracts;

namespace Common.Core.Initialization
{
	/// <summary>
	/// Represents a service exporter.
	/// </summary>
	public static class ServiceExporter
	{
		/// <summary>
		/// Gets the service contract pairs.
		/// </summary>
		/// <param name="game">The game.</param>
		/// <returns>The service contract pairs.</returns>
		public static (Type type, object provider)[] GetServiceContractPairs(Game game)
		{
			return
			[
				(typeof(IMouseService), new MouseService(game.Services)),
				(typeof(IUserInterfaceScreenZoneService), new UserInterfaceScreenZoneService(game.Services)),
				(typeof(IUserInterfaceElementService), new UserInterfaceElementService(game.Services)),
				(typeof(IUserInterfaceService), new UserInterfaceService(game.Services)),
				(typeof(ICursorService), new CursorService(game.Services)),
				(typeof(ITileService), new TileService(game.Services))
			];
		}
	}
}

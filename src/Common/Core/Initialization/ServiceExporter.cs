using Common.Controls.CursorInteraction.Services;
using Common.Controls.CursorInteraction.Services.Contracts;
using Common.Controls.Cursors.Services;
using Common.Controls.Cursors.Services.Contracts;
using Common.Tiling.Services;
using Common.Tiling.Services.Contracts;
using Common.UserInterface.Services;
using Common.UserInterface.Services.Contracts;
using Microsoft.Xna.Framework;
using System;

namespace Common.Core.Initialization
{
	/// <summary>
	/// Represents a service exporter.
	/// </summary>
	static public class ServiceExporter
	{
		/// <summary>
		/// Gets the service contract pairs.
		/// </summary>
		/// <param name="game">The game.</param>
		/// <returns>The service contract pairs.</returns>
		static public (Type type, object provider)[] GetServiceContractPairs(Game game)
		{
			return
			[
				(typeof(ICursorService), new CursorService(game.Services)),
				(typeof(ICursorInteractionService), new CursorInteractionService(game.Services)),
				(typeof(IUserInterfaceScreenZoneService), new UserInterfaceScreenZoneService(game.Services)),
				(typeof(IUserInterfaceElementService), new UserInterfaceElementService(game.Services)),
				(typeof(IUserInterfaceService), new UserInterfaceService(game.Services)),
				(typeof(ITileService), new TileService(game.Services))
			];
		}
	}
}

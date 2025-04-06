using Common.Controls.Services;
using Common.Controls.Services.Contracts;
using Common.Tiling.Services;
using Common.Tiling.Services.Contracts;
using Microsoft.Xna.Framework;
using System;

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
				(typeof(ICursorService), new CursorService(game.Services)),
				(typeof(ITileService), new TileService(game.Services))
			];
		}
	}
}

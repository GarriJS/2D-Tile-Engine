using Common.DiskModels.Common.Tiling;
using Common.Tiling.Models;
using Common.Tiling.Services.Contracts;
using Microsoft.Xna.Framework;
using System;

namespace Common.DiskModels
{
	/// <summary>
	/// Represents a model mappings exporter.
	/// </summary>
	public static class ModelMappingsExporter
	{
		/// <summary>
		/// Gets the model type mappings.
		/// </summary>
		/// <returns>The model type mappings.</returns>
		public static (Type typeIn, Type typeOut)[] GetModelTypeMappings()
		{
			return
			[
				(typeof(AnimatedTileModel), typeof(AnimatedTile)),
				(typeof(TileMapLayerModel), typeof(TileMapLayer)),
				(typeof(TileMapModel), typeof(TileMap)),
				(typeof(TileModel), typeof(Tile))
			];
		}

		/// <summary>
		/// Gets the model processing mappings.
		/// </summary>
		/// <param name="gameServices">The game services.</param>
		/// <returns>The model processing mappings.</returns>
		public static (Type typeIn, Delegate)[] GetModelProcessingMappings(GameServiceContainer gameServices)
		{
			var tileService = gameServices.GetService<ITileService>();

			return
			[
				(typeof(AnimatedTileModel), tileService.GetTile),
				(typeof(TileMapLayerModel), tileService.GetTileMapLayer),
				(typeof(TileMapModel), tileService.GetTileMap),
				(typeof(TileModel), tileService.GetTile)
			];
		}
	}
}

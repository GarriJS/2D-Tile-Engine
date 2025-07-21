using Common.DiskModels.Common.Tiling;
using Common.DiskModels.Common.Tiling.Contracts;
using Common.Tiling.Models;
using Common.Tiling.Models.Contracts;
using Engine.Core.Contracts;
using Microsoft.Xna.Framework;

namespace Common.Tiling.Services.Contracts
{
	/// <summary>
	/// Represents a tile service.
	/// </summary>
	public interface ITileService : ILoadContent
	{
		/// <summary>
		/// Gets the local tile coordinates.
		/// </summary>
		/// <param name="coordinates">The coordinates.</param>
		/// <param name="gridOffset">The grid offset.</param>
		/// <returns>The local tile coordinates.</returns>
		public Vector2 GetLocalTileCoordinates(Vector2 coordinates, int gridOffset = 0);

		/// <summary>
		/// Gets the tile map.
		/// </summary>
		/// <param name="tileMapModel">The tile map model.</param>
		/// <returns>The tile map.</returns>
		public TileMap GetTileMap(TileMapModel tileMapModel);

		/// <summary>
		/// Gets the tile map layer.
		/// </summary>
		/// <param name="tileMapLayerModel">The tile map layer model.</param>
		/// <returns>The tile map layer.</returns>
		public TileMapLayer GetTileMapLayer(TileMapLayerModel tileMapLayerModel);

		/// <summary>
		/// Gets the tile.
		/// </summary>
		/// <param name="tileModel">The tile model.</param>
		/// <returns>The tile.</returns>
		public IAmATile GetTile(IAmATileModel tileModel);
	}
}

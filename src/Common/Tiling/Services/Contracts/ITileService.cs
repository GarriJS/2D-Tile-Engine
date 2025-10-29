using Common.Controls.Cursors.Models;
using Common.DiskModels.Tiling;
using Common.Tiling.Models;
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
		/// Updates the tile grid cursor position.
		/// </summary>
		/// <param name="cursor">The cursor.</param>
		/// <param name="gameTime">The game time.</param>
		public void TileGridCursorUpdater(Cursor cursor, GameTime gameTime);

		/// <summary>
		/// Gets the local tile coordinates.
		/// </summary>
		/// <param name="coordinates">The coordinates.</param>
		/// <param name="gridOffset">The grid offset.</param>
		/// <returns>The local tile coordinates.</returns>
		public Vector2 GetLocalTileCoordinates(Vector2 coordinates, int gridOffset = 0);

		/// <summary>
		/// Gets the tile map from the model.
		/// </summary>
		/// <param name="tileMapModel">The tile map model.</param>
		/// <returns>The tile map.</returns>
		public TileMap GetTileMapFromModel(TileMapModel tileMapModel);

		/// <summary>
		/// Gets the tile map layer from the model.
		/// </summary>
		/// <param name="tileMapLayerModel">The tile map layer model.</param>
		/// <returns>The tile map layer.</returns>
		public TileMapLayer GetTileMapLayerFromModel(TileMapLayerModel tileMapLayerModel);

		/// <summary>
		/// Gets the tile from the model.
		/// </summary>
		/// <param name="tileModel">The tile model.</param>
		/// <returns>The tile.</returns>
		public Tile GetTileFromModel(TileModel tileModel);
	}
}

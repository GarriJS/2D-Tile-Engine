using Common.Tiling.Models.Contracts;
using System.Collections.Generic;

namespace Common.Tiling.Models
{
	/// <summary>
	/// Represents a tile map layer.
	/// </summary>
	public class TileMapLayer
	{
		/// <summary>
		/// Gets or sets the layer.
		/// </summary>
		public int Layer { get; set; }

		/// <summary>
		/// Gets or sets the tiles.
		/// </summary>
		public Dictionary<(int row, int col), IAmATile> Tiles { get; set; } = [];

		/// <summary>
		/// Adds the tile.
		/// </summary>
		/// <param name="tile">The tile.</param>
		public void AddTile(IAmATile tile)
		{
			this.Tiles[(tile.Row, tile.Column)] = tile;
		}
	}
}

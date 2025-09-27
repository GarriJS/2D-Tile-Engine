using Common.Tiling.Models.Contracts;
using System.Collections.Generic;

namespace Common.Tiling.Models
{
	/// <summary>
	/// Represents a tile map.
	/// </summary>
	public class TileMap
	{
		/// <summary>
		/// Gets or sets the tile map name.
		/// </summary>
		public string TileMapName { get; set; }

		/// <summary>
		/// Gets or sets the tile map layer.
		/// </summary>
		public Dictionary<int, TileMapLayer> TileMapLayers { get; set; } = [];

		/// <summary>
		/// Adds the tile. 
		/// </summary>
		/// <param name="layer">The layer of the tile.</param>
		/// <param name="tile">The tile.</param>
		public void AddTile(int layer, IAmATile tile)
		{
			if (true == this.TileMapLayers.TryGetValue(layer, out var tileMapLayer))
			{ 
				tileMapLayer.AddTile(tile);
			}

			tileMapLayer = new TileMapLayer
			{ 
				Layer = layer,
			};

			tileMapLayer.AddTile(tile);
			this.TileMapLayers[layer] = tileMapLayer;
		}
	}
}

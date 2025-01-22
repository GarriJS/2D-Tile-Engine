using System.Collections.Generic;

namespace Engine.Tiling.Models
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
		public Dictionary<int, TileMapLayer> TileMapLayers { get; set; }
	}
}

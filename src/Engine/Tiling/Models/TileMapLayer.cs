using Engine.Tiling.Models.Contracts;
using System.Collections.Generic;

namespace Engine.Tiling.Models
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
		public Dictionary<(int row, int col), IAmATile> Tiles { get; set; }
	}
}

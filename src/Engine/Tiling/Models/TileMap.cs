using Engine.Signals.Models;
using Engine.Signals.Models.Contracts;
using System.Collections.Generic;

namespace Engine.Tiling.Models
{
	/// <summary>
	/// Represents a tile map.
	/// </summary>
	public class TileMap : IReceiveSignals
	{
		/// <summary>
		/// Gets or sets the tile map name.
		/// </summary>
		public string TileMapName { get; set; }

		/// <summary>
		/// Gets or sets the tile map layer.
		/// </summary>
		public Dictionary<int, TileMapLayer> TileMapLayers { get; set; }

		/// <summary>
		/// Gets the active signal subscriptions.
		/// </summary>
		public IList<SignalSubscription> ActiveSignalSubscriptions { get; set; }
	}
}

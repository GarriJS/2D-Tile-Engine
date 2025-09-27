using Common.Tiling.Models.Contracts;
using Engine.Physics.Models;
using Engine.Physics.Models.Contracts;
using Engine.RunTime.Models.Contracts;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Common.Tiling.Models
{
	/// <summary>
	/// Represents a tile map.
	/// </summary>
	public class TileMap : IAmDrawable, IHaveArea
	{
		/// <summary>
		/// Gets or sets the draw layer.
		/// </summary>
		public int DrawLayer { get; set; }

		/// <summary>
		/// Gets or sets the tile map name.
		/// </summary>
		public string TileMapName { get; set; }

		/// <summary>
		/// Gets the position.
		/// </summary>
		public Position Position { get => this.Area.Position; }

		/// <summary>
		/// Gets or sets the area.
		/// </summary>
		public IAmAArea Area { get; set; }

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

				return;
			}

			tileMapLayer = new TileMapLayer
			{ 
				Layer = layer,
			};

			tileMapLayer.AddTile(tile);
			this.TileMapLayers[layer] = tileMapLayer;
		}

		/// <summary>
		/// Draws the drawable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		public void Draw(GameTime gameTime, GameServiceContainer gameServices)
		{
			foreach (var tileMapLayer in this.TileMapLayers.Values)
			{
				tileMapLayer.Draw(gameTime, gameServices, this.Position);
			}
		}
	}
}

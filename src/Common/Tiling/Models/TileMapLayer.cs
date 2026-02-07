using Common.DiskModels.Tiling;
using Engine.Core.Files.Models.Contract;
using Engine.RunTime.Models.Contracts;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Common.Tiling.Models
{
	/// <summary>
	/// Represents a tile map layer.
	/// </summary>
	sealed public class TileMapLayer: IAmSubDrawable, ICanBeSerialized<TileMapLayerModel>
	{
		/// <summary>
		/// Gets or sets the layer.
		/// </summary>
		required public int Layer { get; set; }

		/// <summary>
		/// The tiles.
		/// </summary>
		readonly public Dictionary<(int row, int col), Tile> _tiles = [];

		/// <summary>
		/// Adds the tile.
		/// </summary>
		/// <param name="tile">The tile.</param>
		public void AddTile(Tile tile)
		{
			this._tiles[(tile.Row, tile.Column)] = tile;
		}

		/// <summary>
		/// Draws the sub drawable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		/// <param name="coordinates">The coordinates.</param>
		/// <param name="color">The color.</param>
		/// <param name="offset">The offset.</param>
		public void Draw(GameTime gameTime, GameServiceContainer gameServices, Vector2 coordinates, Color color, Vector2 offset = default)
		{
			foreach (var tile in this._tiles.Values)
				tile.Draw(gameTime, gameServices, coordinates, color, offset);
		}

		/// <summary>
		/// Converts the object to a serialization model.
		/// </summary>
		/// <returns>The serialization model.</returns>
		public TileMapLayerModel ToModel()
		{ 
			var tileModels = this._tiles.Values.Select(e => e.ToModel())
											  .ToArray();
			var result = new TileMapLayerModel
			{
				Layer = this.Layer,
				Tiles = tileModels
			};

			return result;
		}
	}
}

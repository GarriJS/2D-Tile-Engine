using Common.Tiling.Models.Contracts;
using Engine.Physics.Models;
using Engine.RunTime.Models.Contracts;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Common.Tiling.Models
{
	/// <summary>
	/// Represents a tile map layer.
	/// </summary>
	public class TileMapLayer: IAmSubDrawable
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

		/// <summary>
		/// Draws the sub drawable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		/// <param name="position">The position.</param>
		/// <param name="offset">The offset.</param>
		public void Draw(GameTime gameTime, GameServiceContainer gameServices, Position position, Vector2 offset = default)
		{
			foreach (var tile in this.Tiles.Values)
			{
				tile.Draw(gameTime, gameServices, position);
			}
		}
	}
}

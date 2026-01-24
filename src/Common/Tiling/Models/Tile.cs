using Common.DiskModels.Tiling;
using Engine.Core.Constants;
using Engine.Graphics.Models.Contracts;
using Engine.Physics.Models;
using Engine.Physics.Models.SubAreas;
using Microsoft.Xna.Framework;

namespace Common.Tiling.Models
{
	/// <summary>
	/// Represents a tile.
	/// </summary>
	public class Tile
	{
		/// <summary>
		/// Gets or sets the row.
		/// </summary>
		public int Row { get; set; }

		/// <summary>
		/// Gets or sets the columns.
		/// </summary>
		public int Column { get; set; }

		/// <summary>
		/// Gets or sets the draw layer.
		/// </summary>
		public int DrawLayer { get; set; }

		/// <summary>
		/// Gets or sets the area.
		/// </summary>
		public SubArea Area { get; set; }

		/// <summary>
		/// Gets the SimpleText.
		/// </summary>
		public IAmAGraphic Graphic { get; set; }

		/// <summary>
		/// Draws the sub drawable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		/// <param name="position">The position.</param>
		/// <param name="offset">The offset.</param>
		public void Draw(GameTime gameTime, GameServiceContainer gameServices, Position position, Vector2 offset = default)
		{
			var tileOffset = offset + new Vector2
			{
				X = this.Column * TileConstants.TILE_SIZE,
				Y = this.Row * TileConstants.TILE_SIZE
			};
			this.Graphic?.Draw(gameTime, gameServices, position, Color.White, tileOffset);
		}

		/// <summary>
		/// Converts the object to a serialization model.
		/// </summary>
		/// <returns>The serialization model.</returns>
		public TileModel ToModel()
		{
			var graphicModel = this.Graphic.ToModel();

			return new TileModel
			{
				Row = this.Row,
				Column = this.Column,
				Graphic = graphicModel,
			};
		}
	}
}

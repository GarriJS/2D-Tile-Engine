using Common.DiskModels.Tiling;
using Engine.Core.Constants;
using Engine.Graphics.Models.Contracts;
using Engine.Physics.Models.SubAreas;
using Engine.RunTime.Models.Contracts;
using Microsoft.Xna.Framework;

namespace Common.Tiling.Models
{
	/// <summary>
	/// Represents a tile.
	/// </summary>
	public class Tile : IAmSubDrawable
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
		/// <param name="coordinates">The coordinates.</param>
		/// <param name="color">The color.</param>
		/// <param name="offset">The offset.</param>
		public void Draw(GameTime gameTime, GameServiceContainer gameServices, Vector2 coordinates, Color color, Vector2 offset = default)
		{
			var tileOffset = offset + new Vector2
			{
				X = this.Column * TileConstants.TILE_SIZE,
				Y = this.Row * TileConstants.TILE_SIZE
			};
			this.Graphic?.Draw(gameTime, gameServices, coordinates, color, tileOffset);
		}

		/// <summary>
		/// Converts the object to a serialization model.
		/// </summary>
		/// <returns>The serialization model.</returns>
		public TileModel ToModel()
		{
			var graphicModel = this.Graphic.ToModel();
			var result = new TileModel
			{
				Row = this.Row,
				Column = this.Column,
				Graphic = graphicModel,
			};

			return result;
		}
	}
}

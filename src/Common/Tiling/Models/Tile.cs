using Common.DiskModel.Tiling.Contracts;
using Common.DiskModels.Tiling;
using Common.Tiling.Models.Contracts;
using Engine.Core.Constants;
using Engine.DiskModels.Drawing;
using Engine.Graphics.Models;
using Engine.Graphics.Models.Contracts;
using Engine.Physics.Models;
using Engine.Physics.Models.SubAreas;
using Microsoft.Xna.Framework;

namespace Common.Tiling.Models
{
	/// <summary>
	/// Represents a tile.
	/// </summary>
	public class Tile : IAmATile, IHaveAnImage
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
		/// Gets the Graphic.
		/// </summary>
		public IAmAGraphic Graphic { get => this.Image; }

		/// <summary>
		/// Gets the image.
		/// </summary>
		public SimpleImage Image { get; set; }

		/// <summary>
		/// Gets or sets the area.
		/// </summary>
		public SubArea Area { get; set; }

		/// <summary>
		/// Draws the sub drawable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		/// <param name="position">The position.</param>
		/// <param name="offset">The offset.</param>
		public void Draw(GameTime gameTime, GameServiceContainer gameServices, Position position, Vector2 offset = default)
		{
			var tileOffset = new Vector2
			{
				X = this.Column * TileConstants.TILE_SIZE,
				Y = this.Row * TileConstants.TILE_SIZE
			};
			this.Image?.Draw(gameTime, gameServices, position, tileOffset);
		}

		/// <summary>
		/// Converts the object to a serialization model.
		/// </summary>
		/// <returns>The serialization model.</returns>
		public IAmATileModel ToModel()
		{
			var imageModel = (SimpleImageModel)this.Image.ToModel();

			return new TileModel
			{
				Row = this.Row,
				Column = this.Column,
				Graphic = imageModel,
			};
		}
	}
}

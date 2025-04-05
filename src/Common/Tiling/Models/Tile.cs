using Common.Tiling.Models.Contracts;
using Engine.Drawables.Models;
using Engine.Physics.Models;
using Engine.Physics.Models.Contracts;

namespace Common.Tiling.Models
{
	/// <summary>
	/// Represents a tile.
	/// </summary>
	public class Tile : IAmATile
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
		/// Gets or sets the image.
		/// </summary>
		public Image Image { get; set; }

		/// <summary>
		/// Gets the position.
		/// </summary>
		public Position Position { get => this.Area.Position; }

		/// <summary>
		/// Gets or sets the area.
		/// </summary>
		public IAmAArea Area { get; set; }

		/// <summary>
		/// Disposes of the tile.
		/// </summary>
		public void Dispose()
		{ 
			this.Image.Dispose();
		}
	}
}

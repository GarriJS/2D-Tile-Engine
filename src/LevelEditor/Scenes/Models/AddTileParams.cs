using Engine.DiskModels.Drawing.Contracts;

namespace LevelEditor.Scenes.Models
{
	/// <summary>
	/// Represents a add tile parameters.
	/// </summary>
	public class AddTileParams
	{
		/// <summary>
		/// Gets or sets the tile graphic.
		/// </summary>
		public IAmAGraphicModel TileGraphic { get; set; }
	}
}

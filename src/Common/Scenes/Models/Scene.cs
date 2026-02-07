using Common.Tiling.Models;

namespace Common.Scenes.Models
{
	/// <summary>
	/// Represents a scene.
	/// </summary>
	sealed public class Scene
	{
		/// <summary>
		/// Gets or sets the scene name.
		/// </summary>
		required public string SceneName { get; set; }

		/// <summary>
		/// Gets or sets the tile map.
		/// </summary>
		required public TileMap TileMap { get; set; }
	}
}

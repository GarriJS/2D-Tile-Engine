using Common.Tiling.Models;

namespace Common.Scenes.Models
{
	/// <summary>
	/// Represents a scene.
	/// </summary>
	public class Scene
	{
		/// <summary>
		/// Gets or sets the scene name.
		/// </summary>
		public string SceneName { get; set; }

		/// <summary>
		/// Gets or sets the tile map.
		/// </summary>
		public TileMap TileMap { get; set; }
	}
}

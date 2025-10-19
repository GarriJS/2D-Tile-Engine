using Common.DiskModels.Tiling;
using System.Text.Json.Serialization;

namespace Common.DiskModels.Scenes
{
	public class SceneModel
	{
		[JsonPropertyName("sceneName")]
		public string SceneName { get; set; }

		[JsonPropertyName("tileMap")]
		public TileMapModel TileMap { get; set; }
	}
}

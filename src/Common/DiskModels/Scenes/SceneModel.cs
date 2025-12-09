using Common.DiskModels.Tiling;
using Engine.DiskModels;
using System.Text.Json.Serialization;

namespace Common.DiskModels.Scenes
{
	public class SceneModel : BaseDiskModel
	{
		[JsonPropertyName("sceneName")]
		public string SceneName { get; set; }

		[JsonPropertyName("tileMap")]
		public TileMapModel TileMap { get; set; }
	}
}

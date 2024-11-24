using DiscModels.Engine.Tiling;
using System.Runtime.Serialization;

namespace DiscModels.Engine.Scenes
{
	[DataContract(Name = "scene")]
	public class SceneModel
	{
		[DataMember(Name = "sceneName", Order = 1)]
		public required string SceneName { get; set; }

		[DataMember(Name = "tileMap", Order = 2)]
		public required TileMapModel TileMap { get; set; }
	}
}

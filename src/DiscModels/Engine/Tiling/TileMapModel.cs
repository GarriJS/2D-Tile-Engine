using System.Runtime.Serialization;

namespace DiscModels.Engine.Tiling
{
	[DataContract(Name = "tileMapModel")]
	public class TileMapModel
	{
		[DataMember(Name = "tileMapName", Order = 1)]
		public required string TileMapName { get; set; }

		[DataMember(Name = "tileMapLayers", Order = 2)]
		public required List<TileMapLayerModel> TileMapLayers { get; set; }
	}
}

using System.Runtime.Serialization;

namespace DiscModels.Common.Tiling
{
    [DataContract(Name = "tileMapModel")]
    public class TileMapModel
    {
        [DataMember(Name = "tileMapName", Order = 1)]
        public string TileMapName { get; set; }

        [DataMember(Name = "tileMapLayers", Order = 2)]
        public List<TileMapLayerModel> TileMapLayers { get; set; }
    }
}

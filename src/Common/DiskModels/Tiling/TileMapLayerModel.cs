using Engine.DiskModels;
using System.Text.Json.Serialization;

namespace Common.DiskModels.Tiling
{
    public class TileMapLayerModel : BaseDiskModel
    {
        [JsonPropertyName("layer")]
        public int Layer { get; set; }

        [JsonPropertyName("tiles")]
        public TileModel[] Tiles { get; set; }
    }
}

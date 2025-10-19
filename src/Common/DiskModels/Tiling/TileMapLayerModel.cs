using Common.DiskModel.Tiling.Contracts;
using System.Text.Json.Serialization;

namespace Common.DiskModels.Tiling
{
    public class TileMapLayerModel
    {
        [JsonPropertyName("layer")]
        public int Layer { get; set; }

        [JsonPropertyName("tiles")]
        public IAmATileModel[] Tiles { get; set; }
    }
}

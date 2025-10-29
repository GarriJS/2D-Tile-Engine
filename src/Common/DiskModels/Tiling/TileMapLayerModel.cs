using System.Text.Json.Serialization;

namespace Common.DiskModels.Tiling
{
    public class TileMapLayerModel
    {
        [JsonPropertyName("layer")]
        public int Layer { get; set; }

        [JsonPropertyName("tiles")]
        public TileModel[] Tiles { get; set; }
    }
}

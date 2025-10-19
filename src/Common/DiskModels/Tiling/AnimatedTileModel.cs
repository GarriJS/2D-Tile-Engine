using Common.DiskModel.Tiling.Contracts;
using Engine.DiskModels.Drawing;
using System.Text.Json.Serialization;

namespace Common.DiskModels.Tiling
{
    public class AnimatedTileModel : IAmATileModel
    {
        [JsonPropertyName("row")]
        public int Row { get; set; }

        [JsonPropertyName("column")]   
        public int Column { get; set; }

        [JsonPropertyName("animation")]
        public AnimationModel Animation { get; set; }
    }
}

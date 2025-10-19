using Common.DiskModel.Tiling.Contracts;
using Engine.DiskModels.Drawing;
using System.Text.Json.Serialization;

namespace Common.DiskModels.Tiling
{
    public class TileModel : IAmATileModel
    {
        [JsonPropertyName("row")]
        public int Row { get; set; }

        [JsonPropertyName("column")]
        public int Column { get; set; }

		[JsonPropertyName("imageId")]
		public int ImageId { get; set; }

		[JsonIgnore]
		public ImageModel Image { get; set; }
    }
}

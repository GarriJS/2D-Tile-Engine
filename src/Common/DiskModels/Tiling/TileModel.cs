using Engine.DiskModels;
using Engine.DiskModels.Drawing.Contracts;
using System.Text.Json.Serialization;

namespace Common.DiskModels.Tiling
{
    public class TileModel : BaseDiskModel
	{
        [JsonPropertyName("row")]
        public int Row { get; set; }

        [JsonPropertyName("column")]
        public int Column { get; set; }

		[JsonPropertyName("graphicId")]
		public int GraphicId { get; set; }

		[JsonIgnore]
		public IAmAGraphicModel Graphic { get; set; }
    }
}

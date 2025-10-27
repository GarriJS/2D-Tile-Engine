using Common.DiskModel.Tiling.Contracts;
using Engine.Graphics.Models.Contracts;
using System.Text.Json.Serialization;

namespace Common.DiskModels.Tiling
{
    public class TileModel : IAmATileModel
    {
        [JsonPropertyName("row")]
        public int Row { get; set; }

        [JsonPropertyName("column")]
        public int Column { get; set; }

		[JsonPropertyName("graphicId")]
		public int GraphicId { get; set; }

		[JsonIgnore]
		public IAmAGraphic Graphic { get; set; }
    }
}

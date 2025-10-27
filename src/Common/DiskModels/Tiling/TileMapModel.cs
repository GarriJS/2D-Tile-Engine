using Engine.DiskModels.Drawing.Contracts;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Common.DiskModels.Tiling
{
    public class TileMapModel
    {
        [JsonPropertyName("tileMapName")]
        public string TileMapName { get; set; }

        [JsonPropertyName("tileMapLayers")]
        public TileMapLayerModel[] TileMapLayers { get; set; }

		[JsonPropertyName("images")]
		public Dictionary<int, IAmAImageModel> Images { get; set; }
	}
}

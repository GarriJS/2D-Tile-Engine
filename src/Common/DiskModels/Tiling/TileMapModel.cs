using Engine.DiskModels;
using Engine.DiskModels.Drawing.Contracts;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Common.DiskModels.Tiling
{
    public class TileMapModel : BaseDiskModel
	{
        [JsonPropertyName("tileMapName")]
        public string TileMapName { get; set; }

        [JsonPropertyName("tileMapLayers")]
        public TileMapLayerModel[] TileMapLayers { get; set; }

		[JsonIgnore]
		[JsonPropertyName("graphics")]
		public Dictionary<int, IAmAGraphicModel> Graphics { get; set; }
	}
}

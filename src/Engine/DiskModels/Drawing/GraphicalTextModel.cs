using Microsoft.Xna.Framework;
using System.Text.Json.Serialization;

namespace Engine.DiskModels.Drawing
{
	public class GraphicalTextModel : BaseDiskModel
	{
		[JsonPropertyName("textColor")]
		public Color TextColor { get; set; }

		[JsonPropertyName("fontName")]
		public string FontName { get; set; }

        [JsonPropertyName("maxLineCharacterCount")]
        public int? MaxLineCharacterCount { get; set; }

        [JsonPropertyName("maxLinesCount")]
        public int? MaxLinesCount { get; set; }

        [JsonPropertyName("textLines")]
        public TextLineModel[] TextLines { get; set; }
    }
}

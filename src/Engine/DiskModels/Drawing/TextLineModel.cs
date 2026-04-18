using System.Text.Json.Serialization;

namespace Engine.DiskModels.Drawing
{
    public class TextLineModel : BaseDiskModel
    {
        [JsonPropertyName("isManualBreak")]
        public bool IsManualBreak { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }
    }
}

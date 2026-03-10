using System.Text.Json.Serialization;

namespace Engine.DiskModels.Drawing
{
	public class SimpleTextModel : GraphicalTextModel
	{
		[JsonPropertyName("maxLineWidth")]
		public float? MaxLineWidth { get; set; }
	}
}

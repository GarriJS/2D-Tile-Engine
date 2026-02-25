using Engine.DiskModels.Drawing.Abstract;
using System.Text.Json.Serialization;

namespace Engine.DiskModels.Drawing
{
	public class SimpleTextModel : GraphicalTextBaseModel
	{
		[JsonPropertyName("maxLineWidth")]
		public float? MaxLineWidth { get; set; }
	}
}

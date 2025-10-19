using System.Text.Json.Serialization;

namespace Engine.DiskModels.Physics
{
	public class OffsetAreaModel : AreaModel
	{
		[JsonPropertyName("horizontalOffset")]
		public float HorizontalOffset { get; set; }

		[JsonPropertyName("verticalOffset")]
		public float VerticalOffset { get; set; }
	}
}

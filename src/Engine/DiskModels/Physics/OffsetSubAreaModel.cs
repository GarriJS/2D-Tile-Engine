using System.Text.Json.Serialization;

namespace Engine.DiskModels.Physics
{
	public class OffsetSubAreaModel : SubAreaModel
	{
		[JsonPropertyName("horizontalOffset")]
		public float HorizontalOffset { get; set; }

		[JsonPropertyName("verticalOffset")]
		public float VerticalOffset { get; set; }
	}
}

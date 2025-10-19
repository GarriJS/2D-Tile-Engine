using System.Text.Json.Serialization;

namespace Engine.DiskModels.Physics
{
	public class PositionModel
	{
		[JsonPropertyName("x")]
		public float X { get; set; }

		[JsonPropertyName("y")]
		public float Y { get; set; }
	}
}

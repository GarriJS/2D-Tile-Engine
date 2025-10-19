using System.Text.Json.Serialization;

namespace Engine.DiskModels.Physics
{
	public class SubAreaModel
	{
		[JsonPropertyName("width")]
		public float Width { get; set; }

		[JsonPropertyName("height")]
		public float Height { get; set; }
	}
}

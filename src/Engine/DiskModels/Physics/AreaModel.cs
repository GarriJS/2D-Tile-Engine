using Engine.DiskModels.Physics.Contracts;
using System.Text.Json.Serialization;

namespace Engine.DiskModels.Physics
{
	public class AreaModel : IAmAAreaModel
	{
		[JsonPropertyName("width")]
		public float Width { get; set; }

		[JsonPropertyName("height")]
		public float Height { get; set; }

		[JsonPropertyName("position")]
		public PositionModel Position { get; set; }
	}
}

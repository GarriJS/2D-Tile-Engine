using System.Text.Json.Serialization;

namespace Engine.DiskModels.Physics.Contracts
{
	public interface IAmAAreaModel
	{
		[JsonPropertyName("width")]
		public float Width { get; set; }

		[JsonPropertyName("height")]
		public float Height { get; set; }

		[JsonPropertyName("position")]
		public PositionModel Position { get; set; }
	}
}

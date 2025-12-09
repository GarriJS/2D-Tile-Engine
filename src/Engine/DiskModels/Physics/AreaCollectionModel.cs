using Engine.DiskModels.Physics.Contracts;
using System.Text.Json.Serialization;

namespace Engine.DiskModels.Physics
{
	public class AreaCollectionModel : BaseDiskModel, IAmAAreaModel
	{
		[JsonPropertyName("width")]
		public float Width { get; set; }

		[JsonPropertyName("height")]
		public float Height { get; set; }

		[JsonPropertyName("position")]
		public PositionModel Position { get; set; }

		[JsonPropertyName("subAreas")]
		public OffsetSubAreaModel[] SubAreas { get; set; }
	}
}

using Engine.DiskModels.Physics.Contracts;
using Engine.Physics.Models;
using System.Runtime.Serialization;

namespace Engine.DiskModels.Physics
{
	public class AreaCollectionModel : IAmAAreaModel
	{
		[DataMember(Name = "width", Order = 1)]
		public float Width { get; set; }

		[DataMember(Name = "height", Order = 2)]
		public float Height { get; set; }

		[DataMember(Name = "position", Order = 3)]
		public PositionModel Position { get; set; }

		[DataMember(Name = "areas", Order = 4)]
		public OffsetSubAreaModel[] Areas { get; set; }
	}
}

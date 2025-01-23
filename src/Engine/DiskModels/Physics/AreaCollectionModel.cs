using Engine.DiskModels.Engine.Physics.Contracts;
using System.Runtime.Serialization;

namespace Engine.DiskModels.Engine.Physics
{
	public class AreaCollectionModel : IAmAAreaModel
	{
		[DataMember(Name = "hasCollision", Order = 1)]
		public bool HasCollision { get; set; }

		[DataMember(Name = "width", Order = 2)]
		public float Width { get; set; }

		[DataMember(Name = "height", Order = 3)]
		public float Height { get; set; }

		[DataMember(Name = "position", Order = 4)]
		public PositionModel? Position { get; set; }

		[DataMember(Name = "areas", Order = 5)]
		public SimpleAreaModel[] Areas { get; set; }
	}
}

using Engine.DiskModels.Engine.Physics.Contracts;
using System.Runtime.Serialization;

namespace Engine.DiskModels.Engine.Physics
{
	public class AreaCollectionModel : IAmAAreaModel
	{
		[DataMember(Name = "width", Order = 1)]
		public float Width { get; set; }

		[DataMember(Name = "height", Order = 2)]
		public float Height { get; set; }

		[DataMember(Name = "areas", Order = 3)]
		public SimpleAreaModel[] Areas { get; set; }
	}
}

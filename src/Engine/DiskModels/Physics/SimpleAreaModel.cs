using Engine.DiskModels.Physics.Contracts;
using System.Runtime.Serialization;

namespace Engine.DiskModels.Physics
{
	[DataContract(Name = "simpleArea")]
	public class SimpleAreaModel : IAmAAreaModel
	{
		[DataMember(Name = "width", Order = 1)]
		public float Width { get; set; }

		[DataMember(Name = "height", Order = 2)]
		public float Height { get; set; }
	}
}

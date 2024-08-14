using DiscModels.Engine.Physics.Contracts;
using System.Runtime.Serialization;

namespace DiscModels.Engine.Physics
{
	[DataContract(Name = "simpleArea")]
	public class SimpleAreaModel : IAmAAreaModel
	{
		[DataMember(Name = "width", Order = 1)]
		public float Width { get; set; }

		[DataMember(Name = "height", Order = 2)]
		public float Height { get; set; }

		[DataMember(Name = "position", Order = 3)]
		public PositionModel Position { get; set; }

		[DataMember(Name = "position", Order = 4)]
		public string[] CollisionTypes { get; set; }
	}
}

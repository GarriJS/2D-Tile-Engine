using DiscModels.Engine.Physics.Contracts;
using System.Runtime.Serialization;

namespace DiscModels.Engine.Physics
{
	[DataContract(Name = "simpleArea")]
	public class SimpleAreaModel : IAmAAreaModel
	{
		[DataMember(Name = "hasCollision", Order = 1)]
		public bool HasCollision { get; set; }

		[DataMember(Name = "width", Order = 2)]
		public float Width { get; set; }

		[DataMember(Name = "height", Order = 3)]
		public float Height { get; set; }

		[DataMember(Name = "position", Order = 4)]
		public PositionModel? Position { get; set; }

		[DataMember(Name = "collisionTypes", Order = 5)]
		public required string[] CollisionTypes { get; set; }
	}
}

using Engine.DiskModels.Physics.Contracts;
using Microsoft.Xna.Framework;
using System.Runtime.Serialization;

namespace Engine.DiskModels.Physics
{
	public class AreaCollectionModel : IAmAAreaModel
	{
		[DataMember(Name = "position", Order = 1)]
		public PositionModel Position { get; set; }

		[DataMember(Name = "width", Order = 2)]
		public float Width { get; set; }

		[DataMember(Name = "height", Order = 3)]
		public float Height { get; set; }

		[DataMember(Name = "areas", Order = 4)]
		public (Vector2 offset, Vector2 dimensions)[] Areas { get; set; }
	}
}

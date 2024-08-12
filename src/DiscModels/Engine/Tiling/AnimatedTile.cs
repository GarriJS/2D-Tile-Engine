using DiskModels.Engine.Drawing;
using DiskModels.Engine.Physics.Contracts;
using System.Runtime.Serialization;

namespace DiskModels.Engine.Tiling
{
	[DataContract(Name = "animatedTile")]
	public class AnimatedTile
	{
		[DataMember(Name = "row", Order = 1)]
		public int Row { get; set; }

		[DataMember(Name = "column", Order = 2)]
		public int Column { get; set; }

		[DataMember(Name = "animation", Order = 3)]
		public AnimationModel Animation { get; set; }

		[DataMember(Name = "area", Order = 4)]
		public IAmAAreaModel Area { get; set; }
	}
}

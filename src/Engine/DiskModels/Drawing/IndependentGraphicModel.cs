using Engine.DiskModels.Drawing.Contracts;
using Engine.DiskModels.Physics;
using System.Runtime.Serialization;

namespace Engine.DiskModels.Drawing
{
	[DataContract(Name = "independentGraphic")]
	public class IndependentGraphicModel
	{
		[DataMember(Name = "position", Order = 1)]
		public PositionModel Position { get; set; }

		[DataMember(Name = "graphic", Order = 2)]
		public IAmAGraphicModel Graphic { get; set; }
	}
}

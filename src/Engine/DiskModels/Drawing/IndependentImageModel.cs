using Engine.DiskModels.Physics;
using System.Runtime.Serialization;

namespace Engine.DiskModels.Drawing
{
	[DataContract(Name = "independentImage")]
	public class IndependentImageModel : ImageModel
	{
		[DataMember(Name = "position", Order = 3)]
		public PositionModel Position { get; set; }
	}
}

using System.Runtime.Serialization;

namespace Engine.DiskModels.Engine.Physics
{
	[DataContract(Name = "offsetArea")]
	public class OffsetAreaModel : SimpleAreaModel
	{
		[DataMember(Name = "horizontalOffset", Order = 6)]
		public float HorizontalOffset { get; set; }

		[DataMember(Name = "verticalOffset", Order = 7)]
		public float VerticalOffset { get; set; }
	}
}

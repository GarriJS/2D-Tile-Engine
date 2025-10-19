using System.Runtime.Serialization;

namespace Engine.DiskModels.Physics
{
	[DataContract(Name = "offsetArea")]
	public class OffsetAreaModel : AreaModel
	{
		[DataMember(Name = "horizontalOffset", Order = 4)]
		public float HorizontalOffset { get; set; }

		[DataMember(Name = "verticalOffset", Order = 5)]
		public float VerticalOffset { get; set; }
	}
}

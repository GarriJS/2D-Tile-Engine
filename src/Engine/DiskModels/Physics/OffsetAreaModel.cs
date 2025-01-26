using System.Runtime.Serialization;

namespace Engine.DiskModels.Physics
{
	[DataContract(Name = "offsetArea")]
	public class OffsetAreaModel : SimpleAreaModel
	{
		[DataMember(Name = "horizontalOffset", Order = 3)]
		public float HorizontalOffset { get; set; }

		[DataMember(Name = "verticalOffset", Order = 4)]
		public float VerticalOffset { get; set; }
	}
}

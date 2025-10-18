using System.Runtime.Serialization;

namespace Engine.DiskModels.Physics
{
	public class OffsetSubAreaModel : SubAreaModel
	{
		[DataMember(Name = "horizontalOffset", Order = 3)]
		public float HorizontalOffset { get; set; }

		[DataMember(Name = "verticalOffset", Order = 4)]
		public float VerticalOffset { get; set; }
	}
}

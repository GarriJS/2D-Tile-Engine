using System.Runtime.Serialization;

namespace DiscModels.Engine.Physics
{
	[DataContract(Name = "offsetArea")]
	public class OffsetAreaModel : SimpleAreaModel
	{
		[DataMember(Name = "horizontalOffset", Order = 5)]
		public float HorizontalOffset { get; set; }

		[DataMember(Name = "verticalOffset", Order = 6)]
		public float VerticalOffset { get; set; }
	}
}

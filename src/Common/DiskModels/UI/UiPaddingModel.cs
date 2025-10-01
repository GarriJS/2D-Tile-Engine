using System.Runtime.Serialization;

namespace Common.DiskModels.UI
{
	[DataContract(Name = "uiPadding")]
	public struct UiPaddingModel
	{
		[DataMember(Name = "topPadding", Order = 1)]
		public float TopPadding { get; set; }

		[DataMember(Name = "bottomPadding", Order = 2)]
		public float BottomPadding { get; set; }

		[DataMember(Name = "leftPadding", Order = 3)]
		public float LeftPadding { get; set; }

		[DataMember(Name = "rightPadding", Order = 4)]
		public float RightPadding { get; set; }
	}
}

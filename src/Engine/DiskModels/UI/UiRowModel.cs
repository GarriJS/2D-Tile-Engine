using Engine.DiskModels.UI.Contracts;
using System.Runtime.Serialization;

namespace Engine.DiskModels.UI
{
	[DataContract(Name = "uiRow")]
	public class UiRowModel
	{
		[DataMember(Name = "uiRowName", Order = 1)]
		public string UiRowName { get; set; }

		[DataMember(Name = "topPadding", Order = 2)]
		public int TopPadding { get; set; }

		[DataMember(Name = "bottomPadding", Order = 3)]
		public int BottomPadding { get; set; }

		[DataMember(Name = "horizontalJustificationType", Order = 4)]
		public int HorizontalJustificationType { get; set; }

		[DataMember(Name = "verticalJustificationType", Order = 5)]
		public int VerticalJustificationType { get; set; }

		[DataMember(Name = "backgroundTextureName", Order = 6)]
		public string BackgroundTextureName { get; set; }

		[DataMember(Name = "subElements", Order = 7)]
		public IAmAUiElementModel[] SubElements { get; set; }
	}
}

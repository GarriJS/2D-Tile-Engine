using Common.DiskModels.UI.Contracts;
using Engine.DiskModels.Drawing;
using System.Runtime.Serialization;

namespace Common.DiskModels.UI
{
	[DataContract(Name = "uiRow")]
	public class UiRowModel
	{
		[DataMember(Name = "uiRowName", Order = 1)]
		public string UiRowName { get; set; }

		[DataMember(Name = "resizeTexture", Order = 2)]
		public bool ResizeTexture { get; set; }

		[DataMember(Name = "topPadding", Order = 3)]
		public int TopPadding { get; set; }

		[DataMember(Name = "bottomPadding", Order = 4)]
		public int BottomPadding { get; set; }

		[DataMember(Name = "horizontalJustificationType", Order = 5)]
		public int HorizontalJustificationType { get; set; }

		[DataMember(Name = "verticalJustificationType", Order = 6)]
		public int VerticalJustificationType { get; set; }

		[DataMember(Name = "rowHoverCursorName", Order = 7)]
		public string RowHoverCursorName { get; set; }

		[DataMember(Name = "backgroundTexture", Order = 8)]
		public ImageModel BackgroundTexture { get; set; }

		[DataMember(Name = "subElements", Order = 9)]
		public IAmAUiElementModel[] SubElements { get; set; }
	}
}

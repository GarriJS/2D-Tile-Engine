using Common.DiskModels.UI.Contracts;
using Common.UserInterface.Enums;
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

		[DataMember(Name = "outsidePadding", Order = 3)]
		public UiPaddingModel OutsidePadding { get; set; }

		[DataMember(Name = "insidePadding", Order = 4)]
		public UiPaddingModel InsidePadding { get; set; }

		[DataMember(Name = "horizontalJustificationType", Order = 5)]
		public UiRowHorizontalJustificationType HorizontalJustificationType { get; set; }

		[DataMember(Name = "verticalJustificationType", Order = 6)]
		public UiRowVerticalJustificationType VerticalJustificationType { get; set; }

		[DataMember(Name = "rowHoverCursorName", Order = 7)]
		public string RowHoverCursorName { get; set; }

		[DataMember(Name = "backgroundTexture", Order = 8)]
		public ImageModel BackgroundTexture { get; set; }

		[DataMember(Name = "subElements", Order = 9)]
		public IAmAUiElementModel[] SubElements { get; set; }
	}
}

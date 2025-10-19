using Common.DiskModels.UI.Contracts;
using Common.UserInterface.Enums;
using Engine.DiskModels.Drawing;
using System.Text.Json.Serialization;

namespace Common.DiskModels.UI
{
	public class UiRowModel
	{
		[JsonPropertyName("uiRowName")]
		public string UiRowName { get; set; }

		[JsonPropertyName("resizeTexture")]
		public bool ResizeTexture { get; set; }

		[JsonPropertyName("outsidePadding")]
		public UiPaddingModel OutsidePadding { get; set; }

		[JsonPropertyName("insidePadding")]
		public UiPaddingModel InsidePadding { get; set; }

		[JsonPropertyName("horizontalJustificationType")]
		public UiRowHorizontalJustificationType HorizontalJustificationType { get; set; }

		[JsonPropertyName("verticalJustificationType")]
		public UiRowVerticalJustificationType VerticalJustificationType { get; set; }

		[JsonPropertyName("rowHoverCursorName")]
		public string RowHoverCursorName { get; set; }

		[JsonPropertyName("backgroundTexture")]
		public ImageModel BackgroundTexture { get; set; }

		[JsonPropertyName("subElements")]
		public IAmAUiElementModel[] SubElements { get; set; }
	}
}

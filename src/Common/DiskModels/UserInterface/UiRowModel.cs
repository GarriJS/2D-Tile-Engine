using Common.DiskModels.UserInterface.Contracts;
using Common.UserInterface.Enums;
using Engine.DiskModels.Drawing;
using System.Text.Json.Serialization;

namespace Common.DiskModels.UserInterface
{
	public class UiRowModel
	{
		[JsonPropertyName("uiRowName")]
		public string UiRowName { get; set; }

		[JsonPropertyName("resizeTexture")]
		public bool ResizeTexture { get; set; }

		[JsonPropertyName("margin")]
		public UiMarginModel Margin { get; set; }

		[JsonPropertyName("horizontalJustificationType")]
		public UiRowHorizontalJustificationType HorizontalJustificationType { get; set; }

		[JsonPropertyName("verticalJustificationType")]
		public UiRowVerticalJustificationType VerticalJustificationType { get; set; }

		[JsonPropertyName("rowHoverCursorName")]
		public string RowHoverCursorName { get; set; }

		[JsonPropertyName("backgroundTexture")]
		public SimpleImageModel BackgroundTexture { get; set; }

		[JsonPropertyName("subElements")]
		public IAmAUiElementModel[] SubElements { get; set; }
	}
}

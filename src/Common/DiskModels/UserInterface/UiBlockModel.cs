using Common.UserInterface.Enums;
using Engine.DiskModels;
using Engine.DiskModels.Drawing;
using System.Text.Json.Serialization;

namespace Common.DiskModels.UserInterface
{
	public class UiBlockModel : BaseDiskModel
	{
		[JsonPropertyName("uiBlockName")]
		public string UiBlockName { get; set; }

		[JsonPropertyName("resizeTexture")]
		public bool ResizeTexture { get; set; }

		[JsonPropertyName("margin")]
		public UiMarginModel Margin { get; set; }

		[JsonPropertyName("horizontalJustificationType")]
		public UiHorizontalJustificationType HorizontalJustificationType { get; set; }

		[JsonPropertyName("verticalJustificationType")]
		public UiVerticalJustificationType VerticalJustificationType { get; set; }

		[JsonPropertyName("rowHoverCursorName")]
		public string BlockHoverCursorName { get; set; }

		[JsonPropertyName("backgroundTexture")]
		public SimpleImageModel BackgroundTexture { get; set; }

		[JsonPropertyName("rows")]
		public UiRowModel[] Rows { get; set; }
	}
}

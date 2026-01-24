using Common.DiskModels.UserInterface.Contracts;
using Common.UserInterface.Enums;
using Engine.DiskModels;
using Engine.DiskModels.Drawing;
using System.Text.Json.Serialization;

namespace Common.DiskModels.UserInterface
{
	public class UiRowModel : BaseDiskModel
	{
		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("resizeTexture")]
		public bool ResizeTexture { get; set; }

		[JsonPropertyName("extendBackgroundToMargin")]
		public bool ExtendBackgroundToMargin { get; set; }

		[JsonPropertyName("margin")]
		public UiMarginModel Margin { get; set; }

		[JsonPropertyName("horizontalJustificationType")]
		public UiHorizontalJustificationType HorizontalJustificationType { get; set; }

		[JsonPropertyName("verticalJustificationType")]
		public UiVerticalJustificationType VerticalJustificationType { get; set; }

		[JsonPropertyName("rowHoverCursorName")]
		public string RowHoverCursorName { get; set; }

		[JsonPropertyName("backgroundTexture")]
		public SimpleImageModel BackgroundTexture { get; set; }

		[JsonPropertyName("elements")]
		public IAmAUiElementModel[] Elements { get; set; }
	}
}

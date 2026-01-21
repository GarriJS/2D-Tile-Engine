using Common.UserInterface.Enums;
using Engine.DiskModels;
using Engine.DiskModels.Drawing;
using System.Text.Json.Serialization;

namespace Common.DiskModels.UserInterface
{
	public class UiZoneModel : BaseDiskModel
	{
		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("resizeTexture")]
		public bool ResizeTexture { get; set; }

		[JsonPropertyName("verticalJustificationType")]
		public UiVerticalJustificationType VerticalJustificationType { get; set; }

		[JsonPropertyName("uiZoneType")]
		public UiZonePositionType UiZonePositionType { get; set; }

		[JsonPropertyName("hoverCursorName")]
		public string HoverCursorName { get; set; }

		[JsonPropertyName("hoverEventName")]
		public string HoverEventName { get; set; }

		[JsonPropertyName("backgroundTexture")]
		public SimpleImageModel BackgroundTexture { get; set; }

		[JsonPropertyName("blocks")]
		public UiBlockModel[] Blocks { get; set; }
	}
}

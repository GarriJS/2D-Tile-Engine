using Common.UserInterface.Enums;
using Engine.DiskModels;
using Engine.DiskModels.Drawing;
using Microsoft.Xna.Framework;
using System.Text.Json.Serialization;

namespace Common.DiskModels.UserInterface
{
	public class UiModalModel : BaseDiskModel
	{
		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("resizeTexture")]
		public bool ResizeTexture { get; set; }

		[JsonPropertyName("horizontalLocationType")]
		public UiModalHorizontalLocationType HorizontalLocationType { get; set; }

		[JsonPropertyName("verticalLocationType")]
		public UiModalVerticalLocationType VerticalLocationType { get; set; }

		[JsonPropertyName("horizontalJustificationType")]
		public UiHorizontalJustificationType HorizontalJustificationType { get; set; }

		[JsonPropertyName("verticalJustificationType")]
		public UiVerticalJustificationType VerticalJustificationType { get; set; }

		[JsonPropertyName("horizontalModalSizeType")]
		public UiModalSizeType HorizontalModalSizeType { get; set; }

		[JsonPropertyName("verticalModalSizeType")]
		public UiModalSizeType VerticalModalSizeType { get; set; }

		[JsonPropertyName("hoverCursorName")]
		public string HoverCursorName { get; set; }

		[JsonPropertyName("hoverEventName")]
		public string HoverEventName { get; set; }

		[JsonPropertyName("fixedSized")]
		public Vector2? FixedSized { get; set; }

		[JsonPropertyName("scrollStateModel")]
		public ScrollStateModel ScrollStateModel { get; set; }

		[JsonPropertyName("backgroundTexture")]
		public SimpleImageModel BackgroundTexture { get; set; }

		[JsonPropertyName("blocks")]
		public UiBlockModel[] Blocks { get; set; }
	}
}

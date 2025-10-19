using Common.UserInterface.Enums;
using Engine.DiskModels.Drawing;
using Microsoft.Xna.Framework;
using System.Text.Json.Serialization;

namespace Common.DiskModels.UI.Contracts
{
	public interface IAmAUiElementModel
	{
		[JsonPropertyName("uiElementName")]
		public string UiElementName { get; set; }

		[JsonPropertyName("resizeTexture")]
		public bool ResizeTexture { get; set; }

		[JsonPropertyName("horizontalSizeType")]
		public UiElementSizeType HorizontalSizeType { get; set; }

		[JsonPropertyName("verticalSizeType")]
		public UiElementSizeType VerticalSizeType { get; set; }

		[JsonPropertyName("outsidePadding")]
		public UiPaddingModel OutsidePadding { get; set; }

		[JsonPropertyName("insidePadding")]
		public UiPaddingModel InsidePadding { get; set; }

		[JsonPropertyName("fixedSized")]
		public Vector2? FixedSized { get; set; }

		[JsonPropertyName("hoverEventName")]
		public string HoverCursorName { get; set; }

		[JsonPropertyName("hoverEventName")]
		public string HoverEventName { get; set; }

		[JsonPropertyName("pressEventName")]
		public string PressEventName { get; set; }

		[JsonPropertyName("texture")]
		public ImageModel Texture { get; set; }
	}
}

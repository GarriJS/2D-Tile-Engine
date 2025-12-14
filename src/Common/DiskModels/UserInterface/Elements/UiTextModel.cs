using Common.DiskModels.UserInterface.Contracts;
using Common.UserInterface.Enums;
using Engine.DiskModels.Drawing;
using Engine.DiskModels.Drawing.Abstract;
using Microsoft.Xna.Framework;
using System.Text.Json.Serialization;

namespace Common.DiskModels.UserInterface.Elements
{
	public class UiTextModel : IAmAUiElementWithTextModel
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
		public UiMarginModel OutsidePadding { get; set; }

		[JsonPropertyName("insidePadding")]
		public UiMarginModel Margin { get; set; }

		[JsonPropertyName("fixedSized")]
		public Vector2? FixedSized { get; set; }

		[JsonPropertyName("hoverEventName")]
		public string HoverCursorName { get; set; }

		[JsonPropertyName("hoverEventName")]
		public string HoverEventName { get; set; }

		[JsonPropertyName("pressEventName")]
		public string PressEventName { get; set; }

		[JsonPropertyName("graphic")]
		public GraphicBaseModel Graphic { get; set; }

		[JsonPropertyName("text")]
		public GraphicalTextModel Text { get; set; }
	}
}

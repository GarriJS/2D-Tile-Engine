using Common.DiskModels.UI.Contracts;
using Common.UserInterface.Enums;
using Engine.DiskModels.Drawing;
using Microsoft.Xna.Framework;
using System.Runtime.Serialization;

namespace Common.DiskModels.UI.Elements
{
	[DataContract(Name = "uiButton")]
	public class UiButtonModel : IAmAUiElementWithTextModel
	{
		[DataMember(Name = "uiElementName", Order = 1)]
		public string UiElementName { get; set; }

		[DataMember(Name = "resizeTexture", Order = 2)]
		public bool ResizeTexture { get; set; }

		[DataMember(Name = "horizontalSizeType", Order = 3)]
		public UiElementSizeType HorizontalSizeType { get; set; }

		[DataMember(Name = "verticalSizeType", Order = 4)]
		public UiElementSizeType VerticalSizeType { get; set; }

		[DataMember(Name = "outsidePadding", Order = 5)]
		public UiPaddingModel OutsidePadding { get; set; }

		[DataMember(Name = "insidePadding", Order = 6)]
		public UiPaddingModel InsidePadding { get; set; }

		[DataMember(Name = "fixedSized", Order = 7)]
		public Vector2? FixedSized { get; set; }

		[DataMember(Name = "hoverCursorName", Order = 8)]
		public string HoverCursorName { get; set; }

		[DataMember(Name = "hoverEventName", Order = 9)]
		public string HoverEventName { get; set; }

		[DataMember(Name = "pressEventName", Order = 10)]
		public string PressEventName { get; set; }

		[DataMember(Name = "texture", Order = 11)]
		public ImageModel Texture { get; set; }

		[DataMember(Name = "text", Order = 12)]
		public GraphicalTextModel Text { get; set; }

		[DataMember(Name = "clickableAreaAnimation", Order = 13)]
		public TriggeredAnimationModel ClickableAreaAnimation { get; set; }

		[DataMember(Name = "clickableAreaScaler", Order = 14)]
		public Vector2 ClickableAreaScaler { get; set; }

		[DataMember(Name = "clickEventName", Order = 15)]
		public string ClickEventName { get; set; }
	}
}

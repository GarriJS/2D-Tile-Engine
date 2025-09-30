using Common.DiskModels.UI.Contracts;
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

		[DataMember(Name = "leftPadding", Order = 3)]
		public float LeftPadding { get; set; }

		[DataMember(Name = "rightPadding", Order = 4)]
		public float RightPadding { get; set; }

		[DataMember(Name = "sizeType", Order = 5)]
		public int? SizeType { get; set; }

		[DataMember(Name = "fixedSized", Order = 6)]
		public Vector2? FixedSized { get; set; }

		[DataMember(Name = "buttonHoverCursorName", Order = 7)]
		public string ElementHoverCursorName { get; set; }

		[DataMember(Name = "elementHoverEventName", Order = 8)]
		public string ElementHoverEventName { get; set; }

		[DataMember(Name = "buttonPressEventName", Order = 9)]
		public string ElementPressEventName { get; set; }

		[DataMember(Name = "graphicText", Order = 10)]
		public GraphicalTextModel GraphicText { get; set; }

		[DataMember(Name = "backgroundTexture", Order = 11)]
		public ImageModel BackgroundTexture { get; set; }

		[DataMember(Name = "clickableAreaAnimation", Order = 12)]
		public TriggeredAnimationModel ClickableAreaAnimation { get; set; }

		[DataMember(Name = "clickableAreaScaler", Order = 13)]
		public Vector2 ClickableAreaScaler { get; set; }

		[DataMember(Name = "buttonClickEventName", Order = 14)]
		public string ButtonClickEventName { get; set; }
	}
}

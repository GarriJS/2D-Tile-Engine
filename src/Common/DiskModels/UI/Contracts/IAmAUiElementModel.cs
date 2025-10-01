using Engine.DiskModels.Drawing;
using Microsoft.Xna.Framework;
using System.Runtime.Serialization;

namespace Common.DiskModels.UI.Contracts
{
	public interface IAmAUiElementModel
	{
		[DataMember(Name = "uiElementName", Order = 1)]
		public string UiElementName { get; set; }

		[DataMember(Name = "resizeTexture", Order = 2)]
		public bool ResizeTexture { get; set; }

		[DataMember(Name = "insidePadding", Order = 3)]
		public UiPaddingModel InsidePadding { get; set; }

		[DataMember(Name = "horizontalSizeType", Order = 4)]
		public int? HorizontalSizeType { get; set; }
		
		[DataMember(Name = "verticalSizeType", Order = 5)]
		public int? VerticalSizeType { get; set; }

		[DataMember(Name = "fixedSized", Order = 6)]
		public Vector2? FixedSized { get; set; }

		[DataMember(Name = "hoverEventName", Order = 7)]
		public string HoverCursorName { get; set; }

		[DataMember(Name = "hoverEventName", Order = 8)]
		public string HoverEventName { get; set; }

		[DataMember(Name = "pressEventName", Order = 9)]
		public string pressEventName { get; set; }

		[DataMember(Name = "texture", Order = 10)]
		public ImageModel Texture { get; set; }
	}
}

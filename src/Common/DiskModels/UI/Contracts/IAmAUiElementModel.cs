using Microsoft.Xna.Framework;
using System.Runtime.Serialization;

namespace Common.DiskModels.UI.Contracts
{
	public interface IAmAUiElementModel
	{
		[DataMember(Name = "uiElementName", Order = 1)]
		public string UiElementName { get; set; }

		[DataMember(Name = "leftPadding", Order = 2)]
		public float LeftPadding { get; set; }

		[DataMember(Name = "rightPadding", Order = 3)]
		public float RightPadding { get; set; }

		[DataMember(Name = "sizeType", Order = 4)]
		public int? SizeType { get; set; }

		[DataMember(Name = "fixedSized", Order = 5)]
		public Vector2? FixedSized { get; set; }

		[DataMember(Name = "backgroundTextureName", Order = 6)]
		public string BackgroundTextureName { get; set; }

		[DataMember(Name = "elementHoverEventName", Order = 7)]
		public string ElementHoverCursorName { get; set; }

		[DataMember(Name = "elementHoverEventName", Order = 8)]
		public string ElementHoverEventName { get; set; }

		[DataMember(Name = "elementPressEventName", Order = 9)]
		public string ElementPressEventName { get; set; }
	}
}

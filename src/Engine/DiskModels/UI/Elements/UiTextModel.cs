using Engine.DiskModels.UI.Contracts;
using Microsoft.Xna.Framework;
using System.Runtime.Serialization;

namespace Engine.DiskModels.UI.Elements
{
	[DataContract(Name = "uiText")]
	public class UiTextModel : IAmAUiElementModel
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

		[DataMember(Name = "text", Order = 7)]
		public string Text { get; set; }
	}
}

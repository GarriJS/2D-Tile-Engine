using Engine.DiscModels.Engine.Drawing;
using Engine.DiscModels.Engine.UI.Contracts;
using Microsoft.Xna.Framework;
using System.Runtime.Serialization;

namespace Engine.DiscModels.Engine.UI.Elements
{
	[DataContract(Name = "uiButton")]
	public class UiButtonModel : IAmAUiElementModel
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

		[DataMember(Name = "clickableAreaAnimation", Order = 7)]
		public TriggeredAnimationModel ClickableAreaAnimation { get; set; }

		[DataMember(Name = "buttonText", Order = 8)]
		public string ButtonText { get; set; }

		[DataMember(Name = "clickableAreaScaler", Order = 9)]
		public Vector2 ClickableAreaScaler { get; set; }
	}
}

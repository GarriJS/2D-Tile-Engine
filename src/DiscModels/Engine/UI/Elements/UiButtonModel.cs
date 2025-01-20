using DiscModels.Engine.Signals;
using DiscModels.Engine.UI.Contracts;
using System.Numerics;
using System.Runtime.Serialization;

namespace DiscModels.Engine.UI.Elements
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
		public int SizeType { get; set; }

		[DataMember(Name = "backgroundTextureName", Order = 5)]
		public string BackgroundTextureName { get; set; }

		[DataMember(Name = "buttonText", Order = 6)]
		public string ButtonText { get; set; }

		[DataMember(Name = "clickableAreaScaler", Order = 7)]
		public Vector2 ClickableAreaScaler { get; set; }

		[DataMember(Name = "signal", Order = 8)]
		public SignalModel Signal { get; set; }
	}
}

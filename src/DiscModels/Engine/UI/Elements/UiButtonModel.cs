using DiscModels.Engine.Signals;
using DiscModels.Engine.UI.Contracts;
using System.Runtime.Serialization;

namespace DiscModels.Engine.UI.Elements
{
	[DataContract(Name = "uiButtonModel")]
	public class UiButtonModel : IAmAUiElementModel
	{
		[DataMember(Name = "uiElementName", Order = 1)]
		public string UiElementName { get; set; }

		[DataMember(Name = "leftPadding", Order = 2)]
		public float LeftPadding { get; set; }

		[DataMember(Name = "rightPadding", Order = 3)]
		public float RightPadding { get; set; }

		[DataMember(Name = "backgroundTextureName", Order = 4)]
		public string BackgroundTextureName { get; set; }

		[DataMember(Name = "buttonText", Order = 5)]
		public string ButtonText { get; set; }

		[DataMember(Name = "signal", Order = 6)]
		public SignalModel Signal { get; set; }
	}
}

using System.Runtime.Serialization;

namespace DiscModels.Engine.UI.Contracts
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
		public int SizeType { get; set; }

		[DataMember(Name = "backgroundTextureName", Order = 5)]
		public string BackgroundTextureName { get; set; }
	}
}

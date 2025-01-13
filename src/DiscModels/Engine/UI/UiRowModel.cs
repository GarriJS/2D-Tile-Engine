using DiscModels.Engine.UI.Contracts;
using System.Runtime.Serialization;

namespace DiscModels.Engine.UI
{
	[DataContract(Name = "uiRowModel")]
	public class UiRowModel
	{
		[DataMember(Name = "uiRowName", Order = 1)]
		public string UiRowName { get; set; }

		[DataMember(Name = "topPadding", Order = 2)]
		public int TopPadding { get; set; }

		[DataMember(Name = "bottomPadding", Order = 3)]
		public int BottomPadding { get; set; }

		[DataMember(Name = "justificationType", Order = 4)]
		public int JustificationType { get; set; }

		[DataMember(Name = "backgroundTextureName", Order = 5)]
		public string BackgroundTextureName { get; set; }

		[DataMember(Name = "subElements", Order = 6)]
		public IAmAUiElementModel[] SubElements { get; set; }
	}
}

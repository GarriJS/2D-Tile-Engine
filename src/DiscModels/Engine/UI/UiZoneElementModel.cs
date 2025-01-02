using DiscModels.Engine.Drawing;
using System.Runtime.Serialization;

namespace DiscModels.Engine.UI
{
	[DataContract(Name = "uiZoneElementModel")]
	public class UiZoneElementModel
	{
		[DataMember(Name = "uiZoneElementName", Order = 1)]
		public string UiZoneElementName { get; set; }

		[DataMember(Name = "justificationType", Order = 2)]
		public int JustificationType { get; set; }

		[DataMember(Name = "background", Order = 3)]
		public SpriteModel Background { get; set; }

		[DataMember(Name = "uiZoneType", Order = 4)]
		public int UiZoneType { get; set; }

		[DataMember(Name = "elementRows", Order = 5)]
		public UiRowModel[] ElementRows { get; set; }
	}
}

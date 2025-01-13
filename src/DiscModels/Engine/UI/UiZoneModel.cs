using DiscModels.Engine.Drawing;
using System.Runtime.Serialization;

namespace DiscModels.Engine.UI
{
	[DataContract(Name = "uiZone")]
	public class UiZoneModel
	{
		[DataMember(Name = "uiZoneName", Order = 1)]
		public string UiZoneName { get; set; }

		[DataMember(Name = "justificationType", Order = 2)]
		public int JustificationType { get; set; }

		[DataMember(Name = "background", Order = 3)]
		public ImageModel Background { get; set; }

		[DataMember(Name = "uiZoneType", Order = 4)]
		public int UiZoneType { get; set; }

		[DataMember(Name = "elementRows", Order = 5)]
		public UiRowModel[] ElementRows { get; set; }
	}
}

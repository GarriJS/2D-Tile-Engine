using System.Runtime.Serialization;

namespace Common.DiskModels.UI
{
	[DataContract(Name = "uiZone")]
	public class UiZoneModel
	{
		[DataMember(Name = "uiZoneName", Order = 1)]
		public string UiZoneName { get; set; }

		[DataMember(Name = "justificationType", Order = 2)]
		public int JustificationType { get; set; }

		[DataMember(Name = "backgroundTextureName", Order = 3)]
		public string BackgroundTextureName { get; set; }

		[DataMember(Name = "uiZoneType", Order = 4)]
		public int UiZoneType { get; set; }

		[DataMember(Name = "zoneHoverEventName", Order = 5)]
		public string ZoneHoverEventName { get; set; }

		[DataMember(Name = "elementRows", Order = 6)]
		public UiRowModel[] ElementRows { get; set; }
	}
}

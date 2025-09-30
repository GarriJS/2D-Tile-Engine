using Engine.DiskModels.Drawing;
using System.Runtime.Serialization;

namespace Common.DiskModels.UI
{
	[DataContract(Name = "uiZone")]
	public class UiZoneModel
	{
		[DataMember(Name = "uiZoneName", Order = 1)]
		public string UiZoneName { get; set; }

		[DataMember(Name = "resizeTexture", Order = 2)]
		public bool ResizeTexture { get; set; }

		[DataMember(Name = "justificationType", Order = 3)]
		public int JustificationType { get; set; }

		[DataMember(Name = "uiZoneType", Order = 4)]
		public int UiZoneType { get; set; }

		[DataMember(Name = "zoneHoverCursorName", Order = 5)]
		public string ZoneHoverCursorName { get; set; }

		[DataMember(Name = "zoneHoverEventName", Order = 6)]
		public string ZoneHoverEventName { get; set; }

		[DataMember(Name = "backgroundTexture", Order = 7)]
		public ImageModel BackgroundTexture { get; set; }

		[DataMember(Name = "elementRows", Order = 8)]
		public UiRowModel[] ElementRows { get; set; }
	}
}

using Engine.DiskModels.Drawing;
using System.Text.Json.Serialization;

namespace Common.DiskModels.UserInterface
{
	public class UiZoneModel
	{
		[JsonPropertyName("uiZoneName")]
		public string UiZoneName { get; set; }

		[JsonPropertyName("resizeTexture")]
		public bool ResizeTexture { get; set; }

		[JsonPropertyName("justificationType")]
		public int JustificationType { get; set; }

		[JsonPropertyName("uiZoneType")]
		public int UiZoneType { get; set; }

		[JsonPropertyName("zoneHoverCursorName")]
		public string ZoneHoverCursorName { get; set; }

		[JsonPropertyName("zoneHoverEventName")]
		public string ZoneHoverEventName { get; set; }

		[JsonPropertyName("backgroundTexture")]
		public SimpleImageModel BackgroundTexture { get; set; }

		[JsonPropertyName("elementRows")]
		public UiRowModel[] ElementRows { get; set; }
	}
}

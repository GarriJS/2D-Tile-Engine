using Engine.DiskModels;
using System.Text.Json.Serialization;

namespace Common.DiskModels.UserInterface
{
	public class UiGroupModel : BaseDiskModel
	{
		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("visibilityGroupId")]
		public int VisibilityGroupId { get; set; }

		[JsonPropertyName("makeVisible")]
		public bool MakeVisible { get; set; }
		[JsonPropertyName("zones")]
		public UiZoneModel[] Zones { get; set; }
	}
}

using Engine.DiskModels;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Common.DiskModels.UserInterface
{
	public class UiGroupModel : BaseDiskModel
	{
		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("visibilityGroupId")]
		public int VisibilityGroupId { get; set; }

		[JsonPropertyName("isVisible")]
		public bool IsVisible { get; set; }

		[JsonPropertyName("zones")]
		public IList<UiZoneModel> Zones { get; set; }
	}
}

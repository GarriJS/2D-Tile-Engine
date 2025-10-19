using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Common.DiskModels.UI
{
	public class UiGroupModel
	{
		[JsonPropertyName("uiGroupName")]
		public string UiGroupName { get; set; }

		[JsonPropertyName("visibilityGroupId")]
		public int VisibilityGroupId { get; set; }

		[JsonPropertyName("isVisible")]
		public bool IsVisible { get; set; }

		[JsonPropertyName("uiZoneElements")]
		public IList<UiZoneModel> UiZoneElements { get; set; }
	}
}

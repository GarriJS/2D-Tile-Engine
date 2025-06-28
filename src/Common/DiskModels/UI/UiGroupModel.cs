using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Common.DiskModels.UI
{
	[DataContract(Name = "uiGroup")]
	public class UiGroupModel
	{
		[DataMember(Name = "uiGroupName", Order = 1)]
		public string UiGroupName { get; set; }

		[DataMember(Name = "visibilityGroupId", Order = 2)]
		public int VisibilityGroupId { get; set; }

		[DataMember(Name = "isVisible", Order = 3)]
		public bool IsVisible { get; set; }

		[DataMember(Name = "uiZoneElements", Order = 4)]
		public IList<UiZoneModel> UiZoneElements { get; set; }
	}
}

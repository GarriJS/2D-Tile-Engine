using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Engine.DiscModels.Engine.UI
{
	[DataContract(Name = "uiGroup")]
	public class UiGroupModel
	{
		[DataMember(Name = "uiGroupName", Order = 1)]
		public string UiGroupName { get; set; }

		[DataMember(Name = "visibilityGroupId", Order = 2)]
		public int? VisibilityGroupId { get; set; }

		[DataMember(Name = "uiZoneElements", Order = 3)]
		public IList<UiZoneModel> UiZoneElements { get; set; }
	}
}

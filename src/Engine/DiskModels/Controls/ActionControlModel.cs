using System.Runtime.Serialization;

namespace Engine.DiskModels.Controls
{
	[DataContract(Name = "actionControl")]
	public class ActionControlModel
	{
		[DataMember(Name = "actionControlDescription", Order = 1)]
		public string ActionControlDescription { get; set; }

		[DataMember(Name = "actionType", Order = 2)]
		public int ActionType { get; set; }

		[DataMember(Name = "controlKeys", Order = 3)]
		public int[] ControlKeys { get; set; }

		[DataMember(Name = "controlMouseButtons", Order = 4)]
		public int[] ControlMouseButtons { get; set; }
	}
}

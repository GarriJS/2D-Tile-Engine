using System.Runtime.Serialization;

namespace DiscModels.Engine.Controls
{
	[DataContract(Name = "actionControl")]
	public class ActionControlModel
	{
		[DataMember(Name = "actionControlDescription", Order = 1)]
		public required string ActionControlDescription { get; set; }

		[DataMember(Name = "actionType", Order = 2)]
		public required int ActionType { get; set; }

		[DataMember(Name = "controlKeys", Order = 3)]
		public required int[] ControlKeys { get; set; }

		[DataMember(Name = "controlMouseButtons", Order = 4)]
		public required int[] ControlMouseButtons { get; set; }
	}
}

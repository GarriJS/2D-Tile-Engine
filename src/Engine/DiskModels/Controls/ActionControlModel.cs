using System.Runtime.Serialization;

namespace Engine.DiskModels.Controls
{
	[DataContract(Name = "actionControl")]
	public class ActionControlModel
	{
		[DataMember(Name = "actionName", Order = 1)]
		public string ActionName { get; set; }

		[DataMember(Name = "controlKeys", Order = 2)]
		public int[] ControlKeys { get; set; }

		[DataMember(Name = "controlMouseButtons", Order = 3)]
		public int[] ControlMouseButtons { get; set; }
	}
}

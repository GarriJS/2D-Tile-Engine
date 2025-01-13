using System.Runtime.Serialization;

namespace DiscModels.Engine.Signals
{
	[DataContract(Name = "signal")]
	public class SignalModel
	{
		[DataMember(Name = "signalName", Order = 1)]
		public string SignalName { get; set; }
	}
}

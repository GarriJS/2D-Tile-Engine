using System.Runtime.Serialization;

namespace DiscModels.Engine.Physics
{
	[DataContract(Name = "position")]
	public class PositionModel
	{
		[DataMember(Name = "x", Order = 1)]
		public float X { get; set; }

		[DataMember(Name = "y", Order = 2)]
		public float Y { get; set; }
	}
}

using System.Runtime.Serialization;

namespace DiscModels.Engine.Physics.Contracts
{
	public interface IAmAAreaModel
	{
		[DataMember(Name = "width", Order = 1)]
		public float Width { get; set; }

		[DataMember(Name = "height", Order = 2)]
		public float Height { get; set; }

		[DataMember(Name = "position", Order = 3)]
		public PositionModel Position { get; set; }
	}
}

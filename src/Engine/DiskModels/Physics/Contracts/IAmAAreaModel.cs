using System.Runtime.Serialization;

namespace Engine.DiskModels.Physics.Contracts
{
	public interface IAmAAreaModel
	{
		[DataMember(Name = "position", Order = 1)]
		public PositionModel Position { get; set; }

		[DataMember(Name = "width", Order = 2)]
		public float Width { get; set; }

		[DataMember(Name = "height", Order = 3)]
		public float Height { get; set; }
	}
}

using System.Runtime.Serialization;

namespace DiscModels.Engine.Tiling.Contracts
{
	public interface IAmATileModel
	{
		[DataMember(Name = "row", Order = 1)]
		public int Row { get; set; }

		[DataMember(Name = "column", Order = 2)]
		public int Column { get; set; }
	}
}

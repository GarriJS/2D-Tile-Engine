using DiscModels.Engine.Physics;
using System.Runtime.Serialization;

namespace DiscModels.Engine.Drawing
{
	[DataContract(Name = "independentImage")]
	public class IndependentImageModel : ImageModel
	{
		[DataMember(Name = "position", Order = 2)]
		public PositionModel Position { get; set; }
	}
}

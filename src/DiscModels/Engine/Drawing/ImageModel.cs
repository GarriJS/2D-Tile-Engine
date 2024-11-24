using DiscModels.Engine.Physics;
using System.Runtime.Serialization;

namespace DiscModels.Engine.Drawing
{
	[DataContract(Name = "image")]
	public class ImageModel
	{
		[DataMember(Name = "position", Order = 1)]
		public PositionModel Position { get; set; }

		[DataMember(Name = "sprite", Order = 1)]
		public SpriteModel Sprite { get; set; }
	}
}

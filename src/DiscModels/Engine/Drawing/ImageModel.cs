using DiscModels.Engine.Physics;
using System.Runtime.Serialization;

namespace DiscModels.Engine.Drawing
{
	[DataContract(Name = "image")]
	public class ImageModel
	{
		[DataMember(Name = "position", Order = 1)]
		public required PositionModel Position { get; set; }

		[DataMember(Name = "sprite", Order = 1)]
		public required SpriteModel Sprite { get; set; }
	}
}

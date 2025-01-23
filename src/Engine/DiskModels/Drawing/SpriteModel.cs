using Microsoft.Xna.Framework;
using System.Runtime.Serialization;

namespace Engine.DiskModels.Engine.Drawing
{
	[DataContract(Name = "sprite")]
	public class SpriteModel : ImageModel
	{
		[DataMember(Name = "spritesheetBox", Order = 2)]
		public Rectangle SpritesheetBox { get; set; }
	}
}

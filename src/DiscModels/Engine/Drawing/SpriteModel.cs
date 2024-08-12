using Microsoft.Xna.Framework;
using System.Runtime.Serialization;

namespace DiskModels.Engine.Drawing
{
	[DataContract(Name = "sprite")]
	public class SpriteModel
	{
		[DataMember(Name = "spritesheetName", Order = 1)]
		public string SpritesheetName { get; set; }

		[DataMember(Name = "spritesheetBox", Order = 2)]
		public Rectangle SpritesheetBox { get; set; }
	}
}

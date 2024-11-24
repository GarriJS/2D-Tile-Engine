using Microsoft.Xna.Framework;
using System.Runtime.Serialization;

namespace DiscModels.Engine.Drawing
{
	[DataContract(Name = "sprite")]
	public class SpriteModel
	{
		[DataMember(Name = "spritesheetName", Order = 1)]
		public required string SpritesheetName { get; set; }

		[DataMember(Name = "spritesheetBox", Order = 2)]
		public required Rectangle SpritesheetBox { get; set; }
	}
}

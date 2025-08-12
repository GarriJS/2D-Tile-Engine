using Microsoft.Xna.Framework;
using System.Runtime.Serialization;

namespace Engine.DiskModels.Drawing
{
	[DataContract(Name = "image")]
	public class ImageModel
	{
		[DataMember(Name = "textureName", Order = 1)]
		public string TextureName { get; set; }

		[DataMember(Name = "textureBox", Order = 2)]
		public Rectangle TextureBox { get; set; }
	}
}

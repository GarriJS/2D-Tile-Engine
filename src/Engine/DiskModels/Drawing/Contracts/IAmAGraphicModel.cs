using Microsoft.Xna.Framework;
using System.Runtime.Serialization;

namespace Engine.DiskModels.Drawing.Contracts
{
	public interface IAmAGraphicModel
	{
		[DataMember(Name = "textureName", Order = 1)]
		public string TextureName { get; }

		[DataMember(Name = "textureBox", Order = 2)]
		public Rectangle TextureBox { get; }
	}
}

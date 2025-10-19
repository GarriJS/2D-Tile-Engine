using Microsoft.Xna.Framework;
using System.Text.Json.Serialization;

namespace Engine.DiskModels.Drawing.Contracts
{
	public interface IAmAGraphicModel
	{
		[JsonPropertyName("textureName")]
		public string TextureName { get; }

		[JsonPropertyName("textureBox")]
		public Rectangle TextureBox { get; }
	}
}

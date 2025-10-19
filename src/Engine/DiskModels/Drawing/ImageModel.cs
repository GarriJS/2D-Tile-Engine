using Engine.DiskModels.Drawing.Contracts;
using Microsoft.Xna.Framework;
using System.Text.Json.Serialization;

namespace Engine.DiskModels.Drawing
{
	public class ImageModel : IAmAGraphicModel
	{
		[JsonPropertyName("textureName")]
		public string TextureName { get; set; }

		[JsonPropertyName("textureBox")]
		public Rectangle TextureBox { get; set; }
	}
}

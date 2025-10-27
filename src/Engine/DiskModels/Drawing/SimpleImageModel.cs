using Engine.DiskModels.Drawing.Contracts;
using System.Text.Json.Serialization;

namespace Engine.DiskModels.Drawing
{
	public class SimpleImageModel : IAmAImageModel
	{
		[JsonPropertyName("textureName")]
		public string TextureName { get; set; }

		[JsonPropertyName("textureRegion")]
		public TextureRegionModel TextureRegion { get; set; }
	}
}

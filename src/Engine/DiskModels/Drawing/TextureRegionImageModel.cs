using System.Text.Json.Serialization;

namespace Engine.DiskModels.Drawing
{
	public class TextureRegionImageModel : ImageModel
	{
		[JsonPropertyName("textureRegion")]
		public TextureRegionModel TextureRegion { get; set; }
	}
}

using System.Text.Json.Serialization;

namespace Engine.DiskModels.Drawing
{
	public class ImageByPartsModel : ImageModel
	{
		[JsonPropertyName("textureRegions")]
		public TextureRegionModel[][] TextureRegions { get; set; }
	}
}

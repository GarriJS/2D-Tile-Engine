using System.Text.Json.Serialization;

namespace Engine.DiskModels.Drawing
{
	public class ImageByPartsModel
	{
		[JsonPropertyName("images")]
		public ImageModel[][] Images { get; set; }
	}
}

using System.Text.Json.Serialization;

namespace Engine.DiskModels.Drawing.Abstract
{
	abstract public class ImageBaseModel : GraphicBaseModel
	{
		[JsonPropertyName("textureName")]
		public string TextureName { get; set; }
	}
}

using Microsoft.Xna.Framework;
using System.Text.Json.Serialization;

namespace Engine.DiskModels.Drawing
{
	public class FillImageModel : ImageModel
	{
		[JsonPropertyName("fillBox")]
		public Vector2 FillBox { get; set; }
	}
}

using Microsoft.Xna.Framework;
using System.Text.Json.Serialization;

namespace Engine.DiskModels.Drawing
{
	public class TiledImageModel : ImageModel
	{
		[JsonPropertyName("fillBox")]
		public Vector2 FillBox { get; set; }
	}
}

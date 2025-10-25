using Engine.DiskModels.Physics;
using Engine.Graphics.Enum;
using Microsoft.Xna.Framework;
using System.Text.Json.Serialization;

namespace Engine.DiskModels.Drawing
{
	public class TextureRegionModel
	{
		[JsonPropertyName("textureRegionType")]
		public TextureRegionType TextureRegionType { get; set; }

		[JsonPropertyName("textureArea")]
		public Rectangle TextureArea {  get; set; }

		[JsonPropertyName("displayArea")]
		public SubAreaModel DisplayArea { get; set; }
	}
}

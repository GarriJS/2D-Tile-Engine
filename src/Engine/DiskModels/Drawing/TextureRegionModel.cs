using Engine.DiskModels.Physics;
using Engine.Graphics.Enum;
using Engine.Physics.Models.SubAreas;
using Microsoft.Xna.Framework;
using System.Text.Json.Serialization;

namespace Engine.DiskModels.Drawing
{
	public class TextureRegionModel
	{
		[JsonPropertyName("textureRegionType")]
		public TextureRegionType TextureRegionType { get; set; }

		[JsonPropertyName("textureBox")]
		public Rectangle TextureBox {  get; set; }

		[JsonPropertyName("displayArea")]
		public SubAreaModel DisplayArea { get; set; }

		public SubArea GetDimensions()
		{
			return new SubArea();
		}
	}
}

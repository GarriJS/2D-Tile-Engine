using Engine.DiskModels.Physics;
using Engine.Graphics.Enum;
using Engine.Physics.Models.SubAreas;
using Microsoft.Xna.Framework;
using System.Text.Json.Serialization;

namespace Engine.DiskModels.Drawing
{
	public class TextureRegionModel : BaseDiskModel
	{
		[JsonPropertyName("textureRegionType")]
		public TextureRegionType TextureRegionType { get; set; }

		[JsonPropertyName("textureBox")]
		public Rectangle TextureBox {  get; set; }

		[JsonPropertyName("displayArea")]
		public SubAreaModel DisplayArea { get; set; }

		public SubArea GetDimensions()
		{
			var result = new SubArea
			{ 
				Width = this.DisplayArea?.Width ?? this.TextureBox.Width,
				Height = this.DisplayArea?.Height ?? this.TextureBox.Height
			};

			return result;
		}
	}
}

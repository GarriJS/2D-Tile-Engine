using Engine.DiskModels.Drawing.Contracts;
using Engine.DiskModels.Physics;
using Engine.Physics.Models.SubAreas;
using System.Text.Json.Serialization;

namespace Engine.DiskModels.Drawing
{
	public class SimpleImageModel : IAmAImageModel
	{
		[JsonPropertyName("textureName")]
		public string TextureName { get; set; }

		[JsonPropertyName("textureRegion")]
		public TextureRegionModel TextureRegion { get; set; }

		public SubArea GetDimensions()
		{
			var result = this.TextureRegion.GetDimensions();

			return result;
		}

		public void SetDrawDimensions(SubAreaModel dimensions)
		{ 
			this.TextureRegion.DisplayArea = dimensions;
		}
	}
}

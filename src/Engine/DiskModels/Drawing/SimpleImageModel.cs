using Engine.DiskModels.Drawing.Abstract;
using Engine.DiskModels.Physics;
using Engine.Physics.Models.SubAreas;
using System.Text.Json.Serialization;

namespace Engine.DiskModels.Drawing
{
	public class SimpleImageModel : ImageBaseModel
	{
		[JsonPropertyName("textureRegion")]
		public TextureRegionModel TextureRegion { get; set; }

		override public SubArea GetDimensions()
		{
			var result = this.TextureRegion.GetDimensions();

			return result;
		}

		override public void SetDrawDimensions(SubAreaModel dimensions)
		{ 
			this.TextureRegion.DisplayArea = dimensions;
		}

		override public bool Equals(object obj)
		{
			if (obj is not SimpleImageModel simpleImageModel)
			{ 
				return false;
			}

			if (false == this.TextureName.Equals(simpleImageModel.TextureName, System.StringComparison.OrdinalIgnoreCase))
			{
				return false;
			}

			if (((this.TextureRegion is not null) &&
				 (simpleImageModel.TextureRegion is null)) ||
				((this.TextureRegion is null) &&
				 (simpleImageModel.TextureRegion is not null)))
			{
				return false;
			}

			if ((this.TextureRegion is not null) &&
				(simpleImageModel.TextureRegion is not null) &&
				(false == this.TextureRegion.Equals(simpleImageModel.TextureRegion)))
			{
				return false;
			}

			return true;
		}
	}
}

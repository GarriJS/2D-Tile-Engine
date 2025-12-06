using Engine.DiskModels.Drawing.Contracts;
using Engine.Physics.Models.SubAreas;
using System;
using System.Text.Json.Serialization;

namespace Engine.DiskModels.Drawing
{
	public class CompositeImageModel : IAmAImageModel
	{
		[JsonPropertyName("textureName")]
		public string TextureName { get; set; }

		[JsonPropertyName("textureRegions")]
		public TextureRegionModel[][] TextureRegions { get; set; }

		public SubArea GetDimensions()
		{
			if ((this.TextureRegions == null) || 
				(this.TextureRegions.Length == 0))
			{
				return new SubArea 
				{ 
					Width = 0,
					Height = 0 
				};
			}

			float largestWidth = 0;
			float largestHeight = 0;

			foreach (var row in this.TextureRegions)
			{
				if ((row == null) || 
					(row.Length == 0))
				{
					continue;
				}

				float rowWidth = 0;
				float rowHeight = 0;

				foreach (var col in row)
				{
					if (col == null)
					{
						continue;
					}

					var colDim = col.GetDimensions();
					rowWidth += colDim.Width;

					if (rowHeight < colDim.Height)
					{ 
						rowHeight = colDim.Height;
					}
				}

				if (largestWidth < rowWidth)
				{
					largestWidth = rowWidth;
				}

				if (largestHeight < rowHeight)
				{
					largestHeight = rowHeight;
				}
			}

			var result = new SubArea
			{
				Width = largestWidth,
				Height = largestHeight
			};

			return result;
		}
	}
}

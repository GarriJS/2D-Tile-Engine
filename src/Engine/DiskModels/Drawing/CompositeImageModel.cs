using Engine.DiskModels.Drawing.Contracts;
using Engine.DiskModels.Physics;
using Engine.Physics.Models.SubAreas;
using System.Text.Json.Serialization;

namespace Engine.DiskModels.Drawing
{
	public class CompositeImageModel : BaseDiskModel, IAmAImageModel
	{
		[JsonPropertyName("textureName")]
		public string TextureName { get; set; }

		[JsonPropertyName("textureRegions")]
		public TextureRegionModel[][] TextureRegions { get; set; }

		public SubArea GetDimensions()
		{
			if ((this.TextureRegions is null) || 
				(0 == this.TextureRegions.Length))
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

		public void SetDrawDimensions(SubAreaModel dimensions)
		{
			if ((this.TextureRegions is null) ||
				(0 == this.TextureRegions.Length) ||
				(this.TextureRegions[0] is null) ||
				(0 == this.TextureRegions[0].Length))
			{
				return;
			}

			var topCornersWidth = this.TextureRegions[0][0].GetDimensions().Width + this.TextureRegions[0][this.TextureRegions[0].Length - 1].DisplayArea.Width;
			var leftCornersHeight = this.TextureRegions[0][0].GetDimensions().Height + this.TextureRegions[this.TextureRegions.Length - 1][0].DisplayArea.Height;
			var middleSetWidth = dimensions.Width - topCornersWidth;
			var middleSetHeight = dimensions.Height - leftCornersHeight;

			if (0 >= middleSetWidth)
			{
				middleSetWidth = 0;
			}
			
			if (0 >= middleSetHeight)
			{
				middleSetHeight = 0;
			}

			for (int i = 0; i < this.TextureRegions.Length; i++)
			{
				if ((this.TextureRegions[i] is null) ||
					(3 > this.TextureRegions[i].Length))
				{
					continue;
				}

				for (int j = 1; j < this.TextureRegions[i].Length - 1; j++)
				{
					if (this.TextureRegions[i][j] is null)
					{ 
						continue;
					}
					else if (this.TextureRegions[i][j].DisplayArea is null)
					{
						this.TextureRegions[i][j].DisplayArea = new SubAreaModel
						{ 
							Width = middleSetWidth,
							Height = leftCornersHeight
						};
					}
					else
					{
						this.TextureRegions[i][j].DisplayArea.Width = middleSetWidth;
					}
				}
			}

			for (int i = 1; i < this.TextureRegions[0].Length - 1; i++)
			{
				if (this.TextureRegions[i] is null)
				{
					continue;
				}

				for (int j = 0; j < this.TextureRegions.Length; j++)
				{
					if (this.TextureRegions[i][j] is null)
					{
						continue;
					}
					else if (this.TextureRegions[i][j].DisplayArea is null)
					{
						this.TextureRegions[i][j].DisplayArea = new SubAreaModel
						{
							Width = topCornersWidth,
							Height = middleSetHeight
						};
					}
					else
					{
						this.TextureRegions[i][j].DisplayArea.Height = middleSetHeight;
					}
				}
			}
		}
	}
}

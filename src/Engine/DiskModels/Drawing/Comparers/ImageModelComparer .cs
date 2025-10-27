using System;
using System.Collections.Generic;

namespace Engine.DiskModels.Drawing.Comparers
{
	public class ImageModelComparer : IEqualityComparer<SimpleImageModel>
	{
		public bool Equals(SimpleImageModel a, SimpleImageModel b)
		{
			if (true == ReferenceEquals(a, b))
			{
				return true;
			}

			if ((a is null) ||
				(b is null))
			{
				return false;
			}

			var result = ((true == string.Equals(a.TextureName, b.TextureName, StringComparison.Ordinal)) &&
						  (a.TextureRegion.TextureRegionType == b.TextureRegion.TextureRegionType) &&
						  (a.TextureRegion.TextureBox == b.TextureRegion.TextureBox) &&
						  (a.TextureRegion.DisplayArea == b.TextureRegion.DisplayArea));

			return result;
		}

		public int GetHashCode(SimpleImageModel model)
		{
			if (model is null)
			{
				return 0;
			}

			var result = HashCode.Combine(
				model.TextureName,
				model.TextureRegion.TextureBox,
				model.TextureRegion.DisplayArea.Width,
				model.TextureRegion.DisplayArea.Height
			);

			return result;
		}
	}
}

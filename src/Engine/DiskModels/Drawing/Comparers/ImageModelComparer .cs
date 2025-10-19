using System;
using System.Collections.Generic;

namespace Engine.DiskModels.Drawing.Comparers
{
	public class ImageModelComparer : IEqualityComparer<ImageModel>
	{
		public bool Equals(ImageModel a, ImageModel b)
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
						  (a.TextureBox == b.TextureBox));

			return result;
		}

		public int GetHashCode(ImageModel model)
		{
			if (model is null)
			{
				return 0;
			}

			var rect = model.TextureBox;
			var result = HashCode.Combine(
				model.TextureName,
				rect.X,
				rect.Y,
				rect.Width,
				rect.Height
			);

			return result;
		}
	}
}

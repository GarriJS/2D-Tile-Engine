using Engine.DiskModels.Drawing.Abstract;
using System;
using System.Collections.Generic;

namespace Engine.DiskModels.Drawing.Comparers
{
	public class ImageModelComparer : IEqualityComparer<ImageBaseModel>
	{
		public bool Equals(ImageBaseModel a, ImageBaseModel b)
		{
			if (true == ReferenceEquals(a, b))
				return true;

			if ((a is null) ||
				(b is null))
				return false;

			var result = a.Equals(b);

			return result;
		}

		public int GetHashCode(ImageBaseModel model)
		{
			if (model is null)
				return 0;

			var result = HashCode.Combine(
				model.TextureName
			);

			return result;
		}
	}
}

using Engine.DiskModels.Drawing.Contracts;
using System;
using System.Collections.Generic;

namespace Engine.DiskModels.Drawing.Comparers
{
	public class ImageModelComparer : IEqualityComparer<IAmAImageModel>
	{
		public bool Equals(IAmAImageModel a, IAmAImageModel b)
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

			var result = (true == string.Equals(a.TextureName, b.TextureName, StringComparison.Ordinal));

			return result;
		}

		public int GetHashCode(IAmAImageModel model)
		{
			if (model is null)
			{
				return 0;
			}

			var result = HashCode.Combine(
				model.TextureName
			);

			return result;
		}
	}
}

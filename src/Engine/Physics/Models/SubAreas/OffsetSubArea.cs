using Engine.Core.Files.Models.Contract;
using Engine.DiskModels.Physics;

namespace Engine.Physics.Models.SubAreas
{
	/// <summary>
	/// Represents a offset sub area.
	/// </summary>
	public class OffsetSubArea : SubArea, ICanBeSerialized<OffsetSubAreaModel>
	{
		/// <summary>
		/// Gets or sets the horizontal offset.
		/// </summary>
		public float HorizontalOffset { get; set; }

		/// <summary>
		/// Gets or sets the vertical offset.
		/// </summary>
		public float VerticalOffset { get; set; }

		/// <summary>
		/// Converts the object to a serialization model.
		/// </summary>
		/// <returns>The serialization model.</returns>
		override public OffsetSubAreaModel ToModel()
		{
			var result = new OffsetSubAreaModel
			{
				Width = Width,
				Height = Height,
				HorizontalOffset = HorizontalOffset,
				VerticalOffset = VerticalOffset
			};

			return result;
		}
	}
}

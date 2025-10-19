using Engine.Core.Files.Models.Contract;
using Engine.DiskModels.Physics;

namespace Engine.Physics.Models.SubAreas
{
	/// <summary>
	/// Represents a sub area.
	/// </summary>
	public class SubArea : ICanBeSerialized<SubAreaModel>
	{
		/// <summary>
		/// Gets or sets the width.
		/// </summary>
		public float Width;

		/// <summary>
		/// Gets or set the height.
		/// </summary>
		public float Height;

		/// <summary>
		/// Converts the object to a serialization model.
		/// </summary>
		/// <returns>The serialization model.</returns>
		virtual public SubAreaModel ToModel()
		{
			return new SubAreaModel
			{
				Width = Width,
				Height = Height
			};
		}
	}
}

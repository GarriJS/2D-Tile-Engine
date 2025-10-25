using Engine.Core.Files.Models.Contract;
using Engine.DiskModels.Physics;
using Microsoft.Xna.Framework;

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
		/// Gets the sub area as a vector.
		/// </summary>
		public Vector2 ToVector => new() { X = Width, Y = Height };

		/// <summary>
		/// Converts the object to a serialization model.
		/// </summary>
		/// <returns>The serialization model.</returns>
		virtual public SubAreaModel ToModel()
		{
			var result =  new SubAreaModel
			{
				Width = Width,
				Height = Height
			};

			return result;
		}
	}
}

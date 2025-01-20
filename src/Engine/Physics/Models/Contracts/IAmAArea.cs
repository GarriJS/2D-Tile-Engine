using Microsoft.Xna.Framework;

namespace Engine.Physics.Models.Contracts
{
	/// <summary>
	/// Represents a area.
	/// </summary>
	public interface IAmAArea : IHavePosition
	{
		/// <summary>
		/// Gets the width.
		/// </summary>
		public float Width { get; }

		/// <summary>
		/// Gets the height.
		/// </summary>
		public float Height { get; }

		/// <summary>
		/// Determines if a the area contains the coordinate.
		/// </summary>
		/// <param name="coordinate">The coordinate.</param>
		/// <returns>A value indicating whether the area contains the coordinate.</returns>
		public bool Contains(Vector2 coordinate);
	}
}

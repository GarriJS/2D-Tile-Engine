using Game.Physics.Models.Contracts;

namespace Engine.Physics.Models.Contracts
{
	/// <summary>
	/// Represents a area.
	/// </summary>
	public interface IAmAArea : IHavePosition
	{
		/// <summary>
		/// Gets a value that indicates whether the area has a value.
		/// </summary>
		public bool HasCollision { get; }

		/// <summary>
		/// Gets the width.
		/// </summary>
		public float Width { get; }

		/// <summary>
		/// Gets the height.
		/// </summary>
		public float Height { get; }
	}
}

using Engine.Physics.Models.Contracts;
using Game.Physics.Models;

namespace Engine.Physics.Models
{
	/// <summary>
	/// Represents a simple area.
	/// </summary>
	public class SimpleArea : IAmAArea, IHaveCollisionTypes
	{
		/// <summary>
		/// Gets a value that indicates whether the area has a value.
		/// </summary>
		public bool HasCollision { get; set; }

		/// <summary>
		/// Get or sets the width.
		/// </summary>
		public float Width { get; set; }

		/// <summary>
		/// Gets or sets the height.
		/// </summary>
		public float Height { get; set; }

		/// <summary>
		/// Gets or sets the position.
		/// </summary>
		public Position Position { get; set; }

		/// <summary>
		/// Gets or sets the collision type ids.
		/// </summary>
		public int[] CollisionTypeIds { get; set; }
	}
}

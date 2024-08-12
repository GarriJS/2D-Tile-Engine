using Engine.Physics.Models.Contracts;
using Game.Physics.Models;

namespace Engine.Physics.Models
{
	/// <summary>
	/// Represents a area collection
	/// </summary>
	public class AreaCollection : IAmAArea
	{
		/// <summary>
		/// Gets a value that indicates whether the area has a value.
		/// </summary>
		public bool HasCollision { get; set; }
		
		/// <summary>
		/// Gets the width.
		/// </summary>
		public float Width { get; }

		/// <summary>
		/// Gets the height.
		/// </summary>
		public float Height { get; }

		/// <summary>
		/// Gets or sets the position.
		/// </summary>
		public Position Position { get; set; }
	
		/// <summary>
		/// Gets or sets the areas.
		/// </summary>
		public SimpleArea[] Areas { get; set; }
	}
}

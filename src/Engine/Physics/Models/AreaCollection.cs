using Engine.Physics.Models.Contracts;

namespace Engine.Physics.Models
{
	/// <summary>
	/// Represents a area collection
	/// </summary>
	public class AreaCollection(float width, float height) : IAmAArea
	{	
		/// <summary>
		/// Gets the width.
		/// </summary>
		public float Width { get; } = width;

		/// <summary>
		/// Gets the height.
		/// </summary>
		public float Height { get; } = height;

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

using Engine.Physics.Models.Contracts;

namespace Engine.Physics.Models
{
	/// <summary>
	/// Represents a simple area.
	/// </summary>
	public class SimpleArea : IAmAArea
	{
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
	}
}

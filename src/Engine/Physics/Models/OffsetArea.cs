namespace Engine.Physics.Models
{
	/// <summary>
	/// Represents a offset area.
	/// </summary>
	public class OffsetArea : SimpleArea
	{
		/// <summary>
		/// Gets or sets the vertical offset.
		/// </summary>
		public float VerticalOffset { get; set; }

		/// <summary>
		/// Gets or sets the horizontal offset.
		/// </summary>
		public float HorizontalOffset { get; set; }
	}
}

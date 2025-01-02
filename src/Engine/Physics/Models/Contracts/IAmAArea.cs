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
	}
}

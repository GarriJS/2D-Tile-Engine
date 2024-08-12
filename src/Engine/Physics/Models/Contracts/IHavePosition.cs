namespace Engine.Physics.Models.Contracts
{
	/// <summary>
	/// Represents something that has a position.
	/// </summary>
	public interface IHavePosition
	{
		/// <summary>
		/// Gets the position.
		/// </summary>
		public Position Position { get; }
	}
}

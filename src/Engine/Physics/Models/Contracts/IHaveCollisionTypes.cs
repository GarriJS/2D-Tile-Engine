namespace Engine.Physics.Models.Contracts
{
	/// <summary>
	/// Represents something with collision types.
	/// </summary>
	public interface IHaveCollisionTypes
	{
		/// <summary>
		/// Gets the collision type ids.
		/// </summary>
		public int[] CollisionTypeIds { get; }
	}
}

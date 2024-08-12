using Engine.Physics.Models.Contracts;

namespace Engine.Physics.Models.Contracts
{
	/// <summary>
	/// Represents something that has a area.
	/// </summary>
	public interface IHaveArea : IHavePosition
	{
		/// <summary>
		/// Gets the area.
		/// </summary>
		public IAmAArea Area { get; }
	}
}

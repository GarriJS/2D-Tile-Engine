using Engine.Physics.Models.SubAreas;

namespace Engine.Physics.Models.Contracts
{
	/// <summary>
	/// Represents a sub area. 
	/// </summary>
	public interface IHaveASubArea
	{
		/// <summary>
		/// Gets the sub area.
		/// </summary>
		public SubArea SubArea { get; }
	}
}

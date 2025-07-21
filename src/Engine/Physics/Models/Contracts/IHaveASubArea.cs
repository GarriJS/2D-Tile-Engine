using Microsoft.Xna.Framework;

namespace Engine.Physics.Models.Contracts
{
	/// <summary>
	/// Represents a sub area. 
	/// </summary>
	public interface IHaveASubArea
	{
		/// <summary>
		/// Gets the area.
		/// </summary>
		public Vector2 Area { get; }
	}
}

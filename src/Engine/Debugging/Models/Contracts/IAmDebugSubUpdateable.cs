using Microsoft.Xna.Framework;

namespace Engine.Debugging.Models.Contracts
{
	/// <summary>
	/// Represents something that is updated by another updateable.
	/// </summary>
	public interface IAmDebugSubUpdateable
	{
		/// <summary>
		/// Gets or sets the update order.
		/// </summary>
		public int UpdateOrder { get; set; }

		/// <summary>
		/// Updates the debug updateable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		public void DebugUpdate(GameTime gameTime, GameServiceContainer gameServices);
	}
}

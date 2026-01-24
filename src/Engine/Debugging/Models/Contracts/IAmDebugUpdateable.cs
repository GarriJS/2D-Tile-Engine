using Microsoft.Xna.Framework;

namespace Engine.Debugging.Models.Contracts
{
	/// <summary>
	/// Represents something that is debug updateable.
	/// </summary>
	public interface IAmDebugUpdateable
	{
		/// <summary>
		/// Updates the debug updateable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		public void DebugUpdate(GameTime gameTime, GameServiceContainer gameServices);
	}
}

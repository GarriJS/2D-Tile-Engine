using Microsoft.Xna.Framework;

namespace Engine.Debugging.Models.Contracts
{
	/// <summary>
	/// Represents something that is drawn by another debug drawable.
	/// </summary>
	public interface IAmDebugSubDrawable
	{
		/// <summary>
		/// Draws the debug drawable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		/// <param name="coordinates">The coordinates.</param>
		/// <param name="color">The color.</param>
		/// <param name="offset">The offset.</param>
		public void DrawDebug(GameTime gameTime, GameServiceContainer gameServices, Vector2 coordinates, Color color, Vector2 offset = default);
	}
}

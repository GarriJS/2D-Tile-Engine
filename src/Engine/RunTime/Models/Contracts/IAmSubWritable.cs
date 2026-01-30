using Microsoft.Xna.Framework;

namespace Engine.RunTime.Models.Contracts
{
	/// <summary>
	/// Represents something that is written by another writable.
	/// </summary>
	public interface IAmSubWritable
	{
		/// <summary>
		/// Writes the sub writable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		/// <param name="coordinates">The coordinates.</param>
		/// <param name="offset">The offset.</param>
		public void Write(GameTime gameTime, GameServiceContainer gameServices, Vector2 coordinates, Vector2 offset = default);
	}
}

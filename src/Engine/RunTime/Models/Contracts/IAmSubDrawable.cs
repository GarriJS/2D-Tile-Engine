using Engine.Physics.Models;
using Microsoft.Xna.Framework;

namespace Engine.RunTime.Models.Contracts
{
	/// <summary>
	/// Represents something that is drawn by another drawable.
	/// </summary>
	public interface IAmSubDrawable
    {
        /// <summary>
        /// Draws the sub drawable.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        /// <param name="gameServices">The game services.</param>
        /// <param name="position">The position.</param>
        /// <param name="offset">The offset.</param>
        public void Draw(GameTime gameTime, GameServiceContainer gameServices, Position position, Vector2 offset = default);
    }
}

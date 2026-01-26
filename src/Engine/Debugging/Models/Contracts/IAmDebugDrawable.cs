using Microsoft.Xna.Framework;

namespace Engine.Debugging.Models.Contracts
{
    /// <summary>
    /// Represents something debug drawable.
    /// </summary>
    public interface IAmDebugDrawable
    {
        /// <summary>
        /// Gets or sets the draw layer.
        /// </summary>
        public int DrawLayer { get; set; }

		/// <summary>
		/// Draws the debug drawable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		public void DrawDebug(GameTime gameTime, GameServiceContainer gameServices);
	}
}

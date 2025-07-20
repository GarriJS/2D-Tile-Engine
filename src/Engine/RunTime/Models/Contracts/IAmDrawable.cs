using Microsoft.Xna.Framework;

namespace Engine.RunTime.Models.Contracts
{
    /// <summary>
    /// Represents something drawable.
    /// </summary>
    public interface IAmDrawable
    {
        /// <summary>
        /// Gets or sets the draw layer.
        /// </summary>
        public int DrawLayer { get; set; }

		/// <summary>
		/// Draws the drawable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		public void Draw(GameTime gameTime, GameServiceContainer gameServices);
	}
}

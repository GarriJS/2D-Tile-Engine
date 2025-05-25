using Microsoft.Xna.Framework;

namespace Engine.Drawables.Models.Contracts
{
	/// <summary>
	/// Represents something that can be drawn.
	/// </summary>
	public interface ICanBeDrawn
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

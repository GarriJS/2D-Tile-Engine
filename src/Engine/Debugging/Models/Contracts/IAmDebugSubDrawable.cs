using Engine.Physics.Models;
using Microsoft.Xna.Framework;

namespace Engine.Debugging.Models.Contracts
{
	/// <summary>
	/// Represents something that is drawn by another debug drawable.
	/// </summary>
	public interface IAmDebugSubDrawable
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
		/// <param name="position">The position.</param>
		/// <param name="color">The color.</param>
		/// <param name="offset">The offset.</param>
		public void DrawDebug(GameTime gameTime, GameServiceContainer gameServices, Position position, Color color, Vector2 offset = default);
	}
}

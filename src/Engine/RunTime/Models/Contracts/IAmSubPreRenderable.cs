using Microsoft.Xna.Framework;

namespace Engine.RunTime.Models.Contracts
{
	/// <summary>
	/// Represents that can be prerendered by another prerenderable.
	/// </summary>
	public interface IAmSubPreRenderable
	{
		/// <summary>
		/// Assess if prerendering is needed.
		/// </summary>
		/// <returns>A value indicating whether prerendering is needed.</returns>
		public bool ShouldPreRender();

		/// <summary>
		/// Draws the sub drawable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		/// <param name="coordinates">The coordinates.</param>
		/// <param name="color">The color.</param>
		/// <param name="offset">The offset.</param>
		public void PreRender(GameTime gameTime, GameServiceContainer gameServices, Vector2 coordinates, Color color, Vector2 offset = default);
	}
}

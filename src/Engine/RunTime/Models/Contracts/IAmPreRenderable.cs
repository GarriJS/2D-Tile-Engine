using Microsoft.Xna.Framework;

namespace Engine.RunTime.Models.Contracts
{
	/// <summary>
	/// Represents that can be prerendered.
	/// </summary>
	public interface IAmPreRenderable
	{
		/// <summary>
		/// Assess if prerendering is needed.
		/// </summary>
		/// <returns>A value indicating whether prerendering is needed.</returns>
		public bool ShouldPreRender();

		/// <summary>
		/// Does the prerender.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game service.</param>
		public void PreRender(GameTime gameTime, GameServiceContainer gameServices);
	}
}

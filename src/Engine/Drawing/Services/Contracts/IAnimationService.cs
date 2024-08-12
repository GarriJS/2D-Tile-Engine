using DiskModels.Engine.Drawing;
using Engine.Drawing.Models;
using Microsoft.Xna.Framework;

namespace Engine.Drawing.Services.Contracts
{
	/// <summary>
	/// Represents a animation service.
	/// </summary>
	public interface IAnimationService
	{
		/// <summary>
		/// Gets the animation.
		/// </summary>
		/// <param name="animationModel">The animation.</param>
		/// <returns>The animation.</returns>
		public Animation GetAnimation(AnimationModel animationModel);

		/// <summary>
		/// Updates the animation frame.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="animation">The animation.</param>
		public void UpdateAnimationFrame(GameTime gameTime, Animation animation);
	}
}

using DiscModels.Engine.Drawing;
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
		/// <param name="frameWidth">The frame width.</param>
		/// <param name="frameHeight">The frame height.</param>
		/// <returns>The animation.</returns>
		public Animation GetAnimation(AnimationModel animationModel, int frameWidth, int frameHeight);

		/// <summary>
		/// Updates the animation frame.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="animation">The animation.</param>
		public void UpdateAnimationFrame(GameTime gameTime, Animation animation);

		/// <summary>
		/// Triggers the animation.
		/// </summary>
		/// <param name="triggeredAnimation">The triggered animation.</param>
		/// <param name="allowReset">A value indicating whether to allow the animation trigger to reset.</param>
		public void TriggerAnimation(TriggeredAnimation triggeredAnimation, bool allowReset = false);
	}
}

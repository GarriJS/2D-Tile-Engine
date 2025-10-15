using Engine.DiskModels.Drawing;
using Engine.Graphics.Models;

namespace Engine.Graphics.Services.Contracts
{
	/// <summary>
	/// Represents a animation service.
	/// </summary>
	public interface IAnimationService
	{
		/// <summary>
		/// Gets the animation from the model.
		/// </summary>
		/// <param name="animationModel">The animation.</param>
		/// <returns>The animation.</returns>
		public Animation GetAnimationFromModel(AnimationModel animationModel);

		/// <summary>
		/// Gets the fixed animation.
		/// </summary>
		/// <param name="animationModel">The animation model.</param>
		/// <param name="frameWidth">The frame width.</param>
		/// <param name="frameHeight">The frame height.</param>
		/// <returns>The fixed animation.</returns>
		public Animation GetFixedAnimationFromModel(AnimationModel animationModel, int frameWidth, int frameHeight);
	}
}

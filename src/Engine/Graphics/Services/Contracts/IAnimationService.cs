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
		/// <param name="frameSetWidth">The frame set width.</param>
		/// <param name="frameSetHeight">The frame set height.</param>
		/// <returns>The fixed animation.</returns>
		public Animation GetFixedAnimationFromModel(AnimationModel animationModel, int frameSetWidth, int frameSetHeight);
	}
}

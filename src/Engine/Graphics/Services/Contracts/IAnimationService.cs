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
	}
}

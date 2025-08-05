using Engine.DiskModels.Drawing;
using Engine.Graphics.Models;
using Engine.Graphics.Services.Contracts;
using Engine.RunTime.Services;
using Microsoft.Xna.Framework;

namespace Engine.Graphics.Services
{
	/// <summary>
	/// Represents a animation service.
	/// </summary>
	/// <remarks>
	/// Initializes a new instance of the animation service.
	/// </remarks>
	/// <param name="gameServices">The game services.</param>
	public class AnimationService(GameServiceContainer gameServices) : IAnimationService
	{
		private readonly GameServiceContainer _gameServices = gameServices;

		/// <summary>
		/// Gets the animation.
		/// </summary>
		/// <param name="animationModel">The animation.</param>
		/// <param name="frameWidth">The frame width.</param>
		/// <param name="frameHeight">The frame height.</param>
		/// <returns>The animation.</returns>
		public Animation GetAnimation(AnimationModel animationModel, int frameWidth, int frameHeight)
		{
			var imageService = this._gameServices.GetService<IImageService>();

			var frames = new Image[animationModel.Frames.Length];

			for (int i = 0; i < frames.Length; i++)
			{
				frames[i] = imageService.GetImage(animationModel.Frames[i], frameWidth, frameHeight);
			}

			var animation = animationModel switch
			{
				TriggeredAnimationModel triggeredAnimationModel => 
				new TriggeredAnimation
				{
					CurrentFrameIndex = triggeredAnimationModel.CurrentFrameIndex,
					FrameMinDuration = triggeredAnimationModel.FrameMinDuration,
					FrameMaxDuration = triggeredAnimationModel.FrameMaxDuration,
					Frames = frames,
					RestingFrameIndex = triggeredAnimationModel.RestingFrameIndex,
				},
				_ => 
				new Animation
				{
					CurrentFrameIndex = animationModel.CurrentFrameIndex,
					FrameMinDuration = animationModel.FrameMinDuration,
					FrameMaxDuration = animationModel.FrameMaxDuration,
					Frames = frames
				}
			};

			if (true == animationModel.FrameDuration.HasValue)
			{
				animation.FrameDuration = animationModel.FrameDuration.Value;
			}
			else
			{
				var randomService = this._gameServices.GetService<RandomService>();

				animation.FrameDuration = randomService.GetRandomInt(animationModel.FrameMinDuration.Value, animationModel.FrameMaxDuration.Value);
			}

			return animation;
		}
	}
}

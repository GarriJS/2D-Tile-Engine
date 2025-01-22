using Engine.DiscModels.Engine.Drawing;
using Engine.Drawing.Models;
using Engine.Drawing.Services.Contracts;
using Engine.RunTime.Services;
using Engine.RunTime.Services.Contracts;
using Microsoft.Xna.Framework;

namespace Engine.Drawing.Services
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

		/// <summary>
		/// Updates the animation frame.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="animation">The animation.</param>
		public void UpdateAnimationFrame(GameTime gameTime, Animation animation)
		{
			if ((null == animation.FrameStartTime) ||
				((animation is TriggeredAnimation triggeredAnimation) &&
				 (triggeredAnimation.CurrentFrameIndex == triggeredAnimation.RestingFrameIndex)))
			{
				animation.FrameStartTime = gameTime.TotalGameTime.TotalMilliseconds;

				return;
			}

			if (animation.FrameStartTime + animation.FrameDuration <= gameTime.TotalGameTime.TotalMilliseconds)
			{
				if ((true == animation.FrameMinDuration.HasValue) &&
					(true == animation.FrameMaxDuration.HasValue))
				{
					var randomService = this._gameServices.GetService<IRandomService>();
					animation.FrameDuration = randomService.GetRandomInt(animation.FrameMinDuration.Value, animation.FrameMaxDuration.Value);
				}
				else
				{
					animation.FrameDuration = animation.FrameDuration;
				}

				if (animation.CurrentFrameIndex < animation.Frames.Length - 1)
				{
					animation.CurrentFrameIndex++;
				}
				else
				{
					animation.CurrentFrameIndex = 0;
				}

				animation.FrameStartTime = gameTime.TotalGameTime.TotalMilliseconds;
			}
		}

		/// <summary>
		/// Triggers the animation.
		/// </summary>
		/// <param name="triggeredAnimation">The triggered animation.</param>
		/// <param name="allowReset">A value indicating whether to allow the animation trigger to reset.</param>
		public void TriggerAnimation(TriggeredAnimation triggeredAnimation, bool allowReset = false)
		{
			if ((false == allowReset) &&
				(triggeredAnimation.RestingFrameIndex != triggeredAnimation.CurrentFrameIndex))
			{ 
				return;
			}

			triggeredAnimation.FrameStartTime = null;
			triggeredAnimation.CurrentFrameIndex = triggeredAnimation.RestingFrameIndex;

			if (triggeredAnimation.CurrentFrameIndex < triggeredAnimation.Frames.Length - 1)
			{
				triggeredAnimation.CurrentFrameIndex++;
			}
			else
			{
				triggeredAnimation.CurrentFrameIndex = 0;
			}
		}

		/// <summary>
		/// Resets the triggered animation.
		/// </summary>
		/// <param name="triggeredAnimation">The triggered animation.</param>
		public void ResetTriggeredAnimation(TriggeredAnimation triggeredAnimation)
		{
			triggeredAnimation.FrameStartTime = null;
			triggeredAnimation.CurrentFrameIndex = triggeredAnimation.RestingFrameIndex;
		}
	}
}

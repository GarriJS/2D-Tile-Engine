using DiscModels.Engine.Drawing;
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
		/// <returns>The animation.</returns>
		public Animation GetAnimation(AnimationModel animationModel)
		{
			var spriteService = this._gameServices.GetService<ISpriteService>();
			var frames = new Sprite[animationModel.Frames.Length];

			for (int i = 0; i < frames.Length; i++)
			{ 
				frames[i] = spriteService.GetSprite(animationModel.Frames[i]);
			}

			var animation = new Animation
			{
				CurrentFrameIndex = animationModel.CurrentFrameIndex,
				FrameMinDuration = animationModel.FrameMinDuration,
				FrameMaxDuration = animationModel.FrameMaxDuration,
				Frames = frames,
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
			if (null == animation.FrameStartTime)
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
	}
}

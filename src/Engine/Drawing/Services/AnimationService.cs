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
	public class AnimationService : IAnimationService
	{
		private readonly GameServiceContainer _gameServiceContainer;

		/// <summary>
		/// Initializes a new instance of the animation service.
		/// </summary>
		/// <param name="gameServices">The game services.</param>
		public AnimationService(GameServiceContainer gameServices)
		{
			this._gameServiceContainer = gameServices;
		}

		/// <summary>
		/// Gets the animation.
		/// </summary>
		/// <param name="animationModel">The animation.</param>
		/// <returns>The animation.</returns>
		public Animation GetAnimation(AnimationModel animationModel)
		{
			var spriteService = this._gameServiceContainer.GetService<ISpriteService>();
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
				var randomService = this._gameServiceContainer.GetService<RandomService>();
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
					var randomService = this._gameServiceContainer.GetService<IRandomService>();
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

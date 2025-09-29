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
		/// Gets the animation from the model.
		/// </summary>
		/// <param name="animationModel">The animation.</param>
		/// <returns>The animation.</returns>
		public Animation GetAnimationFromModel(AnimationModel animationModel)
		{
			var imageService = this._gameServices.GetService<IImageService>();

			var frames = new Image[animationModel.Frames.Length];

			for (int i = 0; i < frames.Length; i++)
			{
				frames[i] = imageService.GetImageFromModel(animationModel.Frames[i]);
			}

			var animation = animationModel switch
			{
				TriggeredAnimationModel triggeredAnimationModel => new TriggeredAnimation
				{
					CurrentFrameIndex = triggeredAnimationModel.CurrentFrameIndex,
					FrameMinDuration = triggeredAnimationModel.FrameMinDuration,
					FrameMaxDuration = triggeredAnimationModel.FrameMaxDuration,
					Frames = frames,
					RestingFrameIndex = triggeredAnimationModel.RestingFrameIndex,
				},
				_ => new Animation
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
		/// Gets the fixed animation.
		/// </summary>
		/// <param name="animationModel">The animation model.</param>
		/// <param name="frameSetWidth">The frame set width.</param>
		/// <param name="frameSetHeight">The frame set height.</param>
		/// <returns>The fixed animation.</returns>
		public Animation GetFixedAnimationFromModel(AnimationModel animationModel, int frameSetWidth, int frameSetHeight)
		{
			foreach (var frameModel in animationModel.Frames)
			{
				frameModel.TextureBox = new Rectangle
				{
					X = frameModel.TextureBox.X,
					Y = frameModel.TextureBox.Y,
					Width = frameSetWidth,
					Height = frameSetHeight
				};
			}

			return this.GetAnimationFromModel(animationModel);
		}
	}
}

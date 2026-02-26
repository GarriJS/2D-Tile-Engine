using Engine.DiskModels.Drawing;
using Engine.DiskModels.Drawing.Abstract;
using Engine.RunTime.Services.Contracts;
using Microsoft.Xna.Framework;

namespace Engine.Graphics.Models
{
	/// <summary>
	/// Represents a triggered animation.
	/// </summary>
	/// <remarks>
	/// Loops once per trigger before returning to the resting frame index.
	/// </remarks>
	sealed public class TriggeredAnimation : Animation
	{
		/// <summary>
		/// Gets a value indicating whether this animation has been triggered. 
		/// </summary>
		public bool AnimationIsTrigged { get => this.CurrentFrameIndex != this.RestingFrameIndex; }

		/// <summary>
		/// Gets or sets the resting frame index.
		/// </summary>
		required public int RestingFrameIndex { get; set; }

		/// <summary>
		/// Triggers the animation.
		/// </summary>
		/// <param name="allowReset">A value indicating whether to allow the animation trigger to reset.</param>
		public void TriggerAnimation(bool allowReset = false)
		{
			if ((false == allowReset) &&
				(this.RestingFrameIndex != this.CurrentFrameIndex))
				return;

			this.ElaspedFrameDuration = 0;
			this.CurrentFrameIndex = this.RestingFrameIndex;

			if (this.CurrentFrameIndex < this.Frames.Length - 1)
				this.CurrentFrameIndex++;
			else
				this.CurrentFrameIndex = 0;
		}

		/// <summary>
		/// Resets the triggered animation.
		/// </summary>>
		public void ResetTriggeredAnimation()
		{
			this.ElaspedFrameDuration = 0;
			this.CurrentFrameIndex = this.RestingFrameIndex;
		}

		/// <summary>
		/// Updates the updateable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		override protected void UpdateFrame(GameTime gameTime, GameServiceContainer gameServices)
		{
			if ((this.ElaspedFrameDuration == 0) ||
				(this.CurrentFrameIndex == this.RestingFrameIndex))
			{
				this.ElaspedFrameDuration += gameTime.ElapsedGameTime.TotalMilliseconds;

				return;
			}

			if (this.ElaspedFrameDuration > this.CurrentFrameDuration)
			{
				if (this.CurrentFrameIndex < this.Frames.Length - 1)
					this.CurrentFrameIndex++;
				else
					this.CurrentFrameIndex = 0;

				if ((true == this.FrameMinDuration.HasValue) &&
					(true == this.FrameMaxDuration.HasValue))
				{
					var randomService = gameServices.GetService<IRandomService>();
					this.CurrentFrameDuration = randomService.GetRandomInt(this.FrameMinDuration.Value, this.FrameMaxDuration.Value);
				}

				this.ElaspedFrameDuration = 0;
			}
			else
				this.ElaspedFrameDuration += gameTime.ElapsedGameTime.TotalMilliseconds;
		}

		/// <summary>
		/// Converts the object to a serialization model.
		/// </summary>
		/// <returns>The serialization model.</returns>
		override public GraphicBaseModel ToModel()
		{
			var frameModels = new SimpleImageModel[this.Frames.Length];

			for (int i = 0; i < this.Frames.Length; i++)
				frameModels[i] = (SimpleImageModel)this.Frames[i].ToModel();

			var result = new TriggeredAnimationModel
			{
				CurrentFrameIndex = this.CurrentFrameIndex,
				FrameDuration = this.CurrentFrameDuration,
				FrameMinDuration = this.FrameMinDuration,
				FrameMaxDuration = this.FrameMaxDuration,
				Frames = frameModels,
				RestingFrameIndex = this.RestingFrameIndex,
			};

			return result;
		}
	}
}

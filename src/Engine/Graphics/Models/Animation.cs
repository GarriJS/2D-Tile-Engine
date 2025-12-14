using Engine.DiskModels.Drawing;
using Engine.DiskModels.Drawing.Abstract;
using Engine.Graphics.Models.Contracts;
using Engine.Physics.Models;
using Engine.Physics.Models.SubAreas;
using Engine.RunTime.Services.Contracts;
using Microsoft.Xna.Framework;
using System.Linq;

namespace Engine.Graphics.Models
{
	/// <summary>
	/// Represents the animation.
	/// </summary>
	public class Animation : IAmAGraphic
	{
		/// <summary>
		/// Gets or sets the current frame index.
		/// </summary>
		public int CurrentFrameIndex { get; set; }

		/// <summary>
		/// Gets or sets the current frame duration in milliseconds.
		/// </summary>
		public int FrameDuration { get; set; }

		/// <summary>
		/// Gets or sets the frame min duration in milliseconds.
		/// </summary>
		public int? FrameMinDuration { get; set; }

		/// <summary>
		/// Gets or sets the frame max duration in milliseconds.
		/// </summary>
		public int? FrameMaxDuration { get; set; }

		/// <summary>
		/// Gets or sets the current frame start time in milliseconds.
		/// </summary>
		public double? FrameStartTime { get; set; }

		/// <summary>
		/// Gets the dimensions.
		/// </summary>
		public SubArea Dimensions { get => this.CurrentFrame.Dimensions; }

		/// <summary>
		/// Gets the current frame.
		/// </summary>
		public IAmAImage CurrentFrame { get => this.Frames[this.CurrentFrameIndex]; }

		/// <summary>
		/// Gets or sets the frames.
		/// </summary>
		public IAmAImage[] Frames { get; set; } = [];

		/// <summary>
		/// Sets the draw dimensions.
		/// </summary>
		/// <param name="dimensions">The dimensions.</param>
		public void SetDrawDimensions(SubArea dimensions)
		{
			foreach (var frame in this.Frames ?? [])
			{ 
				frame.SetDrawDimensions(dimensions);
			}
		}

		/// <summary>
		/// Draws the sub drawable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		/// <param name="position">The position.</param>
		/// <param name="offset">The offset.</param>
		public void Draw(GameTime gameTime, GameServiceContainer gameServices, Position position, Vector2 offset = default)
		{
			this.UpdateFrame(gameTime, gameServices);
			this.CurrentFrame.Draw(gameTime, gameServices, position, offset);
		}

		/// <summary>
		/// Updates the updateable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		virtual protected void UpdateFrame(GameTime gameTime, GameServiceContainer gameServices)
		{
			if (null == this.FrameStartTime)
			{
				this.FrameStartTime = gameTime.TotalGameTime.TotalMilliseconds;

				return;
			}

			if (gameTime.TotalGameTime.TotalMilliseconds >= this.FrameStartTime + this.FrameDuration)
			{
				if ((true == this.FrameMinDuration.HasValue) &&
					(true == this.FrameMaxDuration.HasValue))
				{
					var randomService = gameServices.GetService<IRandomService>();

					this.FrameDuration = randomService.GetRandomInt(this.FrameMinDuration.Value, this.FrameMaxDuration.Value);
				}

				if (this.CurrentFrameIndex < this.Frames.Length - 1)
				{
					this.CurrentFrameIndex++;
				}
				else
				{
					this.CurrentFrameIndex = 0;
				}

				this.FrameStartTime = gameTime.TotalGameTime.TotalMilliseconds;
			}
		}

		/// <summary>
		/// Converts the object to a serialization model.
		/// </summary>
		/// <returns>The serialization model.</returns>
		virtual public GraphicBaseModel ToModel()
		{
			var frameModels = this.Frames.Select(e => e.ToModel())
										 .ToArray();

			var result = new AnimationModel
			{
				CurrentFrameIndex = this.CurrentFrameIndex,
				FrameDuration = this.FrameDuration,
				FrameMinDuration = this.FrameMinDuration,
				FrameMaxDuration = this.FrameMaxDuration,
				//Frames = frameModels
			};

			return result;
		}

		/// <summary>
		/// Disposes of the draw data texture.
		/// </summary>
		public void Dispose()
		{
			foreach (var frame in this.Frames ?? [])
			{ 
				frame?.Dispose();
			}
		}
	}
}

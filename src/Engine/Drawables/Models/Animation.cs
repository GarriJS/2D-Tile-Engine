using Engine.Drawables.Models.Contracts;
using Engine.Physics.Models;
using Engine.RunTime.Services.Contracts;
using Microsoft.Xna.Framework;
using System;
using System.Linq;

namespace Engine.Drawables.Models
{
	/// <summary>
	/// Represents the animation.
	/// </summary>
	public class Animation : IAmSubDrawable, IDisposable
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
		/// Gets or sets the image.
		/// </summary>
		public Image Image { get => Frames[this.CurrentFrameIndex]; }

		/// <summary>
		/// Gets or sets the frames.
		/// </summary>
		public Image[] Frames { get; set; }

		/// <summary>
		/// Draws the sub drawable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		/// <param name="position">The position.</param>
		/// <param name="offset">The offset.</param>
		public void Draw(GameTime gameTime, GameServiceContainer gameServices, Position position, Vector2 offset = default)
		{
			var drawingService = gameServices.GetService<IDrawingService>();

			this.UpdateFrame(gameTime, gameServices);
			drawingService.Draw(gameTime, this, position, offset);
		}

		/// <summary>
		/// Updates the updateable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		protected virtual void UpdateFrame(GameTime gameTime, GameServiceContainer gameServices)
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
		/// Disposes of the draw data texture.
		/// </summary>
		public void Dispose()
		{
			if (true != this.Frames?.Any())
			{
				return;
			}

			foreach (var frame in this.Frames)
			{ 
				frame?.Dispose();
			}
		}
	}
}

using Engine.RunTime.Models.Contracts;
using System;

namespace Engine.Drawing.Models
{
	/// <summary>
	/// Represents the animation.
	/// </summary>
	public class Animation : IDisposable
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
		/// Gets or sets current frame start time in milliseconds.
		/// </summary>
		public double? FrameStartTime { get; set; }

		/// <summary>
		/// Gets or sets the current frame.
		/// </summary>
		public Sprite CurrentFrame { get => Frames[this.CurrentFrameIndex]; }

		/// <summary>
		/// Gets or sets the frames.
		/// </summary>
		public Sprite[] Frames { get; set; }

		/// <summary>
		/// Disposes of the draw data texture.
		/// </summary>
		public void Dispose()
		{
			foreach (var frame in this.Frames)
			{ 
				frame.Dispose();
			}
		}
	}
}

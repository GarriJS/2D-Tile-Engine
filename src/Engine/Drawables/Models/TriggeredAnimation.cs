namespace Engine.Drawables.Models
{
	/// <summary>
	/// Represents a triggered animation.
	/// </summary>
	/// <remarks>
	/// Loops once per trigger before returning to the resting frame index.
	/// </remarks>
	public class TriggeredAnimation : Animation
	{
		/// <summary>
		/// Gets a value indicating whether this animation has been triggered. 
		/// </summary>
		public bool AnimationIsTrigged { get => this.CurrentFrameIndex != this.RestingFrameIndex; }

		/// <summary>
		/// Gets or sets the resting frame index.
		/// </summary>
		public int RestingFrameIndex { get; set; }
	}
}

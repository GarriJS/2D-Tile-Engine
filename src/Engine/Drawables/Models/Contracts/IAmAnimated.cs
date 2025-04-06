using Engine.RunTime.Models.Contracts;

namespace Engine.Drawables.Models.Contracts
{
    /// <summary>
    /// Represents something animated.
    /// </summary>
    public interface IAmAnimated : IAmDrawable, IAmUpdateable
	{
		/// <summary>
		/// Gets the image.
		/// </summary>
		public new Image Image { get => Animation.CurrentFrame; }

		/// <summary>
		/// Gets the animation.
		/// </summary>
		public Animation Animation { get; }
	}
}

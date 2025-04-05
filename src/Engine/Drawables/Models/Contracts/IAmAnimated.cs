using Engine.RunTime.Models.Contracts;

namespace Engine.Drawables.Models.Contracts
{
    /// <summary>
    /// Represents something animated.
    /// </summary>
    public interface IAmAnimated : IAmDrawable, ICanBeUpdated
	{
		/// <summary>
		/// Gets the animation.
		/// </summary>
		public Animation Animation { get; }
	}
}

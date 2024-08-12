using Game.RunTime.Models.Contracts;

namespace Game.Drawing.Models.Contracts
{
    /// <summary>
    /// Represents something animated.
    /// </summary>
    public interface IAmAnimated : ICanBeDrawn, ICanBeUpdated
	{
		/// <summary>
		/// Gets the animation.
		/// </summary>
		public Animation Animation { get; }
	}
}

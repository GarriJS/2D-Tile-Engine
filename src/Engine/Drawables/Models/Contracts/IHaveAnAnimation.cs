using Engine.RunTime.Models.Contracts;

namespace Engine.Drawables.Models.Contracts
{
    /// <summary>
    /// Represents something with an animation.
    /// </summary>
    public interface IHaveAnAnimation : IHaveAnImage, ICanBeUpdated
	{
		/// <summary>
		/// Gets the image.
		/// </summary>
		public new Image Image { get => Animation.Image; }

		/// <summary>
		/// Gets the animation.
		/// </summary>
		public Animation Animation { get; }
	}
}

using Engine.RunTime.Models.Contracts;

namespace Engine.Drawables.Models.Contracts
{
    /// <summary>
    /// Represents something with an animation.
    /// </summary>
    public interface IHaveAnAnimation : IHaveAnImage, IAmUpdateable
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

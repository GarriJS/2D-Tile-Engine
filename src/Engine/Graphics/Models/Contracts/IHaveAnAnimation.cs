namespace Engine.Graphics.Models.Contracts
{
	/// <summary>
	/// Represents something with an animation.
	/// </summary>
	public interface IHaveAnAnimation : IHaveAGraphic
	{
		/// <summary>
		/// Gets the graphic.
		/// </summary>
		public new IAmAGraphic Graphic { get => this.Animation; }

		/// <summary>
		/// Gets the animation.
		/// </summary>
		public Animation Animation { get; }
	}
}

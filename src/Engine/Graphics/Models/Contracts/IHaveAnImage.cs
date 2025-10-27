namespace Engine.Graphics.Models.Contracts
{
	/// <summary>
	/// Represents something that has an image.
	/// </summary>
	public interface IHaveAnImage : IHaveAGraphic
	{
		/// <summary>
		/// Gets the graphic.
		/// </summary>
		new public IAmAGraphic Graphic { get => this.Image; }

		/// <summary>
		/// Gets the image.
		/// </summary>
		public SimpleImage Image { get; }
	}
}

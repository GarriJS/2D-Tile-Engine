using System;

namespace Engine.Graphics.Models.Contracts
{
	/// <summary>
	/// Represents something that has an image.
	/// </summary>
	public interface IHaveAnImage : IHaveAGraphic, IDisposable
	{
		/// <summary>
		/// Gets the graphic.
		/// </summary>
		public new IAmAGraphic Graphic { get => this.Image; }

		/// <summary>
		/// Gets the image.
		/// </summary>
		public Image Image { get; }
	}
}

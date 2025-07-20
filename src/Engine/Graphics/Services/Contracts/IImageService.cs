using Engine.DiskModels.Drawing;
using Engine.Graphics.Models;

namespace Engine.Graphics.Services.Contracts
{
	/// <summary>
	/// Represents a image service.
	/// </summary>
	public interface IImageService
	{
		/// <summary>
		/// Gets the graphic.
		/// </summary>
		/// <param name="imageModel">The image model.</param>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <returns>The image.</returns>
		public Image GetImage(ImageModel imageModel, int width, int height);

		/// <summary>
		/// Gets the graphic.
		/// </summary>
		/// <param name="textureName">The texture name.</param>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <returns>The image.</returns>
		public Image GetImage(string textureName, int width, int height);
	}
}

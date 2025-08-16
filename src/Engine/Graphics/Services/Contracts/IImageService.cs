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
		/// Gets the image from the model.
		/// </summary>
		/// <param name="imageModel">The image model.</param>
		/// <returns>The image.</returns>
		public Image GetImageFromModel(ImageModel imageModel);

		/// <summary>
		/// Gets the image from the model.
		/// </summary>
		/// <param name="imageModel">The image model.</param>
		/// <returns>The image.</returns>
		public T GetImageFromModel<T>(ImageModel imageModel) where T : Image;

		/// <summary>
		/// Gets the image.
		/// </summary>
		/// <param name="textureName">The texture name.</param>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <returns>The image.</returns>
		public Image GetImage(string textureName, int width, int height);
	}
}

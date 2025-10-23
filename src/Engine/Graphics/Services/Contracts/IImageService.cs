using Engine.DiskModels.Drawing;
using Engine.Graphics.Models;
using Microsoft.Xna.Framework.Graphics;

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

		/// <summary>
		/// Gets the image from the image by parts model.
		/// </summary>
		/// <param name="imageByPartsModel">The image by parts model.</param>
		/// <returns>The image.</returns>
		public Image GetImageFromImageByPartsModel(ImageByPartsModel imageByPartsModel);

		/// <summary>
		/// Combines the textures into one texture.
		/// </summary>
		/// <param name="images">The image.</param>
		/// <returns>The combined texture.</returns>
		public Texture2D CombineTextures(Image[][] images);
	}
}

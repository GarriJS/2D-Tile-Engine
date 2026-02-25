using Engine.DiskModels.Drawing.Abstract;
using Engine.Graphics.Models;
using Engine.Graphics.Models.Contracts;
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
		/// <param name="model">The model.</param>
		/// <returns>The image.</returns>
		public IAmAImage GetImageFromModel(ImageBaseModel model);

		/// <summary>
		/// Gets the image from the model.
		/// </summary>
		/// <param name="model">The model.</param>
		/// <returns>The image.</returns>
		public T GetImageFromModel<T>(ImageBaseModel model) where T : IAmAImage;

		/// <summary>
		/// Gets the image.
		/// </summary>
		/// <param name="textureName">The texture name.</param>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <returns>The image.</returns>
		public SimpleImage GetImage(string textureName, int width, int height);

		/// <summary>
		/// Combines the image textures into one texture.
		/// </summary>
		/// <param name="images">The image.</param>
		/// <returns>The combined texture.</returns>
		public Texture2D CombineImageTextures(SimpleImage[][] images);
	}
}

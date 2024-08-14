using DiscModels.Engine.Drawing;
using Engine.Drawing.Models;

namespace Engine.Drawing.Services.Contracts
{
	/// <summary>
	/// Represents a image service.
	/// </summary>
	public interface IImageService
	{
		/// <summary>
		/// Gets the image.
		/// </summary>
		/// <param name="imageModel"></param>
		/// <returns></returns>
		public Image GetImage(ImageModel imageModel);
	}
}

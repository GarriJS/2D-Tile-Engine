using Engine.DiskModels.Drawing;
using Engine.Graphics.Models;

namespace Engine.Graphics.Services.Contracts
{
	/// <summary>
	/// Represents a independent image service.
	/// </summary>
	public interface IIndependentImageService
	{
		/// <summary>
		/// Gets the independent image.
		/// </summary>
		/// <param name="independentImageModel">The independent image model.</param>
		/// <returns>The independent image.</returns>
		public IndependentImage GetImage(IndependentImageModel independentImageModel, int width, int height);
	}
}

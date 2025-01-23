using Engine.DiskModels.Engine.Drawing;
using Engine.Drawing.Models;

namespace Engine.Drawing.Services.Contracts
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

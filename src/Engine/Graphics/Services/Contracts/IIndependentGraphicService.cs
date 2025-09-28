using Engine.DiskModels.Drawing;
using Engine.Graphics.Models;

namespace Engine.Graphics.Services.Contracts
{
	/// <summary>
	/// Represents a independent graphic service.
	/// </summary>
	public interface IIndependentGraphicService
	{
		/// <summary>
		/// Gets the independent graphic from the image.
		/// </summary>
		/// <param name="independentGraphicModel">The independent graphic model.</param>
		/// <param name="drawLayer">The draw layer.</param>
		/// <returns>The independent graphic.</returns>
		public IndependentGraphic GetIndependentGraphicFromModel(IndependentGraphicModel independentGraphicModel, int drawLayer = 0);
	}
}

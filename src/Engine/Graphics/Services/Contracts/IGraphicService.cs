using Engine.DiskModels.Drawing;
using Engine.DiskModels.Drawing.Contracts;
using Engine.Graphics.Models;
using Engine.Graphics.Models.Contracts;

namespace Engine.Graphics.Services.Contracts
{   
	/// <summary>
	/// Represents a graphic service.
	/// </summary>
	public interface IGraphicService
	{
		/// <summary>
		/// Gets the graphic from the model.
		/// </summary>
		/// <param name="model">The model.</param>
		/// <returns>The graphic.</returns>
		public IAmAGraphic GetGraphicFromModel(IAmAGraphicModel model);

		/// <summary>
		/// Gets the texture region from the model.
		/// </summary>
		/// <param name="textureRegionModel">The texture region model.</param>
		/// <returns>The texture region.</returns>
		public TextureRegion GetTextureRegionFromModel(TextureRegionModel textureRegionModel);
	}
}

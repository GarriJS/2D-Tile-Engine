using DiscModels.Engine.Drawing;
using Engine.Core.Constants;
using Engine.Core.Textures.Contracts;
using Engine.Drawing.Models;
using Engine.Drawing.Services.Contracts;
using Engine.Physics.Services.Contracts;
using Microsoft.Xna.Framework;

namespace Engine.Drawing.Services
{
	/// <summary>
	/// Represents a independent image service.
	/// </summary>
	/// <remarks>
	/// Initializes the independent image service.
	/// </remarks>
	/// <param name="gameService">The game services.</param>
	public class IndependentImageService(GameServiceContainer gameService) : IIndependentImageService
	{
		private readonly GameServiceContainer _gameServices = gameService;

		/// <summary>
		/// Gets the independent image.
		/// </summary>
		/// <param name="independentImageModel">The independent image model.</param>
		/// <returns>The independent image.</returns>
		public IndependentImage GetImage(IndependentImageModel independentImageModel, int width, int height)
		{
			var textureService = this._gameServices.GetService<ITextureService>();
			var positionService = this._gameServices.GetService<IPositionService>();
			var position = positionService.GetPosition(independentImageModel.Position);

			if (false == textureService.TryGetTexture(independentImageModel.TextureName, out var texture))
			{
				texture = textureService.DebugTexture;
			}

			var textureBox = new Rectangle(TextureConstants.TEXTURE_EXTENSION_AMOUNT,
										   TextureConstants.TEXTURE_EXTENSION_AMOUNT,
										   width,
										   height);

			return new IndependentImage
			{
				TextureName = independentImageModel.TextureName,
				TextureBox = textureBox,
				Texture = texture,
				Position = position
			};
		}
	}
}

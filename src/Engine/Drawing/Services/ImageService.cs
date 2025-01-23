using Engine.DiskModels.Engine.Drawing;
using Engine.Core.Constants;
using Engine.Core.Textures.Contracts;
using Engine.Drawing.Models;
using Engine.Drawing.Services.Contracts;
using Microsoft.Xna.Framework;

namespace Engine.Drawing.Services
{
	/// <summary>
	/// Represents a image service.
	/// </summary>
	/// <remarks>
	/// Initializes the image service.
	/// </remarks>
	/// <param name="gameService">The game services.</param>
	public class ImageService(GameServiceContainer gameService) : IImageService
	{
		private readonly GameServiceContainer _gameServices = gameService;

		/// <summary>
		/// Gets the image.
		/// </summary>
		/// <param name="imageModel">The image model.</param>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <returns>The image.</returns>
		public Image GetImage(ImageModel imageModel, int width, int height)
		{
			if (null == imageModel)
			{
				return null;
			}

			var textureService = this._gameServices.GetService<ITextureService>();

			if (false == textureService.TryGetTexture(imageModel.TextureName, out var texture))
			{
				texture = textureService.DebugTexture;
			}

			var textureBox = new Rectangle(TextureConstants.TEXTURE_EXTENSION_AMOUNT,
										   TextureConstants.TEXTURE_EXTENSION_AMOUNT,
										   width,
										   height);

			return new Image
			{
				TextureName = imageModel.TextureName,
				TextureBox = textureBox,
				Texture = texture
			};
		}

		/// <summary>
		/// Gets the image.
		/// </summary>
		/// <param name="textureName">The texture name.</param>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <returns>The image.</returns>
		public Image GetImage(string textureName, int width, int height)
		{
			if (true == string.IsNullOrEmpty(textureName))
			{
				return null;
			}

			var textureService = this._gameServices.GetService<ITextureService>();

			if (false == textureService.TryGetTexture(textureName, out var texture))
			{
				texture = textureService.DebugTexture;
			}

			var textureBox = new Rectangle(TextureConstants.TEXTURE_EXTENSION_AMOUNT,
										   TextureConstants.TEXTURE_EXTENSION_AMOUNT,
										   width,
										   height);

			return new Image
			{
				TextureName = textureName,
				TextureBox = textureBox,
				Texture = texture
			};
		}
	}
}

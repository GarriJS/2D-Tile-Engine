using DiscModels.Engine.Drawing;
using Engine.Drawing.Models;
using Engine.Drawing.Services.Contracts;
using Engine.Physics.Services.Contracts;
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
		private readonly GameServiceContainer _gameServiceContainer = gameService;

		/// <summary>
		/// Gets the image.
		/// </summary>
		/// <param name="imageModel"></param>
		/// <returns></returns>
		public Image GetImage(ImageModel imageModel)
		{
			var spriteService = this._gameServiceContainer.GetService<ISpriteService>();
			var positionService = this._gameServiceContainer.GetService<IPositionService>();
			var sprite = spriteService.GetSprite(imageModel.Sprite);
			var position = positionService.GetPosition(imageModel.Position);

			return new Image
			{
				Position = position,
				Sprite = sprite
			};
		}
	}
}

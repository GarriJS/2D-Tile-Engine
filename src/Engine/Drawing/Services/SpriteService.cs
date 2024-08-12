using DiskModels.Engine.Drawing;
using Engine.Core.Constants;
using Engine.Core.Textures.Contracts;
using Engine.Drawing.Models;
using Engine.Drawing.Services.Contracts;
using Microsoft.Xna.Framework;

namespace Engine.Drawing.Services
{
	/// <summary>
	/// Represents a sprite service.
	/// </summary>
	public class SpriteService : ISpriteService
	{
		private readonly GameServiceContainer _gameServiceContainer;

		/// <summary>
		/// Initializes the sprite service.
		/// </summary>
		/// <param name="gameServices">The game services.</param>
		public SpriteService(GameServiceContainer gameServices)
		{
			this._gameServiceContainer = gameServices;
		}

		/// <summary>
		/// Gets the sprite.
		/// </summary>
		/// <param name="spriteModel">The sprite model.</param>
		/// <returns>The sprite.</returns>
		public Sprite GetSprite(SpriteModel spriteModel)
		{
			var textureService = this._gameServiceContainer.GetService<ITextureService>();
			var texture = textureService.GetTexture(spriteModel.SpritesheetName, spriteModel.SpritesheetBox);
			var textureBox = new Rectangle(TextureConstants.TEXTURE_EXTENSION_AMOUNT,
										   TextureConstants.TEXTURE_EXTENSION_AMOUNT,
										   spriteModel.SpritesheetBox.Width + TextureConstants.TEXTURE_EXTENSION_AMOUNT,
										   spriteModel.SpritesheetBox.Height + TextureConstants.TEXTURE_EXTENSION_AMOUNT);

			return new Sprite
			{
				DrawDataName = spriteModel.SpritesheetName + spriteModel.SpritesheetBox,
				SpritesheetName = spriteModel.SpritesheetName,
				SpritesheetCoordinate = spriteModel.SpritesheetBox.Location,
				TextureBox = textureBox,
				SpritesheetBox = spriteModel.SpritesheetBox,
				Texture = texture
			};
		}
	}
}

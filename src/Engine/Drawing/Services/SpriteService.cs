using Engine.DiskModels.Drawing;
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
	/// <remarks>
	/// Initializes the sprite service.
	/// </remarks>
	/// <param name="gameServices">The game services.</param>
	public class SpriteService(GameServiceContainer gameServices) : ISpriteService
	{
		private readonly GameServiceContainer _gameServices = gameServices;

		/// <summary>
		/// Gets the sprite.
		/// </summary>
		/// <param name="spriteModel">The sprite model.</param>
		/// <returns>The sprite.</returns>
		public Sprite GetSprite(SpriteModel spriteModel)
		{
			var textureService = this._gameServices.GetService<ITextureService>();
			var textureName = textureService.GetTextureName(spriteModel.TextureName, spriteModel.SpritesheetBox);

			if (false == textureService.TryGetTexture(textureName, out var texture))
			{
				texture = textureService.DebugTexture;
			}

			var textureBox = new Rectangle(TextureConstants.TEXTURE_EXTENSION_AMOUNT,
										   TextureConstants.TEXTURE_EXTENSION_AMOUNT,
										   spriteModel.SpritesheetBox.Width,
										   spriteModel.SpritesheetBox.Height);

			return new Sprite
			{
				TextureName = textureName,
				SpritesheetCoordinate = spriteModel.SpritesheetBox.Location,
				TextureBox = textureBox,
				SpritesheetBox = spriteModel.SpritesheetBox,
				Texture = texture
			};
		}
	}
}

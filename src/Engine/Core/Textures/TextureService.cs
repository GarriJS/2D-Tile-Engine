using Engine.Core.Textures.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Engine.Core.Textures
{
	/// <summary>
	/// Represents a texture service.
	/// </summary>
	public class TextureService : ITextureService
	{
		private readonly GameServiceContainer _gameServiceContainer;
		
		/// <summary>
		/// Gets or sets the textures by texture name.
		/// </summary>
		private Dictionary<string, Texture> Textures { get; set; }

		/// <summary>
		/// Initializes the texture service.
		/// </summary>
		/// <param name="gameServices">The game service.</param>
		public TextureService(GameServiceContainer gameServices) 
		{
			this._gameServiceContainer = gameServices;
			this.Textures = new Dictionary<string, Texture>();
		}

		/// <summary>
		/// Gets the texture.
		/// </summary>
		/// <param name="spritesheet">The spritesheet.</param>
		/// <param name="spritesheetBox">The spritesheet box.</param>
		/// <returns>The texture.</returns>
		public Texture2D GetTexture(string spritesheet, Rectangle spritesheetBox)
		{

			return null;
		}
	}
}

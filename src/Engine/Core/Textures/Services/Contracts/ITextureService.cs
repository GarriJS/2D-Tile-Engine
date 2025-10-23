using Engine.Core.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Core.Textures.Services.Contracts
{
	/// <summary>
	/// Represents a texture service.
	/// </summary>
	public interface ITextureService : ILoadContent
	{
		/// <summary>
		/// Gets the debug texture.
		/// </summary>
		public Texture2D DebugTexture { get; }

		/// <summary>
		/// Tries to get the texture.
		/// </summary>
		/// <param name="textureName">The texture name.</param>
		/// <param name="texture">The texture.</param>
		/// <returns>A value indicating whether the texture was found.</returns>
		public bool TryGetTexture(string textureName, out Texture2D texture);

		/// <summary>
		/// Gets the texture name.
		/// </summary>
		/// <param name="spritesheet">The sprite sheet.</param>
		/// <param name="spritesheetBox">The spritesheet box.</param>
		/// <returns>The texture name.</returns>
		public string GetTextureName(string spritesheet, Rectangle spritesheetBox);

		/// <summary>
		/// Extends the texture.
		/// </summary>
		/// <param name="textureData">The texture data.</param>
		/// <param name="sourceTextureWidth">The source texture width.</param>
		/// <param name="sourceTextureHeight">The source texture height.</param>
		/// <param name="extendAmount">The extend amount.</param>
		/// <returns>The extended texture.</returns>
		public Texture2D ExtendTexture(Color[] textureData, int sourceTextureWidth, int sourceTextureHeight, int extendAmount);
	}
}

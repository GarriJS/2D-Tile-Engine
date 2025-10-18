using Engine.Core.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Core.Textures.Contracts
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
	}
}

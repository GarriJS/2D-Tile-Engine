using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Core.Textures.Contracts
{
	/// <summary>
	/// Represents a texture service.
	/// </summary>
	public interface ITextureService
	{
		/// <summary>
		/// Gets the texture.
		/// </summary>
		/// <param name="spritesheet">The spritesheet.</param>
		/// <param name="spritesheetBox">The spritesheet box.</param>
		/// <returns>The texture.</returns>
		public Texture2D GetTexture(string spritesheet, Rectangle spritesheetBox);
	}
}

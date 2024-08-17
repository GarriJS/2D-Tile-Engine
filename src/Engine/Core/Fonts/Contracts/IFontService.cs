using Engine.Core.Contracts;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Core.Fonts.Contracts
{
	/// <summary>
	/// Represents a font service.
	/// </summary>
	public interface IFontService : ILoadContent
	{
		/// <summary>
		/// Gets the sprite font.
		/// </summary>
		/// <param name="spriteFontName">The sprite font name.</param>
		/// <returns>The sprite font.</returns>
		public SpriteFont GetSpriteFont(string spriteFontName);
	}
}

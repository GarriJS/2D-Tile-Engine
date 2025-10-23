using Engine.Core.Contracts;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Core.Fonts.Services.Contracts
{
	/// <summary>
	/// Represents a font service.
	/// </summary>
	public interface IFontService : ILoadContent
	{
		/// <summary>
		/// Gets or sets the debug sprite font.
		/// </summary>
		public SpriteFont DebugSpriteFont { get; }

		/// <summary>
		/// Gets the sprite font.
		/// </summary>
		/// <param name="spriteFontName">The sprite font name.</param>
		/// <returns>The sprite font.</returns>
		public SpriteFont GetSpriteFont(string spriteFontName);

		/// <summary>
		/// Sets the debug sprite front.
		/// </summary>
		/// <param name="spriteFontName">The sprite font name.</param>
		public void SetDebugSpriteFont(string spriteFontName);
	}
}

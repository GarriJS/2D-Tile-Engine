using Engine.Core.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Drawables.Services.Contracts
{
	/// <summary>
	/// Represents a writing service.
	/// </summary>
	public interface IWritingService : INeedInitialization
	{
		/// <summary>
		/// Measures the string.
		/// </summary>
		/// <param name="fontName">The font name.</param>
		/// <param name="text">The text.</param>
		/// <returns>The string measurements.</returns>
		public Vector2 MeasureString(string fontName, string text);

		/// <summary>
		/// Measures the string.
		/// </summary>
		/// <param name="font">The font.</param>
		/// <param name="text">The text.</param>
		/// <returns>The string measurements.</returns>
		public Vector2 MeasureString(SpriteFont font, string text);

		/// <summary>
		/// Draws the text.
		/// </summary>
		/// <param name="fontName">The font name.</param>
		/// <param name="text">The text.</param>
		/// <param name="position">The position.</param>
		/// <param name="color">The color.</param>
		public void Draw(string fontName, string text, Vector2 position, Color color);

		/// <summary>
		/// Draws the text.
		/// </summary>
		/// <param name="font">The font.</param>
		/// <param name="text">The text.</param>
		/// <param name="position">The position.</param>
		/// <param name="color">The color.</param>
		public void Draw(SpriteFont font, string text, Vector2 position, Color color);
	}
}

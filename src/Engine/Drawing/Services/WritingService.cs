using Engine.Core.Fonts.Contracts;
using Engine.Drawing.Services.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Drawing.Services

{   /// <summary>
	/// Represents a writing service.
	/// </summary>
	/// <remarks>
	/// Initializes the writing service.
	/// </remarks>
	/// <param name="gameServices">The game service.</param>
	public class WritingService(GameServiceContainer gameServices) : IWritingService
	{
		private readonly GameServiceContainer _gameServices = gameServices;

		/// <summary>
		/// Gets the spritebatch.
		/// </summary>
		private SpriteBatch SpriteBatch { get; set; }
	
		/// <summary>
		/// Performs initializes.
		/// </summary>
		public void Initialize()
		{
			var drawingService = this._gameServices.GetService<IDrawingService>();
			this.SpriteBatch = drawingService.SpriteBatch;
		}

		/// <summary>
		/// Measures the string.
		/// </summary>
		/// <param name="fontName">The font name.</param>
		/// <param name="text">The text.</param>
		/// <returns>The string measurements.</returns>
		public Vector2 MeasureString(string fontName, string text)
		{
			var fontService = this._gameServices.GetService<IFontService>();
			var font = fontService.GetSpriteFont(fontName);
			return font.MeasureString(text);
		}

		/// <summary>
		/// Measures the string.
		/// </summary>
		/// <param name="font">The font.</param>
		/// <param name="text">The text.</param>
		/// <returns>The string measurements.</returns>
		public Vector2 MeasureString(SpriteFont font, string text)
		{
			return font.MeasureString(text);
		}

		/// <summary>
		/// Draws the text.
		/// </summary>
		/// <param name="fontName">The font name.</param>
		/// <param name="text">The text.</param>
		/// <param name="position">The position.</param>
		/// <param name="color">The color.</param>
		public void Draw(string fontName, string text, Vector2 position, Color color)
		{
			var fontService = this._gameServices.GetService<IFontService>();		
			var font = fontService.GetSpriteFont(fontName);
			this.Draw(font, text, position, color);
		}

		/// <summary>
		/// Draws the text.
		/// </summary>
		/// <param name="font">The font.</param>
		/// <param name="text">The text.</param>
		/// <param name="position">The position.</param>
		/// <param name="color">The color.</param>
		public void Draw(SpriteFont font, string text, Vector2 position, Color color)
		{
			this.SpriteBatch.DrawString(font, text, position, color);
		}
	}
}

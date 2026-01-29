using Engine.Core.Fonts.Services.Contracts;
using Engine.RunTime.Services.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.RunTime.Services

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
		/// Configures the service.
		/// </summary>
		public void ConfigureService()
		{
			var drawingService = _gameServices.GetService<IDrawingService>();

			this.SpriteBatch = drawingService.SpriteBatch;
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
			var fontService = _gameServices.GetService<IFontService>();		

			var font = fontService.GetSpriteFont(fontName);
			Draw(font, text, position, color);
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

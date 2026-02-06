using Engine.Core.Fonts.Services.Contracts;
using Engine.DiskModels.Drawing;
using Engine.Graphics.Models;
using Engine.Graphics.Services.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Graphics.Services
{
	/// <summary>
	/// Represents a graphic text service.
	/// </summary>
	/// <remarks>
	/// Initializes the graphic text service.
	/// </remarks>
	/// <param name="gameService">The game services.</param>
	sealed public class GraphicTextService(GameServiceContainer gameService) : IGraphicTextService
	{
		readonly private GameServiceContainer _gameServices = gameService;

		/// <summary>
		/// Measures the string.
		/// </summary>
		/// <param name="fontName">The font name.</param>
		/// <param name="text">The text.</param>
		/// <returns>The string measurements.</returns>
		public Vector2 MeasureString(string fontName, string text)
		{
			var fontService = _gameServices.GetService<IFontService>();
			var font = fontService.GetSpriteFont(fontName);
			var result = font.MeasureString(text);

			return result;
		}

		/// <summary>
		/// Measures the string.
		/// </summary>
		/// <param name="font">The font.</param>
		/// <param name="text">The text.</param>
		/// <returns>The string measurements.</returns>
		public Vector2 MeasureString(SpriteFont font, string text)
		{
			var result = font.MeasureString(text);

			return result;
		}

		/// <summary>
		/// Gets the graphic text from the model.
		/// </summary>
		/// <param name="model">The graphic text model.</param>
		/// <returns>The graphical text.</returns>
		public SimpleText GetGraphicTextFromModel(GraphicalTextModel model)
		{
			if (null == model)
				return null;

			var fontService = this._gameServices.GetService<IFontService>();
			var font = fontService.GetSpriteFont(model.FontName);
			font ??= fontService.DebugSpriteFont;
			var result = new SimpleText
			{
				MaxLineWidth = null,
				FontName = model.FontName,
				Text = model.Text,
				TextColor = model.TextColor,
				Font = font
			};

			return result;
		}
	}
}

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
	public class GraphicTextService(GameServiceContainer gameService) : IGraphicTextService
	{
		private readonly GameServiceContainer _gameServices = gameService;

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
		/// Gets the graphic text from the model.
		/// </summary>
		/// <param name="graphicalTextModel">The graphic text model.</param>
		/// <returns>The graphical text.</returns>
		public GraphicalText GetGraphicTextFromModel(GraphicalTextModel graphicalTextModel)
		{
			if (null == graphicalTextModel)
			{
				return null;
			}

			var fontService = this._gameServices.GetService<IFontService>();

			var font = fontService.GetSpriteFont(graphicalTextModel.FontName);
			font ??= fontService.DebugSpriteFont;

			return new GraphicalText
			{
				FontName = graphicalTextModel.FontName,
				Text = graphicalTextModel.Text,
				TextColor = graphicalTextModel.TextColor,
				Font = font
			};
		}
	}
}

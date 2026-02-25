using Engine.Core.Fonts.Services.Contracts;
using Engine.DiskModels.Drawing;
using Engine.DiskModels.Drawing.Abstract;
using Engine.Graphics.Models;
using Engine.Graphics.Models.Contracts;
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
			var fontService = this._gameServices.GetService<IFontService>();
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
		/// <param name="model">The model.</param>
		/// <returns></returns>
		public IAmGraphicText GetGraphicTextFromModel(GraphicalTextBaseModel model)
		{ 
			var result = this.GetGraphicTextFromModel<IAmGraphicText>(model);

			return result;
		}

		/// <summary>
		/// Gets the graphic text from the model.
		/// </summary>
		/// <typeparam name="T">The type of graphic text.</typeparam>
		/// <param name="model">The model.</param>
		/// <returns>The graphic text.</returns>
		public T GetGraphicTextFromModel<T>(GraphicalTextBaseModel model) where T : IAmGraphicText
		{
			IAmGraphicText text = model switch
			{
				WritableTextModel writeableTextModel => this.GetWritableTextFromModel(writeableTextModel),
				SimpleTextModel simpleTextModel => this.GetSimpleTextFromModel(simpleTextModel),
				_ => null
			};

			if (text is null)
			{
				// LOGGING

				return default;
			}

			return (T)text;
		}

		/// <summary>
		/// Gets the simple text from the model.
		/// </summary>
		/// <param name="model">The graphic text model.</param>
		/// <returns>The simple text.</returns>
		public SimpleText GetSimpleTextFromModel(SimpleTextModel model)
		{
			if (model is null)
				return null;

			var fontService = this._gameServices.GetService<IFontService>();
			var font = fontService.GetSpriteFont(model.FontName);
			font ??= fontService.DebugSpriteFont;
			var result = new SimpleText
			{
				MaxLineWidth = model.MaxLineWidth,
				FontName = model.FontName,
				Text = model.Text,
				TextColor = model.TextColor,
				Font = font
			};

			return result;
		}

		/// <summary>
		/// Gets the writable text from the model.
		/// </summary>
		/// <param name="model">The model.</param>
		/// <returns>The writable text.</returns>
		public WritableText GetWritableTextFromModel(WritableTextModel model)
		{
			if (model is null)
				return null;

			var fontService = this._gameServices.GetService<IFontService>();
			var font = fontService.GetSpriteFont(model.FontName);
			font ??= fontService.DebugSpriteFont;
			var result = new WritableText
			{
				MaxLineWidth = model.MaxLineWidth,
				FontName = model.FontName,
				Text = model.Text,
				TextColor = model.TextColor,
				Font = font
			};

			return result;
		}
	}
}

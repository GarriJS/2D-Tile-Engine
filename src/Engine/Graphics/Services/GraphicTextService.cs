using Engine.Controls.Typing.Models;
using Engine.Core.Fonts.Services.Contracts;
using Engine.DiskModels.Drawing;
using Engine.Graphics.Models;
using Engine.Graphics.Models.Contracts;
using Engine.Graphics.Services.Contracts;
using Engine.Physics.Models.SubAreas;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

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
		/// Measures the text lines.
		/// </summary>
		/// <param name="fontName">The font name.</param>
		/// <param name="textLines">The text lines.</param>
		/// <returns>The text line measurements.</returns>
		public Vector2 MeasureTextLines(string fontName, IList<string> textLines)
		{
            var fontService = this._gameServices.GetService<IFontService>();
            var font = fontService.GetSpriteFont(fontName);
            var result = this.MeasureTextLines(font, textLines);

            return result;
        }

        /// <summary>
        /// Measures the text lines.
        /// </summary>
        /// <param name="font">The font.</param>
        /// <param name="textLines">The text lines.</param>
        /// <returns>The text line measurements.</returns>
        public Vector2 MeasureTextLines(SpriteFont font, IList<string> textLines)
		{ 
			var result = new Vector2(0, 0);

			foreach (var textLine in textLines)
			{ 
				var textLineMeasurement = this.MeasureString(font, textLine);
				result.X = MathHelper.Max(result.X, textLineMeasurement.X);
				result.Y += textLineMeasurement.Y;
            }

			return result;
		}

        /// <summary>
        /// Gets the text line from the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>The text line.</returns>
        public TextLine GetTextLineFromModel(TextLineModel model)
        {
            if (model is null)
                return null;

            var result = new TextLine
            {
                IsManualBreak = model.IsManualBreak,
                Text = model.Text
            };

            return result;
        }

        /// <summary>
        /// Gets the graphic text from the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public IAmGraphicText GetGraphicTextFromModel(GraphicalTextModel model)
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
		public T GetGraphicTextFromModel<T>(GraphicalTextModel model) where T : IAmGraphicText
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
            var textLines = new List<TextLine>(model.TextLines.Length);

            for (int i = 0; i < model.TextLines.Length; i++)
                textLines.Add(this.GetTextLineFromModel(model.TextLines[i]));

            var result = new SimpleText
			{
				MaxLineCharacterCount = model.MaxLineCharacterCount,
				MaxLinesCount = model.MaxLinesCount,
				FontName = model.FontName,
				TextLines = textLines,
				TextColor = model.TextColor,
				Font = font
			};

			return result;
		}

		/// <summary>
		/// Gets the writable text from the model.
		/// </summary>
		/// <param name="model">The model.</param>
		/// <param name="maxLineWidth">The max line width.</param>
		/// <returns>The writable text.</returns>
		public WritableText GetWritableTextFromModel(WritableTextModel model, int? maxLineWidth = null)
		{
			if (model is null)
				return null;

            var fontService = this._gameServices.GetService<IFontService>();
			var font = fontService.GetSpriteFont(model.FontName);
			font ??= fontService.DebugSpriteFont;
            var textLines = new List<TextLine>(model.TextLines.Length);

            for (int i = 0; i < model.TextLines.Length; i++)
                textLines.Add(this.GetTextLineFromModel(model.TextLines[i]));

            var result = new WritableText
			{
				TextIsBeingEdited = false,
				MaxLinesCount = model.MaxLinesCount,
				MaxLineCharacterCount = model.MaxLineCharacterCount,
				FontName = model.FontName,
				TextLines = textLines,
				TextColor = model.TextColor,
				Font = font,
				TextHighlightingState = new TextHighlightingState
				{
					TextAnchor = null,
					TextHighlightColor = new Color(
						Color.DarkSlateBlue.R,
						Color.DarkSlateBlue.G,
						Color.DarkSlateBlue.B,
						(byte)51)
				},
				StartAnchor = default,
				EndAnchor = default,
				TextCursor = new TextCursor
				{
					Blink = true,
					IsVisible = true,
					ElaspedFrameDuration = 0,
					Color = Color.DarkSlateGray,
					Position = new TextPosition
					{
						Index = textLines.LastOrDefault()?.Text.Length ?? 0,
						Line = textLines?.Count - 1 ?? 0
                    },
					SubArea = new SubArea
					{
						Width = 2,
						Height = 15
					}
				}
			};

			return result;
		}
	}
}

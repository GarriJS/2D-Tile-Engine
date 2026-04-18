using Engine.Controls.Typing.Models;
using Engine.DiskModels.Drawing;
using Engine.Graphics.Models;
using Engine.Graphics.Models.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Engine.Graphics.Services.Contracts
{
	/// <summary>
	/// Represents a graphic text service.
	/// </summary>
	public interface IGraphicTextService
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
		/// Measures the text lines.
		/// </summary>
		/// <param name="fontName">The font name.</param>
		/// <param name="textLines">The text lines.</param>
		/// <returns>The text line measurements.</returns>
		public Vector2 MeasureTextLines(string fontName, IList<string> textLines);

        /// <summary>
        /// Measures the text lines.
        /// </summary>
        /// <param name="font">The font.</param>
        /// <param name="textLines">The text lines.</param>
        /// <returns>The text line measurements.</returns>
        public Vector2 MeasureTextLines(SpriteFont font, IList<string> textLines);
		
        /// <summary>
        /// Gets the text line from the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns>The text line.</returns>
        public TextLine GetTextLineFromModel(TextLineModel model);

        /// <summary>
        /// Gets the graphic text from the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public IAmGraphicText GetGraphicTextFromModel(GraphicalTextModel model);

		/// <summary>
		/// Gets the graphic text from the model.
		/// </summary>
		/// <typeparam name="T">The type of graphic text.</typeparam>
		/// <param name="model">The model.</param>
		/// <returns>The graphic text.</returns>
		public T GetGraphicTextFromModel<T>(GraphicalTextModel model) where T : IAmGraphicText;

		/// <summary>
		/// Gets the simple text from the model.
		/// </summary>
		/// <param name="model">The graphic text model.</param>
		/// <returns>The simple text.</returns>
		public SimpleText GetSimpleTextFromModel(SimpleTextModel model);

		/// <summary>
		/// Gets the writable text from the model.
		/// </summary>
		/// <param name="model">The model.</param>
		/// <param name="maxLineWidth">The max line width.</param>
		/// <returns>The writable text.</returns>
		public WritableText GetWritableTextFromModel(WritableTextModel model, int? maxLineWidth = null);
    }
}

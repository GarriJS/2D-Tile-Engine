using Engine.DiskModels.Drawing;
using Engine.DiskModels.Drawing.Abstract;
using Engine.Graphics.Models;
using Engine.Graphics.Models.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
		/// Gets the graphic text from the model.
		/// </summary>
		/// <param name="model">The model.</param>
		/// <returns></returns>
		public IAmGraphicText GetGraphicTextFromModel(GraphicalTextBaseModel model);

		/// <summary>
		/// Gets the graphic text from the model.
		/// </summary>
		/// <typeparam name="T">The type of graphic text.</typeparam>
		/// <param name="model">The model.</param>
		/// <returns>The graphic text.</returns>
		public T GetGraphicTextFromModel<T>(GraphicalTextBaseModel model) where T : IAmGraphicText;

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
		/// <returns>The writable text.</returns>
		public WritableText GetWritableTextFromModel(WritableTextModel model);
	}
}

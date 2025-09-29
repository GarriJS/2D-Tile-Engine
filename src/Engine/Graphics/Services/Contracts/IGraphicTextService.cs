using Engine.DiskModels.Drawing;
using Engine.Graphics.Models;
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
		/// <param name="graphicalTextModel">The graphic text model.</param>
		/// <returns>The graphical text.</returns>
		public GraphicalText GetGraphicTextFromModel(GraphicalTextModel graphicalTextModel);
	}
}

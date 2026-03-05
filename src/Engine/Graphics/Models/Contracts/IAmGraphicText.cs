using Engine.RunTime.Models.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Engine.Graphics.Models.Contracts
{
	/// <summary>
	/// Represents graphic text.
	/// </summary>
	public interface IAmGraphicText : IAmSubWritable
	{
		/// <summary>
		/// Gets or sets the max line character count.
		/// </summary>
		public int? MaxLineCharacterCount { get; set; }

		/// <summary>
		/// Gets or sets the max line count.
		/// </summary>
		public int? MaxLinesCount { get; set; }

		/// <summary>
		/// Gets the font name.
		/// </summary>
		public string FontName { get; }

		/// <summary>
		/// Gets the text lines.
		/// </summary>
		public List<string> TextLines { get; }

		/// <summary>
		/// Gets the text color
		/// </summary>
		public Color TextColor { get; }

		/// <summary>
		/// Gets the font.
		/// </summary>
		public SpriteFont Font { get; }

		/// <summary>
		/// Gets the text dimensions.
		/// </summary>
		/// <param name="alwaysGetFontHeight">A value indicating whether to always get the font height.</param>
		/// <returns>The text dimensions.</returns>
		public Vector2 GetTextDimensions(bool alwaysGetFontHeight = false);

		/// <summary>
		/// Determines if the text needs conforming.
		/// </summary>
		/// <param name="maintainExistingLineBreaks">A value indicating whether to maintain existing line breaks.</param>
		/// <returns>A value indicating whether the text needs conforming.</returns>
		public bool NeedsConforming(string text, bool maintainExistingLineBreaks = false);

		/// <summary>
		/// Conforms the text to its line limits and characters per line limit.
		/// </summary>
		/// <param name="maintainExistingLineBreaks">A value indicating whether to maintain the existing line breaks in the text.</param>
		public void ConformTextToLimits(bool maintainExistingLineBreaks = false);

	}
}

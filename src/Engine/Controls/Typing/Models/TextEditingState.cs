using Engine.Core.Fonts.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Engine.Controls.Typing.Models
{
	/// <summary>
	/// Represents a text editing state.
	/// </summary>
	public class TextEditingState
	{
		/// <summary>
		/// Gets a value indicating whether the text editing state is highlighting text.
		/// </summary>
		public bool IsHighlighting { get => 0 != this.SelectionOffset; }

		/// <summary>
		/// Gets or sets the text editor position.
		/// </summary>
		required public int TextEditorPosition { get; set; }

		/// <summary>
		/// Gets or sets the text editor selection offset.
		/// </summary>
		required public int SelectionOffset { get; set; }

		/// <summary>
		/// Gets or sets the typing cursor color.
		/// </summary>
		required public Color TypingCursorColor { get; set; }

		/// <summary>
		/// Gets or sets the text highlighting color.
		/// </summary>
		required public Color TextHighlightColor { get; set; }

		/// <summary>
		/// Gets or sets the typing cursor.
		/// </summary>
		required public TypingCursor TypingCursor { get; set; }

		/// <summary>
		/// Gets the highlighted text.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="font">The font.</param>
		/// <param name="coordinates">The coordinates.</param>
		/// <param name="offset">The offset.</param>
		/// <returns>The highlighted text.</returns>
		public Rectangle? GetHighlightedRectangle(string text, SpriteFont font, Vector2 coordinates, Vector2 offset = default)
		{
			if (string.IsNullOrEmpty(text))
				return null;

			if (false == this.IsHighlighting)
				return null;

			var start = Math.Min(this.TextEditorPosition, this.TextEditorPosition + this.SelectionOffset);
			var length = Math.Abs(this.SelectionOffset);

			if (0 > start)
				start = 0;

			if (start > text.Length)
				return null;

			if (start + length > text.Length)
				length = text.Length - start;

			if (0 >= length)
				return null;

			var highlightedText = text.Substring(start, length);
			var highlightedDimensions = font.MeasureString(highlightedText);
			var finalOffset = coordinates + offset;
			var horizontalPosition = 0 < this.SelectionOffset ?
				(int)finalOffset.X :
				(int)(finalOffset.X - highlightedDimensions.X);
			
			var result = new Rectangle
			{
				X = horizontalPosition,
				Y = (int)finalOffset.Y,
				Width = (int)highlightedDimensions.X,
				Height = (int)highlightedDimensions.Y
			};

			return result;
		}
	}
}

namespace Engine.Controls.Typing.Models
{
	/// <summary>
	/// Represents a text edit state.
	/// </summary>
	readonly public struct TextEditState
	{
		/// <summary>
		/// Gets the max line character count.
		/// </summary>
		readonly required public int? MaxLineCharacterCount { get; init; }

		/// <summary>
		/// Gets the max line count.
		/// </summary>
		readonly required public int? MaxLinesCount { get; init; }

		/// <summary>
		/// Gets the text cursor position.
		/// </summary>
		readonly required public TextPosition TextCursorPosition { get; init; }

		/// <summary>
		/// Gets the start anchor.
		/// </summary>
		readonly required public TextPosition StartAnchor { get; init; }

		/// <summary>
		/// Gets the end anchor.
		/// </summary>
		readonly required public TextPosition EndAnchor { get; init; }

		/// <summary>
		/// Gets the text highlighting state.
		/// </summary>
		readonly required public TextHighlightingState TextHighlightingState { get; init; }
	}
}

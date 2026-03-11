namespace Engine.Controls.Typing.Models
{
	/// <summary>
	/// Represents text constraints.
	/// </summary>
	readonly public struct TextConstraints
	{
		/// <summary>
		/// Gets or sets the max line character count.
		/// </summary>
		readonly required public int? MaxLineCharacterCount { get; init; }

		/// <summary>
		/// Gets or sets the max line count.
		/// </summary>
		readonly required public int? MaxLinesCount { get; init; }

		/// <summary>
		/// Gets or sets the text highlighting state.
		/// </summary>
		readonly required public TextHighlightingState TextHighlightingState { get; init; }
	}
}

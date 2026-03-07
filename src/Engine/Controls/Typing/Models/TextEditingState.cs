namespace Engine.Controls.Typing.Models
{
	/// <summary>
	/// Represents a text editing state.
	/// </summary>
	public struct TextEditingState
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
		required public TextHighlightingState TextHighlightingState { get; set; }

		/// <summary>
		/// Gets or sets the text lines.
		/// </summary>
		required public string[] TextLines { get; set; }
	}
}

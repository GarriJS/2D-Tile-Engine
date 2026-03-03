namespace Engine.Controls.Typing.Models
{
	/// <summary>
	/// Represents a typing result.
	/// </summary>
	sealed public class TypingResult
	{
		/// <summary>
		/// Gets or sets the text.
		/// </summary>
		required public string Text { get; set; }

		/// <summary>
		/// Gets or sets the text editing state.
		/// </summary>
		required public TextEditingState TextEditingState { get; set; }
	}
}

namespace Engine.Controls.Typing.Models.Contracts
{
	/// <summary>
	/// Represents something with a text highlighting state.
	/// </summary>
	public interface IHaveATextHighlightingState
	{
		/// <summary>
		/// Gets or sets the text editing state.
		/// </summary>
		public TextHighlightingState TextHighlightingState { get; set; }
	}
}

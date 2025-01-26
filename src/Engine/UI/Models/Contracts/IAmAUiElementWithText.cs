namespace Engine.UI.Models.Contracts
{
	/// <summary>
	/// Represents a user interface element with text
	/// </summary>
	public interface IAmAUiElementWithText : IAmAUiElement
	{
		/// <summary>
		/// Gets or sets the text.
		/// </summary>
		public string Text { get; set; }
	}
}

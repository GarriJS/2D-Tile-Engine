namespace Engine.Controls.Typing.Models.Contracts
{
	/// <summary>
	/// Represents something with a text editing state.
	/// </summary>
	public interface IHaveATextEditingState
	{
		/// <summary>
		/// Gets or sets the text editing state.
		/// </summary>
		public TextEditingState TextEditingState { get; set; }
	}
}

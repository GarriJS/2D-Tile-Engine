namespace Engine.Controls.Typing.Models
{
	/// <summary>
	/// Represents a text position.
	/// </summary>
	public struct TextPosition
	{
		/// <summary>
		/// Gets or sets the index.
		/// </summary>
		required public int Index { get; set; }

		/// <summary>
		/// Gets or sets the line.
		/// </summary>
		required public int Line { get; set; }
	}
}

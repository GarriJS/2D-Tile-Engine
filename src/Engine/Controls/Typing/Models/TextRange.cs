namespace Engine.Controls.Typing.Models
{
	/// <summary>
	/// Represents a text range.
	/// </summary>
	public struct TextRange
	{
		/// <summary>
		/// Gets or sets the start index.
		/// </summary>
		required public int StartIndex { get; set; }

		/// <summary>
		/// Gets or sets the length.
		/// </summary>
		required public int Length { get; set; }
	}
}

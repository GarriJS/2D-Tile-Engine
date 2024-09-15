namespace Engine.UserInterface.Models
{
	/// <summary>
	/// Represents a text cursor.
	/// </summary>
	public class TextCursor : Cursor
	{
		/// <summary>
		/// Gets or sets the cursor line index.
		/// </summary>
		public int CursorLineIndex { get; set; }

		/// <summary>
		/// Gets or sets the text line.
		/// </summary>
		public TextLine TextLine { get; set; }
	}
}

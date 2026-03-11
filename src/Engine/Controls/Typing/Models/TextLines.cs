namespace Engine.Controls.Typing.Models
{
    /// <summary>
    /// Represents a text line.
    /// </summary>
    sealed public class TextLine
    {
        /// <summary>
        /// Gets or sets a value indicating if this text line is a manual break.
        /// </summary>
        required public bool IsManualBreak { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        required public string Text { get; set; }
    }
}

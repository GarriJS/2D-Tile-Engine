using Microsoft.Xna.Framework;

namespace Engine.Controls.Typing.Models
{
	/// <summary>
	/// Represents a text highlighting state.
	/// </summary>
	public struct TextHighlightingState
	{
		/// <summary>
		/// Gets a value indicating whether the text highlighting state is highlighting text.
		/// </summary>
		readonly public bool IsHighlighting => this.TextAnchor is not null;

		/// <summary>
		/// Gets or sets the text anchor.
		/// </summary>
		required public TextPosition? TextAnchor { get; set; }

		/// <summary>
		/// Gets or sets the text highlighting color.
		/// </summary>
		required public Color TextHighlightColor { get; set; }
	}
}

using Microsoft.Xna.Framework;

namespace Engine.Graphics.Models
{
	/// <summary>
	/// Represents writable text.
	/// </summary>
	public class WritableText : SimpleText
	{
		/// <summary>
		/// Updates the text.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="conformText">A value indicating whether to conform the text.</param>
		/// <returns>The dimensions of the next text.</returns>
		public Vector2 UpdateText(string text, bool conformText = true)
		{
			this.Text = text;

			if (true == conformText)
				this.ConformTextToMaxWidth();
			
			var dimensions = this.GetTextDimensions();

			return dimensions;
		}
	}
}

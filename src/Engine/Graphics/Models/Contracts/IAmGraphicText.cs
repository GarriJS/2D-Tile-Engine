using Engine.RunTime.Models.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Graphics.Models.Contracts
{
	/// <summary>
	/// Represents graphic text.
	/// </summary>
	public interface IAmGraphicText : IAmSubWritable
	{
		/// <summary>
		/// Gets the max line width.
		/// </summary>
		public float? MaxLineWidth { get; }

		/// <summary>
		/// Gets the font name.
		/// </summary>
		public string FontName { get; }

		/// <summary>
		/// Gets the text.
		/// </summary>
		public string Text { get; }

		/// <summary>
		/// Gets the text color
		/// </summary>
		public Color TextColor { get; }

		/// <summary>
		/// Gets the font.
		/// </summary>
		public SpriteFont Font { get; }

		/// <summary>
		/// Gets the text dimensions.
		/// </summary>
		/// <returns>The text dimensions.</returns>
		public Vector2 GetTextDimensions();

		/// <summary>
		/// Conforms the text to the max width.
		/// </summary>
		/// <param name="maintainExistingLineBreaks">A value indicating whether to maintain the existing line breaks in the text.</param>
		public void ConformTextToMaxWidth(bool maintainExistingLineBreaks = false);

	}
}

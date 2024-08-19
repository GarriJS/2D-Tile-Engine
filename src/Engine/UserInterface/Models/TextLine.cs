using Engine.Drawing.Models;
using Engine.Drawing.Models.Contracts;
using Engine.Physics.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.UserInterface.Models
{
	/// <summary>
	/// Represents a text line.
	/// </summary>
	public class TextLine : IAmWriteableAndDrawable
	{
		/// <summary>
		/// Gets or sets the max visible text width.
		/// </summary>
		public int MaxVisibleTextWidth { get; set; }

		/// <summary>
		/// Gets or sets the text.
		/// </summary>
		public string Text { get; set; }

		/// <summary>
		/// Gets or set the text offset.
		/// </summary>
		public Vector2 TextOffset { get; set; }

		/// <summary>
		/// Gets or sets the background.
		/// </summary>
		public Sprite Background { get; set; }

		/// <summary>
		/// Gets or sets the sprite.
		/// </summary>
		public Sprite Sprite { get; set; }

		/// <summary>
		/// Gets or sets the position.
		/// </summary>
		public Position Position { get; set; }

		/// <summary>
		/// Gets or sets the font.
		/// </summary>
		public SpriteFont Font { get; set; }

		/// <summary>
		/// Disposes of the text input line.
		/// </summary>
		public void Dispose()
		{
			this.Sprite.Dispose();
		}
	}
}

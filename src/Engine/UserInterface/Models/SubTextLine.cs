using Engine.Drawing.Models;
using Engine.Physics.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Engine.UserInterface.Models
{
	/// <summary>
	/// Represents a sub text line.
	/// </summary>
	public class SubTextLine : IDisposable
	{
		/// <summary>
		/// Gets or sets the width.
		/// </summary>
		public int Width { get; set; }

		/// <summary>
		/// Gets or sets the text.
		/// </summary>
		public string Text { get; set; }

		/// <summary>
		/// Gets or set the text buffer.
		/// </summary>
		public Vector2 TextBuffer { get; set; }

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
			this.Sprite?.Dispose();
		}
	}
}

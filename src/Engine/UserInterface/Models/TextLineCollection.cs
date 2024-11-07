using Engine.Drawing.Models;
using Engine.Drawing.Models.Contracts;
using Engine.Physics.Models;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Engine.UserInterface.Models
{
	/// <summary>
	/// Represents a text line collection.
	/// </summary>
	public class TextLineCollection : IAmDrawable
	{
		/// <summary>
		/// Gets or sets the max visible text width.
		/// </summary>d
		public int Width { get; set; }

		/// <summary>
		/// Gets or sets the max visible text height.
		/// </summary>
		public int Height { get; set; }

		/// <summary>
		/// Gets or set the text offset.
		/// </summary>
		public Vector2 TextOffset { get; set; }

		/// <summary>
		/// Gets or sets the sprite.
		/// </summary>
		public Sprite Sprite { get; set; }

		/// <summary>
		/// Gets or sets the position.
		/// </summary>
		public Position Position { get; set; }

		/// <summary>
		/// Gets or sets the text lines.
		/// </summary>
		public List<SubTextLine> TextLines { get; set; }

		/// <summary>
		/// Disposes of the text line collection.
		/// </summary>
		public void Dispose()
		{ 
			this.Sprite?.Dispose();

			if (0 < this.TextLines.Count)
			{ 
				this.TextLines.ForEach(e => e.Dispose());
			}
		}
	}
}

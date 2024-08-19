using Engine.Drawing.Models.Contracts;
using Engine.Physics.Models;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Drawing.Models
{
	/// <summary>
	/// Represents drawable text.
	/// </summary>
	public class DrawableText : IAmWritable
	{
		/// <summary>
		/// Gets or sets the text.
		/// </summary>
		public string Text { get; set; }

		/// <summary>
		/// Gets or sets the position.
		/// </summary>
		public Position Position { get; set; }

		/// <summary>
		/// Gets or sets the font. 
		/// </summary>
		public SpriteFont Font { get; set; }
	}
}

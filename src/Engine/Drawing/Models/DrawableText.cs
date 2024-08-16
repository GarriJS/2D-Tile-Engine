using Engine.Physics.Models;
using Engine.Physics.Models.Contracts;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Drawing.Models
{
	/// <summary>
	/// Represents drawable text.
	/// </summary>
	public class DrawableText : IHavePosition
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
		/// Gets or sets the sprite font. 
		/// </summary>
		public SpriteFont SpriteFont { get; set; }
	}
}

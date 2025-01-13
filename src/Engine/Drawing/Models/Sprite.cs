using Microsoft.Xna.Framework;

namespace Engine.Drawing.Models
{
	/// <summary>
	/// Represents a sprite.
	/// </summary>
	public class Sprite : Image	
	{
		/// <summary>
		/// Gets or sets the spritesheet coordinate.
		/// </summary>
		public Point SpritesheetCoordinate { get; set; }

		/// <summary>
		/// Gets the spritesheet box.
		/// </summary>
		public Rectangle SpritesheetBox { get; set; }
	}
}

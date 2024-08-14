using Engine.Drawing.Models.Contracts;
using Engine.Physics.Models;

namespace Engine.Drawing.Models
{
	/// <summary>
	/// Represents a image.
	/// </summary>
	public class Image : IAmDrawable
	{
		/// <summary>
		/// Gets or sets the position.
		/// </summary>
		public Position Position { get; set; }

		/// <summary>
		/// Gets or sets the sprite.
		/// </summary>
		public Sprite Sprite { get; set; }

		/// <summary>
		/// Disposes of the image.
		/// </summary>
		public void Dispose()
		{
			this.Sprite.Dispose();
		}
	}
}

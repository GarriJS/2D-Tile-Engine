using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Drawables.Models
{
	/// <summary>
	/// Represents a image.
	/// </summary>
	public class Image
	{
		/// <summary>
		/// Gets or sets the texture name.
		/// </summary>
		public string TextureName { get; set; }

		/// <summary>
		/// Gets or sets the texture box.
		/// </summary>
		public Rectangle TextureBox { get; set; }

		/// <summary>
		/// Gets or sets the texture.
		/// </summary>
		public Texture2D Texture { get; set; }

		/// <summary>
		/// Disposes of the draw data texture.
		/// </summary>
		public void Dispose()
		{
			this.Texture?.Dispose();
		}
	}
}

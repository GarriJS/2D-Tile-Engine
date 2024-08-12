using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Game.Drawing.Models
{
	/// <summary>
	/// Represents a sprite.
	/// </summary>
	public class Sprite : IDisposable
	{
		/// <summary>
		/// Gets or sets the draw data name.
		/// </summary>
		public string DrawDataName { get => this.SpritesheetName + this.SpritesheetBox; }

		/// <summary>
		/// Gets or sets the spritesheet name.
		/// </summary>
		public string SpritesheetName { get; set; }

		/// <summary>
		/// Gets or sets the spritesheet coordinate.
		/// </summary>
		public Point SpritesheetCoordinate { get; set; }

		/// <summary>
		/// Gets or sets the texture box.
		/// </summary>
		public Rectangle TextureBox { get; set; }

		/// <summary>
		/// Gets the spritesheet box.
		/// </summary>
		public Rectangle SpritesheetBox { get; set; }

		/// <summary>
		/// Gets or sets the texture.
		/// </summary>
		public Texture2D Texture { get; set; }

		/// <summary>
		/// Disposes of the draw data texture.
		/// </summary>
		public void Dispose()
		{
			this.Texture.Dispose();
		}
	}
}

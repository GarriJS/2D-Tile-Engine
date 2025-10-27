using Microsoft.Xna.Framework.Graphics;

namespace Engine.Graphics.Models.Contracts
{
	/// <summary>
	/// Represents a image.
	/// </summary>
	public interface IAmAImage : IAmAGraphic
	{
		/// <summary>
		/// Gets or sets the texture name.
		/// </summary>
		public string TextureName { get; }

		/// <summary>
		/// Gets or sets the texture.
		/// </summary>
		public Texture2D Texture { get; }

	}
}

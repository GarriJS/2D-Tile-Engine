using Engine.Drawing.Models.Contracts;
using Engine.Physics.Models;

namespace Engine.Drawing.Models
{
	/// <summary>
	/// Represents a independent image.
	/// </summary>
	public class IndependentImage : Image, IAmDrawable
	{
		/// <summary>
		/// Gets or sets the position.
		/// </summary>
		public Position Position { get; set; }

		/// <summary>
		/// Gets or sets the draw layer.
		/// </summary>
		public int DrawLayer { get; set; }

		/// <summary>
		/// Gets or sets the image.
		/// </summary>
		public Image Image { get => this; }
	}
}

using Engine.Drawing.Models;
using Engine.Drawing.Models.Contracts;
using Engine.Physics.Models;
using Engine.Physics.Models.Contracts;

namespace Engine.UI.Models
{
	/// <summary>
	/// Represents a user interface element.
	/// </summary>
	public class UserInterfaceElement : IAmDrawable, IHaveArea
	{
		/// <summary>
		/// Gets or sets the user interface element name.
		/// </summary>
		public string UserInterfaceElementName { get; set; }

		/// <summary>
		/// Gets or sets the draw layer.
		/// </summary>
		public int DrawLayer { get; set; }

		/// <summary>
		/// Gets or sets the sprite.
		/// </summary>
		public Sprite Sprite { get; set; }

		/// <summary>
		/// Gets the position.
		/// </summary>
		public Position Position { get => this.Area.Position; }

		/// <summary>
		/// Gets or sets the area.
		/// </summary>
		public IAmAArea Area { get; set; }

		/// <summary>
		/// Disposes of the user interface element.
		/// </summary>
		public void Dispose()
		{
			this.Sprite.Dispose();
		}
	}
}

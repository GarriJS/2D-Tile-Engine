using Engine.Drawing.Models;
using Engine.Drawing.Models.Contracts;
using Engine.Physics.Models;

namespace Engine.UserInterface.Models
{
	/// <summary>
	/// Represents a cursor.
	/// </summary>
	public class Cursor : IAmAnimated
	{
		/// <summary>
		/// Gets or sets the position.
		/// </summary>
		public Position Position { get; set; }

		/// <summary>
		/// Gets the sprite.
		/// </summary>
		public Sprite Sprite { get => Animation.CurrentFrame; }

		/// <summary>
		/// Gets or sets the animation.
		/// </summary>
		public Animation Animation { get; set; }

		/// <summary>
		/// Disposes of the cursor. 
		/// </summary>
		public void Dispose()
		{ 
			this.Animation?.Dispose();
		}
	}
}

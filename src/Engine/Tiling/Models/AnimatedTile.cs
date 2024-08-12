using Engine.Physics.Models.Contracts;
using Engine.Tiling.Models.Contracts;
using Game.Drawing.Models;
using Game.Drawing.Models.Contracts;
using Game.Physics.Models;

namespace Engine.Tiling.Models
{
	/// <summary>
	/// Represents a animated tile.
	/// </summary>
	public class AnimatedTile : IAmATile, IAmAnimated
	{
		/// <summary>
		/// Gets or sets the row.
		/// </summary>
		public int Row { get; set; }

		/// <summary>
		/// Gets or sets the columns.
		/// </summary>
		public int Column { get; set; }

		/// <summary>
		/// Get the sprite.
		/// </summary>
		public Sprite Sprite { get => Animation.CurrentFrame; }

		/// <summary>
		/// Gets or sets the animation.
		/// </summary>
		public Animation Animation { get; set; }

		/// <summary>
		/// Gets the position.
		/// </summary>
		public Position Position { get => this.Area.Position; }

		/// <summary>
		/// Gets or sets the area.
		/// </summary>
		public IAmAArea Area { get; set; }

		/// <summary>
		/// Disposes of the animated tile.
		/// </summary>
		public void Dispose()
		{
			this.Animation.Dispose();
		}
	}
}

using Common.Tiling.Models.Contracts;
using Engine.Graphics.Models;
using Engine.Graphics.Models.Contracts;
using Engine.Physics.Models;
using Engine.Physics.Models.Contracts;
using Microsoft.Xna.Framework;

namespace Common.Tiling.Models
{
	/// <summary>
	/// Represents a animated tile.
	/// </summary>
	public class AnimatedTile : IAmATile, IHaveAnAnimation
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
		/// Gets or sets the draw layer.
		/// </summary>
		public int DrawLayer { get; set; }

		/// <summary>
		/// Gets or sets the update order.
		/// </summary>
		public int UpdateOrder { get; set; }

		/// <summary>
		/// Gets the graphic.
		/// </summary>
		public IAmAGraphic Graphic { get => this.Image; }

		/// <summary>
		/// Get the image.
		/// </summary>
		public Image Image { get => this.Animation.CurrentFrame; }

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
		/// Draws the drawable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		public void Draw(GameTime gameTime, GameServiceContainer gameServices)
		{

		}

		/// <summary>
		/// Updates the updateable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="updateable">The updateable.</param>
		public void Update(GameTime gameTime, GameServiceContainer gameServices)
		{

		}

		/// <summary>
		/// Disposes of the animated tile.
		/// </summary>
		public void Dispose()
		{
			this.Animation?.Dispose();
		}
	}
}

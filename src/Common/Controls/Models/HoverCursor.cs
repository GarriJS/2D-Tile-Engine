using Engine.Drawables.Models;
using Engine.Drawables.Models.Contracts;
using Engine.Physics.Models;
using Engine.RunTime.Models.Contracts;
using Engine.RunTime.Services.Contracts;
using Microsoft.Xna.Framework;

namespace Common.Controls.Models
{
	/// <summary>
	/// Represents a hover cursor.
	/// </summary>
	public class HoverCursor : Image, IAmSubDrawable, IAmSubUpdateable
	{
		/// <summary>
		/// A value describing if the cursor is active or not.
		/// </summary>
		public bool IsActive { get; set; }

		/// <summary>
		/// Gets or sets the cursor name.
		/// </summary>
		public string CursorName { get; set; }

		/// <summary>
		/// Gets or sets the offset.
		/// </summary>
		public Vector2 Offset { get; set; }

		/// <summary>
		/// Gets or sets the image.
		/// </summary>
		public Image Image { get => this; }

		/// <summary>
		/// Draws the sub drawable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		/// <param name="position">The position.</param>
		/// <param name="offset">The offset.</param>
		public void Draw(GameTime gameTime, GameServiceContainer gameServices, Position position, Vector2 offset = default)
		{
			var drawingService = gameServices.GetService<IDrawingService>();

			if (false == this.IsActive)
			{
				return;
			}

			drawingService.Draw(gameTime, this, position, this.Offset + offset);
		}

		/// <summary>
		/// Updates the updateable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		public void Update(GameTime gameTime, GameServiceContainer gameServices)
		{

		}
	}
}

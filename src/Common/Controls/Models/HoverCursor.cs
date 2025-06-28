using Engine.Drawables.Models;
using Engine.Drawables.Models.Contracts;
using Engine.Physics.Models;
using Engine.RunTime.Services.Contracts;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Common.Controls.Models
{
	/// <summary>
	/// Represents a hover cursor.
	/// </summary>
	public class HoverCursor : Image, IAmSubDrawable
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
		/// Gets or sets the trailing cursors.
		/// </summary>
		public IList<TrailingCursor> TrailingCursors { get; set; }

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
			
			if (true != this.TrailingCursors?.Any())
			{
				return;
			}

			foreach (var trailingCursor in this.TrailingCursors)
			{
				trailingCursor.Draw(gameTime, gameServices, position, trailingCursor.Offset);
			}
		}

		/// <summary>
		/// Disposes of the draw data texture.
		/// </summary>
		new public void Dispose()
		{
			this.Image?.Dispose();

			if (true != this.TrailingCursors?.Any())
			{
				return;
			}

			foreach (var trailingCursor in this.TrailingCursors)
			{ 
				trailingCursor.Dispose();
			}
		}
	}
}

using Common.Controls.Services.Contracts;
using Engine.Drawables.Models;
using Engine.Drawables.Models.Contracts;
using Engine.Physics.Models;
using Engine.RunTime.Models.Contracts;
using Engine.RunTime.Services.Contracts;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Common.Controls.Models
{
	/// <summary>
	/// Represents a cursor.
	/// </summary>
	public class Cursor : Image, IAmDrawable, IAmUpdateable
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

		/// <summary>
		/// Gets or sets the trailing cursors.
		/// </summary>
		public IList<TrailingCursor> TrailingCursors { get; set; }

		/// <summary>
		/// Draws the drawable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		public void Draw(GameTime gameTime, GameServiceContainer gameServices)
		{
			var drawingService = gameServices.GetService<IDrawingService>();

			if (true == this.TrailingCursors?.Any())
			{
				foreach (var trailingCursor in this.TrailingCursors)
				{
					trailingCursor.Draw(gameTime, gameServices, this.Position, trailingCursor.Offset);
				}
			}

			drawingService.Draw(gameTime, this);
		}

		/// <summary>
		/// Updates the updateable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		public void Update(GameTime gameTime, GameServiceContainer gameServices)
		{
			var cursorService = gameServices.GetService<ICursorService>();

			cursorService.UpdateCursorPosition(this);
		}
	}
}

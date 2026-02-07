using Engine.Graphics.Models.Contracts;
using Engine.Physics.Models;
using Engine.RunTime.Models.Contracts;
using Microsoft.Xna.Framework;
using System;

namespace Common.Controls.Cursors.Models
{
	/// <summary>
	/// Represents a cursor.
	/// </summary>
	sealed public class Cursor : IHaveAGraphic, IAmDrawable, IAmSubUpdateable
    {
		/// <summary>
		/// Gets or sets the draw layer.
		/// </summary>
		required public int DrawLayer { get; set; }

		/// <summary>
		/// Gets or sets the update order.
		/// </summary>
		required public int UpdateOrder { get; set; }

		/// <summary>
		/// Gets or sets the cursor name.
		/// </summary>
		required public string CursorName { get; set; }

        /// <summary>
        /// Gets or sets the offset.
        /// </summary>
        required public Vector2 Offset { get; set; }

		/// <summary>
		/// Gets or sets the position.
		/// </summary>
		required public Position Position { get; set; }

		/// <summary>
		/// Gets the SimpleText.
		/// </summary>
		required public IAmAGraphic Graphic { get; set; }

        /// <summary>
        /// Gets or sets the cursor updater.
        /// </summary>
        required public Action<Cursor, GameTime> CursorUpdater { get; set; }

		/// <summary>
		/// Draws the sub drawable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>\
		public void Draw(GameTime gameTime, GameServiceContainer gameServices)
		{
			this.Graphic.Draw(gameTime, gameServices, this.Position.Coordinates, Color.White, this.Offset);
		}

        /// <summary>
        /// Updates the updateable.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        /// <param name="gameServices">The game services.</param>
        public void Update(GameTime gameTime, GameServiceContainer gameServices)
        {
			this.CursorUpdater?.Invoke(this, gameTime);
        }

        /// <summary>
        /// Disposes of the draw data texture.
        /// </summary>
        public void Dispose()
        {
            this.Graphic.Dispose();
        }
    }
}

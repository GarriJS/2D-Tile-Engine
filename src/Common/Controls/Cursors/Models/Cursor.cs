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
	public class Cursor : IHaveAGraphic, IAmDrawable, IAmSubUpdateable
    {
		/// <summary>
		/// Gets or sets the draw layer.
		/// </summary>
		public int DrawLayer { get; set; }

		/// <summary>
		/// Gets or sets the update order.
		/// </summary>
		public int UpdateOrder { get; set; }

		/// <summary>
		/// Gets or sets the cursor name.
		/// </summary>
		public string CursorName { get; set; }

        /// <summary>
        /// Gets or sets the offset.
        /// </summary>
        public Vector2 Offset { get; set; }

		/// <summary>
		/// Gets or sets the position.
		/// </summary>
		public Position Position { get; set; }

		/// <summary>
		/// Gets the SimpleText.
		/// </summary>
		public IAmAGraphic Graphic { get; set; }

        /// <summary>
        /// Gets or sets the cursor updater.
        /// </summary>
        public Action<Cursor, GameTime> CursorUpdater { get; set; }

		/// <summary>
		/// Draws the sub drawable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>\
		public void Draw(GameTime gameTime, GameServiceContainer gameServices)
		{
			this.Graphic.Draw(gameTime, gameServices, this.Position, Color.White, this.Offset);
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

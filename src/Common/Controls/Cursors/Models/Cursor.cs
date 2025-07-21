using Engine.Controls.Services;
using Engine.Graphics.Models;
using Engine.Graphics.Models.Contracts;
using Engine.Physics.Models;
using Engine.RunTime.Models.Contracts;
using Engine.RunTime.Services.Contracts;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Controls.Cursors.Models
{
    /// <summary>
    /// Represents a cursor.
    /// </summary>
    public class Cursor : Image, IAmDrawable, IHaveAnImage, IAmSubUpdateable
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
        /// Gets or sets the draw layer.
        /// </summary>
        public int DrawLayer { get; set; }

        /// <summary>
        /// Gets or sets the update order.
        /// </summary>
        public int UpdateOrder { get; set; }

        /// <summary>
        /// Gets or sets the offset.
        /// </summary>
        public Vector2 Offset { get; set; }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        public Position Position { get; set; }

        /// <summary>
        /// Gets the graphic.
        /// </summary>
        public Image Image { get => this; }

        /// <summary>
        /// Gets or sets the hover cursor.
        /// </summary>
        public HoverCursor HoverCursor { get; set; }

        /// <summary>
        /// Gets or sets the cursor updater.
        /// </summary>
        public Action<Cursor, GameTime> CursorUpdater { get; set; }

        /// <summary>
        /// Gets or sets the trailing cursors.
        /// </summary>
        public IList<TrailingCursor> TrailingCursors { get; set; }

        /// <summary>
        /// Initializes a new cursor.
        /// </summary>
        public Cursor()
        {
            ControlManager.ControlStateUpdated += Update;
        }

        /// <summary>
        /// Draws the drawable.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        /// <param name="gameServices">The game services.</param>
        public void Draw(GameTime gameTime, GameServiceContainer gameServices)
        {
            var drawingService = gameServices.GetService<IDrawingService>();

            if (false == IsActive)
            {
                return;
            }

            if (true == HoverCursor?.IsActive)
            {
                HoverCursor.Draw(gameTime, gameServices, Position);
                HoverCursor.IsActive = false;

                return;
            }

            drawingService.Draw(gameTime, Graphic, Position, Offset);

            if (true != TrailingCursors?.Any())
            {
                return;
            }

            foreach (var trailingCursor in TrailingCursors)
            {
                trailingCursor.Draw(gameTime, gameServices, Position, trailingCursor.Offset);
            }
        }

        /// <summary>
        /// Updates the updateable.
        /// </summary>
        /// <param name="gameTime">The game time.</param>
        /// <param name="gameServices">The game services.</param>
        public void Update(GameTime gameTime, GameServiceContainer gameServices)
        {
            if (false == IsActive)
            {
                return;
            }

            CursorUpdater?.Invoke(this, gameTime);

            if (true != TrailingCursors?.Any())
            {
                return;
            }

            foreach (var trailingCursor in TrailingCursors)
            {
                trailingCursor.CursorUpdater?.Invoke(this, trailingCursor, gameTime);
            }
        }

        /// <summary>
        /// Disposes of the draw data texture.
        /// </summary>
        new public void Dispose()
        {
            ControlManager.ControlStateUpdated -= Update;
            Texture?.Dispose();

            if (true != TrailingCursors?.Any())
            {
                return;
            }

            foreach (var trailingCursor in TrailingCursors)
            {
                trailingCursor.Dispose();
            }
        }
    }
}

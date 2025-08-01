using Common.Controls.Cursors.Models;
using Engine.Controls.Models;
using Engine.Core.Contracts;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Common.Controls.Cursors.Services.Contracts
{
    /// <summary>
    /// Represents a cursors service.
    /// </summary>
    public interface ICursorService : ILoadContent
    {
        /// <summary>
        /// Gets the active cursor.
        /// </summary>
        public Cursor ActiveCursor { get; }

        /// <summary>
        /// Gets the cursors.
        /// </summary>
        public Dictionary<string, Cursor> Cursors { get; }

        /// <summary>
        /// Sets the active cursor.
        /// </summary>
        /// <param name="cursor"></param>
        public void SetActiveCursor(Cursor cursor);

        /// <summary>
        /// Disables all cursors.
        /// </summary>
        public void DisableAllCursors();

        /// <summary>
        /// Process the cursor and control state.
        /// </summary>
        /// <param name="cursor">The cursor.</param>
        /// <param name="controlState">The control state.</param>
        /// <param name="priorControlState">The prior control state.</param>
        public void ProcessCursorControlState(Cursor cursor, ControlState controlState, ControlState priorControlState);

        /// <summary>
        /// Updates the cursor.
        /// </summary>
        /// <param name="cursor">The cursor.</param>
        /// <param name="gameTime">The game time.</param>
        public void BasicCursorUpdater(Cursor cursor, GameTime gameTime);
    }
}

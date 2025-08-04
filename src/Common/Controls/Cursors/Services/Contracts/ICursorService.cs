using Common.Controls.CursorInteraction.Models.Contracts;
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
        public Cursor PrimaryCursor { get; }

		/// <summary>
		/// Gets or sets the primary hover cursor.
		/// </summary>
		public Cursor PrimaryHoverCursor { get; }

		/// <summary>
		/// Gets the secondary cursors.
		/// </summary>
		public List<Cursor> SecondaryCursors { get; }

		/// <summary>
		/// Gets the secondary cursors.
		/// </summary>
		public List<Cursor> SecondaryHoverCursors { get; }

		/// <summary>
		/// Gets the cursors.
		/// </summary>
		public Dictionary<string, Cursor> Cursors { get; }

        /// <summary>
        /// Sets the primary cursor.
        /// </summary>
        /// <param name="cursor"></param>
        public void SetPrimaryCursor(Cursor cursor);

        /// <summary>
        /// Adds the secondary cursors.
        /// </summary>
        /// <param name="cursor">The cursor.</param>
        public void AddSecondaryCursor(Cursor cursor);

        /// <summary>
        /// Sets the primary hover cursor.
        /// </summary>
        /// <param name="cursor">The cursor.</param>
        public void SetPrimaryHoverCursor(Cursor cursor);

        /// <summary>
        /// Adds the secondary cursors.
        /// </summary>
        /// <param name="cursor">The cursor.</param>
        public void AddSecondaryHoverCursor(Cursor cursor);

		/// <summary>
		/// Disables all non hover cursors.
		/// </summary>
		public void DisableAllNonHoverCursors();

        /// <summary>
        /// Disables all hover cursors.
        /// </summary>
        public void DisableAllHoverCursors();

        /// <summary>
        /// Clears the hover cursors.
        /// </summary>
        public void ClearHoverCursors();

		/// <summary>
		/// Process the cursor and control state.
		/// </summary>
		/// <param name="cursor">The cursor.</param>
		/// <param name="controlState">The control state.</param>
		/// <param name="priorControlState">The prior control state.</param>
		/// <returns>The object the cursor is hovering.</returns>
		public IHaveAHoverConfiguration ProcessCursorControlState(Cursor cursor, ControlState controlState, ControlState priorControlState);

        /// <summary>
        /// Updates the cursor.
        /// </summary>
        /// <param name="cursor">The cursor.</param>
        /// <param name="gameTime">The game time.</param>
        public void BasicCursorUpdater(Cursor cursor, GameTime gameTime);
    }
}

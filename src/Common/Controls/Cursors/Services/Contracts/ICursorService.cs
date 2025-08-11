using Common.Controls.CursorInteraction.Models.Contracts;
using Common.Controls.Cursors.Models;
using Common.DiskModels.Controls;
using Engine.Controls.Models;
using Engine.Core.Contracts;
using Engine.Physics.Models;
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
		/// Gets the cursor position.
		/// </summary>
		public Position CursorPosition { get; }

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
		/// Gets the cursor.
		/// </summary>
		/// <param name="cursorModel">The cursor model.</param>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <param name="addCursor">A value indicating whether to add the cursors.</param>
		/// <returns>The cursor.</returns>
		public Cursor GetCursor(CursorModel cursorModel, int width, int height, bool addCursor = false);

		/// <summary>
		/// Sets the primary cursor.
		/// </summary>
		/// <param name="cursor"></param>
		public void SetPrimaryCursor(Cursor cursor);

		/// <summary>
		/// Adds the secondary cursors.
		/// </summary>
		/// <param name="cursor">The cursor.</param>
		/// <param name="disableExisting">A value indicating whether to disable existing secondary hover cursors.</param>
		public void AddSecondaryCursor(Cursor cursor, bool disableExisting);

		/// <summary>
		/// Sets the primary hover cursor.
		/// </summary>
		/// <param name="cursor">The cursor.</param>
		public void SetPrimaryHoverCursor(Cursor cursor);

		/// <summary>
		/// Adds the secondary cursors.
		/// </summary>
		/// <param name="cursor">The cursor.</param>
		/// <param name="disableExisting">A value indicating whether to disable existing secondary hover cursors.</param>
		public void AddSecondaryHoverCursor(Cursor cursor, bool disableExisting);

		/// <summary>
		/// Disables all non hover cursors.
		/// </summary>
		public void DisableAllNonHoverCursors();

		/// <summary>
		/// Disables all hover cursors.
		/// </summary>
		/// <param name="disableSecondaryHoverCursors">A value indicating whether to disable secondary hover cursors.</param>
		public void DisableAllHoverCursors(bool disableSecondaryHoverCursors);

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

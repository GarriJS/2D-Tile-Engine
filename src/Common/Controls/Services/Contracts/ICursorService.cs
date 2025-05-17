using Common.Controls.Models;
using Engine.Core.Contracts;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Common.Controls.Services.Contracts
{
	/// <summary>
	/// Represents a cursors service.
	/// </summary>
	public interface ICursorService : ILoadContent
    {
		/// <summary>
		/// Gets the cursors.
		/// </summary>
		public Dictionary<string, Cursor> Cursors { get; }

		/// <summary>
		/// Updates the cursor position.
		/// </summary>
		/// <param name="cursor">The cursor.</param>
		/// <param name="gameTime">The game time.</param>
		public void UpdateCursorPosition(Cursor cursor, GameTime gameTime);

		/// <summary>
		/// Adds a trailing cursor.
		/// </summary>
		/// <param name="primaryCursor">The primary cursor to add the trailing cursor to.</param>
		/// <param name="trailingCursor">The trailing cursor.</param>
		public void AddTrailingCursor(Cursor primaryCursor, TrailingCursor trailingCursor);
	}
}

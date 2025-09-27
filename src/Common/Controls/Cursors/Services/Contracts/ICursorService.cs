using Common.Controls.CursorInteraction.Models;
using Common.Controls.Cursors.Models;
using Common.DiskModels.Controls;
using Engine.Core.Contracts;
using System.Collections.Generic;

namespace Common.Controls.Cursors.Services.Contracts
{
	/// <summary>
	/// Represents a cursors service.
	/// </summary>
	public interface ICursorService : ILoadContent
    {
		/// <summary>
		/// Gets or sets the cursor state monitor.
		/// </summary>
		public CursorState CursorState { get; set; }

		/// <summary>
		/// Gets the cursors.
		/// </summary>
		public Dictionary<string, Cursor> Cursors { get; }

		/// <summary>
		/// Gets the cursor.
		/// </summary>
		/// <param name="cursorModel">The cursor model.</param>
		/// <param name="addCursor">A value indicating whether to add the cursors.</param>
		/// <param name="drawLayerOffset">The draw layer offset.</param>
		/// <returns>The cursor.</returns>
		public Cursor GetCursor(CursorModel cursorModel, bool addCursor = false, byte drawLayerOffset = 0);

		/// <summary>
		/// Gets the cursor hover state.
		/// </summary>
		/// <param name="cursor">The cursor.</param>
		/// <returns>The hover state.</returns>
		public HoverState GetCursorHoverState(Cursor cursor);

	}
}

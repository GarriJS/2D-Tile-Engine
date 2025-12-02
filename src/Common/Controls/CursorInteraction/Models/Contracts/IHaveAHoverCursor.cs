using Common.Controls.Cursors.Models;
using System;

namespace Common.Controls.CursorInteraction.Models.Contracts
{
	/// <summary>
	/// Represents something with a hover configuration.
	/// </summary>
	public interface IHaveAHoverCursor : IDisposable
	{
		/// <summary>
		/// Gets or sets the hover cursor.
		/// </summary>
		public Cursor HoverCursor { get; set; }
	}
}

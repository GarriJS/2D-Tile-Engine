using Common.Controls.Cursors.Models;
using System;

namespace Common.Controls.CursorInteraction.Models.Abstract
{
	/// <summary>
	/// Represents a base hover configuration.
	/// </summary>
	abstract public class BaseHoverConfiguration : InteractionConfigurationBase, IDisposable
	{


		/// <summary>
		/// Gets or sets the hover cursor.
		/// </summary>
		public Cursor HoverCursor { get; set; }

		/// <summary>
		/// Disposes of the hover configuration.
		/// </summary>
		public void Dispose()
		{
			this.HoverCursor.Dispose();
		}
	}
}

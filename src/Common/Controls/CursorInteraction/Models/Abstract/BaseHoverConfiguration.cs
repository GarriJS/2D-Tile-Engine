using Common.Controls.Cursors.Models;
using Engine.Physics.Models.Contracts;
using Engine.Physics.Models.SubAreas;
using Microsoft.Xna.Framework;
using System;

namespace Common.Controls.CursorInteraction.Models.Abstract
{
	/// <summary>
	/// Represents a base hover configuration.
	/// </summary>
	public abstract class BaseHoverConfiguration : IHaveASubArea, IDisposable
	{
		/// <summary>
		/// Gets or sets the area.
		/// </summary>
		public SubArea Area { get; set; }

		/// <summary>
		/// Gets or sets the offset;
		/// </summary>
		public Vector2 Offset { get; set; }

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

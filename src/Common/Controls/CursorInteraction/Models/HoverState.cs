using Common.Controls.CursorInteraction.Models.Abstract;
using Common.Controls.CursorInteraction.Models.Contracts;
using Common.Controls.Cursors.Models;
using Common.UserInterface.Models.Contracts;
using Engine.Physics.Models;
using System;
using System.Collections.Generic;

namespace Common.Controls.CursorInteraction.Models
{
	/// <summary>
	/// Represents a hover state.
	/// </summary>
	sealed public class HoverState
	{
		/// <summary>
		/// Gets or sets the top cursor configuration.
		/// </summary>
		public BaseCursorConfiguration TopCursorConfiguration { get; set; }

		/// <summary>
		/// Gets or sets the top hover cursor.
		/// </summary>
		public Cursor TopHoverCursor { get; set; }

		/// <summary>
		/// Gets or sets the bottom scrollable.
		/// </summary>
		public IAmScrollable BottomScrollable { get; set; }

		/// <summary>
		/// Gets or sets the hover object location.
		/// </summary>
		public Vector2Extender<IHaveACursorConfiguration>? HoverObjectLocation { get; set; }

		/// <summary>
		/// The hovered object.
		/// </summary>
		readonly public Dictionary<Type, object> _hoveredObjects = [];
	}
}

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
	public class HoverState
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
		public LocationExtender<IHaveACursorConfiguration> HoverObjectLocation { get; set; }

		/// <summary>
		/// Get or sets the hovered object.
		/// </summary>
		public Dictionary<Type, object> HoveredObjects { get; set; }
	}
}

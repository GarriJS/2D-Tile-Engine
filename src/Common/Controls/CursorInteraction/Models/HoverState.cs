using Common.Controls.CursorInteraction.Models.Abstract;
using Common.Controls.CursorInteraction.Models.Contracts;
using Common.Controls.Cursors.Models;
using Engine.Physics.Models;

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
		/// Gets or sets the hover object location.
		/// </summary>
		public LocationExtender<IHaveACursorConfiguration> HoverObjectLocation { get; set; }
	}
}

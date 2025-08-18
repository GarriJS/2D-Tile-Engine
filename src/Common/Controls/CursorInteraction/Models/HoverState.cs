using Common.Controls.CursorInteraction.Models.Abstract;
using Common.Controls.CursorInteraction.Models.Contracts;
using Engine.Physics.Models;

namespace Common.Controls.CursorInteraction.Models
{
	/// <summary>
	/// Represents a hover state.
	/// </summary>
	public class HoverState
	{
		/// <summary>
		/// Gets or sets the hover cursor configuration.
		/// </summary>
		public BaseHoverConfiguration TopHoverCursorConfiguration { get; set; }

		/// <summary>
		/// Gets or sets the hover object location.
		/// </summary>
		public LocationExtender<IHaveAHoverConfiguration> HoverObjectLocation { get; set; }
	}
}

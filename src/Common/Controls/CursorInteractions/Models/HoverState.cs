using Common.UserInterface.Models;
using Microsoft.Xna.Framework;

namespace Common.Controls.CursorInteractions.Models
{
	/// <summary>
	/// Represents a hover state.
	/// </summary>
	sealed public class HoverState
	{
        /// <summary>
        /// Gets or Initializes the hover location.
        /// </summary>
        public Vector2 HoverLocation { get; init; } 

        /// <summary>
        /// Gets or Initializes the user interface location descent.
        /// </summary>
        public UiLocationDescent UiLocationDescent { get; init; }
    }
}

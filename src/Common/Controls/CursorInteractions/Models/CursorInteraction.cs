using Common.Controls.CursorInteractions.Models.Contracts;
using Common.UserInterface.Models.Contracts;
using Microsoft.Xna.Framework;

namespace Common.Controls.CursorInteractions.Models
{
	/// <summary>
	/// Represents a cursor interaction.
	/// </summary>
	sealed public class CursorInteraction
	{
        /// <summary>
        /// Gets or initializes the cursor location.
        /// </summary>
        required public Vector2 CursorLocation { get; init; }

        /// <summary>
        /// Gets or initializes the Subject location.
        /// </summary>
        required public Vector2 SubjectLocation { get; init; }

        /// <summary>
        /// Gets or initializes the subject.
        /// </summary>
        required public IHaveACursorConfiguration Subject { get; init; }

        /// <summary>
        /// Gets or initializes the subject user interface parent.
        /// </summary>
        required public IAmAUiParent SubjectUiParent { get; init; }
    }
}

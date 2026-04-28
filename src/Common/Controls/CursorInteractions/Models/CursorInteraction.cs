using Common.Controls.CursorInteractions.Models.Contracts;
using Microsoft.Xna.Framework;

namespace Common.Controls.CursorInteractions.Models
{
	/// <summary>
	/// Represents a cursor interaction.
	/// </summary>
	sealed public class CursorInteraction
	{
		/// <summary>
		/// Gets or sets the cursor location.
		/// </summary>
		required public Vector2 CursorLocation { get; init; }

		/// <summary>
		/// Gets or sets the Subject location.
		/// </summary>
		required public Vector2 SubjectLocation { get; init; }

        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        required public IHaveACursorConfiguration Subject { get; init; }
	}
}

using Microsoft.Xna.Framework;

namespace Common.Controls.CursorInteraction.Models
{
	/// <summary>
	/// Represents a cursor interaction.
	/// </summary>
	/// <typeparam name="T">The type that generated the cursor interaction.</typeparam>
	sealed public class CursorInteraction<T>
	{
		/// <summary>
		/// Gets or sets the cursor location.
		/// </summary>
		required public Vector2 CursorLocation { get; set; }

		/// <summary>
		/// Gets or sets the Subject location.
		/// </summary>
		required public Vector2 SubjectLocation { get; set; }

		/// <summary>
		/// Gets or sets the subject.
		/// </summary>
		required public T Subject { get; set; }
	}
}

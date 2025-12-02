using Microsoft.Xna.Framework;

namespace Common.Controls.CursorInteraction.Models
{
	/// <summary>
	/// Represents a cursor interaction.
	/// </summary>
	/// <typeparam name="T">The type that generated the cursor interaction.</typeparam>
	public class CursorInteraction<T>
	{
		/// <summary>
		/// Gets or sets the cursor location.
		/// </summary>
		public Vector2 CursorLocation { get; set; }

		/// <summary>
		/// Gets or sets the element location.
		/// </summary>
		public Vector2 ElementLocation { get; set; }

		/// <summary>
		/// Gets or sets the element.
		/// </summary>
		public T Element { get; set; }
	}
}

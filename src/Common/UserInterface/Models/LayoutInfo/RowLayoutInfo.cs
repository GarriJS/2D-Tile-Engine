using Microsoft.Xna.Framework;

namespace Common.UserInterface.Models.LayoutInfo
{
	/// <summary>
	/// Represents a row layout info.
	/// </summary>
	public class RowLayoutInfo
	{
		/// <summary>
		/// Gets or sets the row.
		/// </summary>
		public UiRow Row { get; init; }

		/// <summary>
		/// Gets or sets the location of the row.
		/// </summary>
		public Vector2 Offset { get; init; }
	}
}

using Microsoft.Xna.Framework;

namespace Common.UserInterface.Models.LayoutInfo
{
	/// <summary>
	/// Represents a block layout info.
	/// </summary>
	public class BlockLayoutInfo
	{
		/// <summary>
		/// Gets or sets the block.
		/// </summary>
		public UiBlock Block { get; init; }

		/// <summary>
		/// Gets or sets the location of the block.
		/// </summary>
		public Vector2 Offset { get; init; }
	}
}

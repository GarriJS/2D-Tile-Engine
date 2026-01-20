using Engine.RunTime.Models.Contracts;
using Microsoft.Xna.Framework;

namespace Common.UserInterface.Models.Contracts
{
	/// <summary>
	/// Represents a user interface zone child.
	/// </summary>
	public interface IAmAUiZoneChild : IAmSubDrawable
	{
		/// <summary>
		/// Gets or sets the cached offset.
		/// </summary>
		public Vector2? CachedOffset { get; set; }

		/// <summary>
		/// Gets the total width.
		/// </summary>
		public float TotalWidth { get; }

		/// <summary>
		/// Gets the total height.
		/// </summary>
		public float TotalHeight { get; }

		/// <summary>
		/// Get the inside width.
		/// </summary>
		public float InsideWidth { get; }

		/// <summary>
		/// Gets the inside height.
		/// </summary>
		public float InsideHeight { get; }

		/// <summary>
		/// Gets or sets the user interface margin
		/// </summary>
		public UiMargin Margin { get; set; }

		/// <summary>
		/// Updates the offsets.
		/// </summary>
		public void UpdateOffsets();

		/// <summary>
		/// Disposes of the user interface block.
		/// </summary>
		public void Dispose();
	}
}


using Microsoft.Xna.Framework;

namespace Engine.UI.Models.Constants
{
	/// <summary>
	/// Constants for element size scalars.
	/// </summary>
	public static class ElementSizesScalars
	{
		/// <summary>
		/// The none element size scalars.
		/// </summary>
		public static Vector2 None { get; } = new Vector2(0, 0);

		/// <summary>
		/// The extra small element size scalars.
		/// </summary>
		public static Vector2 ExtraSmall { get; } = new Vector2(1f / 5f, 1f / 6f);

		/// <summary>
		/// The small element size scalars.
		/// </summary>
		public static Vector2 Small { get; } = new Vector2(1f / 4f, 1f / 5f);

		/// <summary>
		/// The medium element size scalars.
		/// </summary>
		public static Vector2 Medium { get; } = new Vector2(1f / 3f, 1f / 4f);

		/// <summary>
		/// The large element size scalars.
		/// </summary>
		public static Vector2 Large { get; } = new Vector2(1f / 2f, 1f / 3f);

		/// <summary>
		/// The extra large element size scalars.
		/// </summary>
		public static Vector2 ExtraLarge { get; } = new Vector2(1f, 1f / 2f);

		/// <summary>
		/// The full element size scalars.
		/// </summary>
		public static Vector2 Full { get; } = new Vector2(1, 1);
	}
}

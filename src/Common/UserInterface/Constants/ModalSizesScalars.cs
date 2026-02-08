using Microsoft.Xna.Framework;

namespace Common.UserInterface.Constants
{
	/// <summary>
	/// Constants for modal size scalars.
	/// </summary>
	static public class ModalSizesScalars
    {
		/// <summary>
		/// The small modal size scalars.
		/// </summary>
		static public Vector2 Small { get; } = new Vector2(1f, 1f);

		/// <summary>
		/// The medium modal size scalars.
		/// </summary>
		static public Vector2 Medium { get; } = new Vector2(2f, 2f);

		/// <summary>
		/// The large modal size scalars.
		/// </summary>
		static public Vector2 Large { get; } = new Vector2(3f, 2f);

		/// <summary>
		/// The extra large modal size scalars.
		/// </summary>
		static public Vector2 ExtraLarge { get; } = new Vector2(4f, 2f);

        /// <summary>
        /// The full screen modal size scalars.
        /// </summary>
        static public Vector2 Fullscreen { get; } = new Vector2(4f, 3f);
    }
}

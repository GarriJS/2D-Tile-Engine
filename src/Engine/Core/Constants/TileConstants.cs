using Microsoft.Xna.Framework;

namespace Engine.Core.Constants
{
	/// <summary>
	/// Constants for tiles.
	/// </summary>
	public static class TileConstants
	{
		/// <summary>
		/// Gets the tile size.
		/// </summary>
		public static int TILE_SIZE { get; } = 32;

		/// <summary>
		/// Gets the tile area.
		/// </summary>
		public static Vector2 TILE_AREA { get; } = new Vector2
		{
			X = TILE_SIZE,
			Y = TILE_SIZE,
		};
	}
}

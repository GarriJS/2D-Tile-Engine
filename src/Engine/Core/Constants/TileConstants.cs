using Engine.Physics.Models.SubAreas;
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
		public static SubArea TILE_AREA { get; } = new SubArea
		{
			Width = TILE_SIZE,
			Height = TILE_SIZE,
		};
	}
}

using Engine.Physics.Models.SubAreas;

namespace Engine.Core.Constants
{
	/// <summary>
	/// Constants for tiles.
	/// </summary>
	static public class TileConstants
	{
		/// <summary>
		/// Gets the tile size.
		/// </summary>
		static public int TILE_SIZE { get; } = 32;

		/// <summary>
		/// Gets the tile area.
		/// </summary>
		static public SubArea TILE_AREA { get; } = new SubArea
		{
			Width = TILE_SIZE,
			Height = TILE_SIZE,
		};
	}
}

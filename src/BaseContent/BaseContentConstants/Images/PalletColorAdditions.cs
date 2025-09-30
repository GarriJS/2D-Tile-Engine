using Microsoft.Xna.Framework;

namespace BaseContent.BaseContentConstants.Images
{   
	/// <summary>
	/// Represents pallet colors additions.
	/// </summary>
	public static class PalletColorAdditions
	{
		/// <summary>
		/// Black
		/// </summary>
		public static Color Hex_000000 { get; } = new Color(0, 0, 0, 255);
		
		/// <summary>
		/// White
		/// </summary>
		public static Color Hex_FFFFFF { get; } = new Color(255, 255, 255, 255);

		/// <summary>
		/// Grey
		/// </summary>
		public static Color Hex_696969 { get; } = new Color(105, 105, 105, 255);

		/// <summary>
		/// Grey transparent
		/// </summary>
		public static Color Hex_404040_Transparent { get; } = new Color(64, 64, 64, 130);
	}
}

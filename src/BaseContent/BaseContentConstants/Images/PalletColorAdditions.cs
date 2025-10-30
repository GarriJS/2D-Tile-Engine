using Microsoft.Xna.Framework;

namespace BaseContent.BaseContentConstants.Images
{   
	/// <summary>
	/// Represents pallet colors additions.
	/// </summary>
	static public class PalletColorAdditions
	{
		/// <summary>
		/// Black
		/// </summary>
		static public Color Hex_000000 { get; } = new Color(0, 0, 0, 255);
		
		/// <summary>
		/// White
		/// </summary>
		static public Color Hex_FFFFFF { get; } = new Color(255, 255, 255, 255);

		/// <summary>
		/// Grey
		/// </summary>
		static public Color Hex_696969 { get; } = new Color(105, 105, 105, 255);

		/// <summary>
		/// Grey transparent
		/// </summary>
		static public Color Hex_404040_Transparent { get; } = new Color(64, 64, 64, 130);
	}
}

using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace BaseContent.BaseContentConstants.Images
{
	/// <summary>
	/// Represents a pallet color to texture box helper.
	/// </summary>
	static public class PalletColorToTextureBoxHelper
	{
		/// <summary>
		/// Tries to gets the pallet color texture.
		/// </summary>
		/// <param name="color">The color.</param>
		/// <param name="textureBox">The texture box.</param>
		/// <returns>A value indicating if a texture box for the color was found.</returns>
		static public bool TryGetPalletColorTextureBox(Color color, out Rectangle textureBox)
		{
			if (PalletColorTextureBoxMap.TryGetValue(color, out textureBox))
			{
				return true;
			}

			if (PalletColorAdditionsTextureBoxMap.TryGetValue(color, out textureBox))
			{
				return true;
			}

			textureBox = default;

			return false;
		}

		/// <summary>
		/// Gets the pallet color texture box.
		/// </summary>
		/// <param name="color">The color.</param>
		/// <returns>The pallet color texture box.</returns>
		static public Rectangle GetPalletColorTextureBox(Color color)
		{
			TryGetPalletColorTextureBox(color, out var textureBox);

			return textureBox;
		}

		/// <summary>
		/// The pallet color texture box maps.
		/// </summary>
		static private Dictionary<Color, Rectangle> PalletColorTextureBoxMap { get; } = new()
		{
			{ PalletColors.Hex_FF0040, new Rectangle(0, 0, 1, 1) },
			{ PalletColors.Hex_131313, new Rectangle(1, 0, 1, 1) },
			{ PalletColors.Hex_1B1B1B, new Rectangle(2, 0, 1, 1) },
			{ PalletColors.Hex_272727, new Rectangle(3, 0, 1, 1) },
			{ PalletColors.Hex_3D3D3D, new Rectangle(4, 0, 1, 1) },
			{ PalletColors.Hex_5D5D5D, new Rectangle(5, 0, 1, 1) },
			{ PalletColors.Hex_858585, new Rectangle(6, 0, 1, 1) },
			{ PalletColors.Hex_B4B4B4, new Rectangle(7, 0, 1, 1) },
			{ PalletColors.Hex_C7CFDD, new Rectangle(8, 0, 1, 1) },
			{ PalletColors.Hex_92A1B9, new Rectangle(9, 0, 1, 1) },
			{ PalletColors.Hex_657392, new Rectangle(10, 0, 1, 1) },
			{ PalletColors.Hex_424C6E, new Rectangle(10, 1, 1, 1) },
			{ PalletColors.Hex_2A2F4E, new Rectangle(12, 0, 1, 1) },
			{ PalletColors.Hex_1A1932, new Rectangle(13, 0, 1, 1) },
			{ PalletColors.Hex_0E071B, new Rectangle(14, 0, 1, 1) },
			{ PalletColors.Hex_1C121C, new Rectangle(15, 0, 1, 1) },
			{ PalletColors.Hex_391F21, new Rectangle(16, 0, 1, 1) },
			{ PalletColors.Hex_5D2C28, new Rectangle(17, 0, 1, 1) },
			{ PalletColors.Hex_8A4836, new Rectangle(18, 0, 1, 1) },
			{ PalletColors.Hex_BF6F4A, new Rectangle(19, 0, 1, 1) },
			{ PalletColors.Hex_E69C69, new Rectangle(20, 0, 1, 1) },
			{ PalletColors.Hex_F6CA9F, new Rectangle(20, 1, 1, 1) },
			{ PalletColors.Hex_F9E6CF, new Rectangle(22, 0, 1, 1) },
			{ PalletColors.Hex_EDAB50, new Rectangle(23, 0, 1, 1) },
			{ PalletColors.Hex_E07438, new Rectangle(24, 0, 1, 1) },
			{ PalletColors.Hex_C64524, new Rectangle(25, 0, 1, 1) },
			{ PalletColors.Hex_8E251D, new Rectangle(26, 0, 1, 1) },
			{ PalletColors.Hex_FF5000, new Rectangle(27, 0, 1, 1) },
			{ PalletColors.Hex_ED7614, new Rectangle(28, 0, 1, 1) },
			{ PalletColors.Hex_FFA214, new Rectangle(29, 0, 1, 1) },
			{ PalletColors.Hex_FFC825, new Rectangle(30, 0, 1, 1) },
			{ PalletColors.Hex_FFEB57, new Rectangle(30, 1, 1, 1) },
			{ PalletColors.Hex_D3FC7E, new Rectangle(32, 0, 1, 1) },
			{ PalletColors.Hex_99E65F, new Rectangle(33, 0, 1, 1) },
			{ PalletColors.Hex_5AC54F, new Rectangle(34, 0, 1, 1) },
			{ PalletColors.Hex_33984B, new Rectangle(35, 0, 1, 1) },
			{ PalletColors.Hex_1E6F50, new Rectangle(36, 0, 1, 1) },
			{ PalletColors.Hex_134C4C, new Rectangle(37, 0, 1, 1) },
			{ PalletColors.Hex_0C2E44, new Rectangle(38, 0, 1, 1) },
			{ PalletColors.Hex_00396D, new Rectangle(39, 0, 1, 1) },
			{ PalletColors.Hex_0069AA, new Rectangle(40, 0, 1, 1) },
			{ PalletColors.Hex_0098DC, new Rectangle(40, 1, 1, 1) },
			{ PalletColors.Hex_00CDF9, new Rectangle(42, 0, 1, 1) },
			{ PalletColors.Hex_0CF1FF, new Rectangle(43, 0, 1, 1) },
			{ PalletColors.Hex_94FDFF, new Rectangle(44, 0, 1, 1) },
			{ PalletColors.Hex_FDD2ED, new Rectangle(45, 0, 1, 1) },
			{ PalletColors.Hex_F389F5, new Rectangle(46, 0, 1, 1) },
			{ PalletColors.Hex_DB3FFD, new Rectangle(47, 0, 1, 1) },
			{ PalletColors.Hex_7A09FA, new Rectangle(48, 0, 1, 1) },
			{ PalletColors.Hex_3003D9, new Rectangle(49, 0, 1, 1) },
			{ PalletColors.Hex_0C0293, new Rectangle(50, 0, 1, 1) },
			{ PalletColors.Hex_03193F, new Rectangle(50, 1, 1, 1) },
			{ PalletColors.Hex_3B1443, new Rectangle(52, 0, 1, 1) },
			{ PalletColors.Hex_622461, new Rectangle(53, 0, 1, 1) },
			{ PalletColors.Hex_93388F, new Rectangle(54, 0, 1, 1) },
			{ PalletColors.Hex_CA52C9, new Rectangle(55, 0, 1, 1) },
			{ PalletColors.Hex_C85086, new Rectangle(56, 0, 1, 1) },
			{ PalletColors.Hex_F68187, new Rectangle(57, 0, 1, 1) },
			{ PalletColors.Hex_F5555D, new Rectangle(58, 0, 1, 1) },
			{ PalletColors.Hex_EA323C, new Rectangle(59, 0, 1, 1) },
			{ PalletColors.Hex_C42430, new Rectangle(60, 0, 1, 1) },
			{ PalletColors.Hex_891E2B, new Rectangle(60, 1, 1, 1) },
			{ PalletColors.Hex_571C27, new Rectangle(62, 0, 1, 1) }

		};

		/// <summary>
		/// The pallet color additions texture box maps.
		/// </summary>
		static private Dictionary<Color, Rectangle> PalletColorAdditionsTextureBoxMap { get; } = new()
		{
			{ PalletColorAdditions.Hex_000000, new Rectangle(0, 1, 1, 1) },
			{ PalletColorAdditions.Hex_FFFFFF, new Rectangle(1, 1, 1, 1) },
			{ PalletColorAdditions.Hex_696969, new Rectangle(2, 1, 1, 1) },
			{ PalletColorAdditions.Hex_404040_Transparent, new Rectangle(3, 1, 1, 1) }
		};
	}
}

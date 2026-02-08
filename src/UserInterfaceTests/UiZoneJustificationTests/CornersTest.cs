using BaseContent.BaseContentConstants.Images;
using Common.DiskModels.UserInterface;
using Common.UserInterface.Enums;
using Engine.DiskModels.Drawing;
using Engine.Graphics.Enum;

namespace UserInterfaceTests.UiZoneJustificationTests
{
	static public class CornersTest
	{

		static public UiGroupModel GetCornersTest1()
		{
			var zones = GetCornersTest1Zones();

			var result = new UiGroupModel
			{
				Name = "CornersTest1",
				VisibilityGroupId = 1,
				MakeVisible = true,
				Zones = zones
			};
		
			return result;
		}


		private static UiZoneModel[] GetCornersTest1Zones()
		{
			UiZoneModel[] result =
			[
				new UiZoneModel
				{
					Name = "TopLeft",
					ResizeTexture = true,
					VerticalJustificationType = UiVerticalJustificationType.Top,
					UiZonePositionType = UiZonePositionType.Row1Col1,
					BackgroundTexture = new SimpleImageModel
					{
						TextureName = "pallet",
						TextureRegion = new TextureRegionModel
						{
							TextureRegionType = TextureRegionType.Fill,
							TextureBox = PalletColorToTextureBoxHelper.GetPalletColorTextureBox(PalletColors.Hex_00396D)
						}
					}
				},
				new UiZoneModel
				{
					Name = "TopRight",
					ResizeTexture = true,
					VerticalJustificationType = UiVerticalJustificationType.Top,
					UiZonePositionType = UiZonePositionType.Row1Col4,
					BackgroundTexture = new SimpleImageModel
					{
						TextureName = "pallet",
						TextureRegion = new TextureRegionModel
						{
							TextureRegionType = TextureRegionType.Fill,
							TextureBox = PalletColorToTextureBoxHelper.GetPalletColorTextureBox(PalletColors.Hex_00396D)
						}
					}
				},
				new UiZoneModel
				{
					Name = "BottomRight",
					ResizeTexture = true,
					VerticalJustificationType = UiVerticalJustificationType.Bottom,
					UiZonePositionType = UiZonePositionType.Row3Col4,
					BackgroundTexture = new SimpleImageModel
					{
						TextureName = "pallet",
						TextureRegion = new TextureRegionModel
						{
							TextureRegionType = TextureRegionType.Fill,
							TextureBox = PalletColorToTextureBoxHelper.GetPalletColorTextureBox(PalletColors.Hex_00396D)
						}
					}
				},             
				new UiZoneModel
				{
					Name = "BottomLeft",
					ResizeTexture = true,
					VerticalJustificationType = UiVerticalJustificationType.Bottom,
					UiZonePositionType = UiZonePositionType.Row3Col1,
					BackgroundTexture = new SimpleImageModel
					{
						TextureName = "pallet",
						TextureRegion = new TextureRegionModel
						{
							TextureRegionType = TextureRegionType.Fill,
							TextureBox = PalletColorToTextureBoxHelper.GetPalletColorTextureBox(PalletColors.Hex_00396D)
						}
					}

				},
			]; 
			
			return result;
		}

	}
}

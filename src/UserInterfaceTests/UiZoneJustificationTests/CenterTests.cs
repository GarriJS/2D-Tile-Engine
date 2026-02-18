using BaseContent.BaseContentConstants.Fonts;
using BaseContent.BaseContentConstants.Images;
using Common.DiskModels.UserInterface;
using Common.DiskModels.UserInterface.Elements;
using Common.UserInterface.Enums;
using Engine.DiskModels.Drawing;
using Engine.Graphics.Enum;
using Microsoft.Xna.Framework;

namespace UserInterfaceTests.UiZoneJustificationTests
{
	/// <summary>
	/// Contains tests for the center justification.
	/// </summary>
	static public class CenterTests
	{
		/// <summary>
		/// Gets the first center test.
		/// This tests that the center justification is working.
		/// A blue square should appear in the four screen zone corners, and in each one a dark gray square should appear in the middle left of the screen.
		/// Inside the dark gray square should be a purple square, and inside the purple square should be a light gray square.
		/// Inside the light gray square should be text describing the where it is.
		/// On the middle right of the screen the same thing will appear, but the inner squares will appear 3 times in a column.
		/// </summary>
		/// <returns>The first center test.</returns>
		static public UiGroupModel GetCenterTest1()
		{
			var zones = GetCenterTest1Zones();
			var result = new UiGroupModel
			{
				Name = "CenterTest1",
				VisibilityGroupId = 1,
				MakeVisible = true,
				Zones = zones
			};

			return result;
		}
		
		/// <summary>
		/// Gets the center tests 1 zones.
		/// </summary>
		/// <returns>The center tests 1 zones.</returns>
		private static UiZoneModel[] GetCenterTest1Zones()
		{
			UiZoneModel[] result =
			[
				new UiZoneModel
				{
					Name = "CenterLeftZone",
					ResizeTexture = true,
					VerticalJustificationType = UiVerticalJustificationType.Center,
					UiZonePositionType = UiZonePositionType.Row2Col2,
					BackgroundTexture = new SimpleImageModel
					{
						TextureName = "pallet",
						TextureRegion = new TextureRegionModel
						{
							TextureRegionType = TextureRegionType.Fill,
							TextureBox = PalletColorToTextureBoxHelper.GetPalletColorTextureBox(PalletColors.Hex_00396D)
						}
					},
					Blocks =
					[
						new UiBlockModel
						{
							Name = "CenterLeftZone",
							ResizeTexture = true,
							HorizontalJustificationType = UiHorizontalJustificationType.Center,
							VerticalJustificationType = UiVerticalJustificationType.Center,
							BackgroundTexture = new SimpleImageModel
							{
								TextureName = "pallet",
								TextureRegion = new TextureRegionModel
								{
									TextureRegionType = TextureRegionType.Fill,
									TextureBox = PalletColorToTextureBoxHelper.GetPalletColorTextureBox(PalletColors.Hex_5D5D5D)
								}
							},
							Rows =
							[
								new UiRowModel
								{
									Name = "CenterLeftRow",
									ResizeTexture = true,
									Margin = new UiMarginModel
									{
										TopMargin = 5,
										BottomMargin = 5,
										LeftMargin = 5,
										RightMargin = 5
									},
									HorizontalJustificationType = UiHorizontalJustificationType.Center,
									VerticalJustificationType = UiVerticalJustificationType.Center,
									BackgroundTexture = new SimpleImageModel
									{
										TextureName = "pallet",
										TextureRegion = new TextureRegionModel
										{
											TextureRegionType = TextureRegionType.Fill,
											TextureBox = PalletColorToTextureBoxHelper.GetPalletColorTextureBox(PalletColors.Hex_622461)
										}
									},
									Elements =
									[
										new UiTextModel
										{
											Name = "CenterLeftElement",
											ResizeTexture = true,
											HorizontalSizeType = UiElementSizeType.Large,
											VerticalSizeType = UiElementSizeType.Medium,
											Margin = new UiMarginModel
											{
												TopMargin = 5,
												BottomMargin = 5,
												LeftMargin = 5,
												RightMargin = 5
											},
											Graphic = new SimpleImageModel
											{
												TextureName = "pallet",
												TextureRegion = new TextureRegionModel
												{
													TextureRegionType = TextureRegionType.Fill,
													TextureBox = PalletColorToTextureBoxHelper.GetPalletColorTextureBox(PalletColors.Hex_858585)
												}
											},
											Text = new GraphicalTextModel
											{
												Text = "Center Left",
												TextColor = Color.Black,
												FontName = FontNames.MonoBold
											}
										}
									]
								}
							]
						}
					]
				},
				new UiZoneModel
				{
					Name = "CenterRightZone",
					ResizeTexture = true,
					VerticalJustificationType = UiVerticalJustificationType.Center,
					UiZonePositionType = UiZonePositionType.Row2Col3,
					BackgroundTexture = new SimpleImageModel
					{
						TextureName = "pallet",
						TextureRegion = new TextureRegionModel
						{
							TextureRegionType = TextureRegionType.Fill,
							TextureBox = PalletColorToTextureBoxHelper.GetPalletColorTextureBox(PalletColors.Hex_0069AA)
						}
					},
					Blocks =
					[
						new UiBlockModel
						{
							Name = "CenterRightZone",
							ResizeTexture = true,
							FlexRows = true,
							HorizontalJustificationType = UiHorizontalJustificationType.Center,
							VerticalJustificationType = UiVerticalJustificationType.Center,
							BackgroundTexture = new SimpleImageModel
							{
								TextureName = "pallet",
								TextureRegion = new TextureRegionModel
								{
									TextureRegionType = TextureRegionType.Fill,
									TextureBox = PalletColorToTextureBoxHelper.GetPalletColorTextureBox(PalletColors.Hex_5D5D5D)
								}
							},
							Rows =
							[
								new UiRowModel
								{
									Name = "CenterRightRow",
									ResizeTexture = true,
									Margin = new UiMarginModel
									{
										TopMargin = 5,
										BottomMargin = 5,
										LeftMargin = 5,
										RightMargin = 5
									},
									HorizontalJustificationType = UiHorizontalJustificationType.Center,
									VerticalJustificationType = UiVerticalJustificationType.Center,
									BackgroundTexture = new SimpleImageModel
									{
										TextureName = "pallet",
										TextureRegion = new TextureRegionModel
										{
											TextureRegionType = TextureRegionType.Fill,
											TextureBox = PalletColorToTextureBoxHelper.GetPalletColorTextureBox(PalletColors.Hex_622461)
										}
									},
									Elements =
									[
										new UiTextModel
										{
											Name = "CenterRightElement1",
											ResizeTexture = true,
											HorizontalSizeType = UiElementSizeType.Large,
											VerticalSizeType = UiElementSizeType.Medium,
											Margin = new UiMarginModel
											{
												TopMargin = 5,
												BottomMargin = 5,
												LeftMargin = 5,
												RightMargin = 5
											},
											Graphic = new SimpleImageModel
											{
												TextureName = "pallet",
												TextureRegion = new TextureRegionModel
												{
													TextureRegionType = TextureRegionType.Fill,
													TextureBox = PalletColorToTextureBoxHelper.GetPalletColorTextureBox(PalletColors.Hex_858585)
												}
											},
											Text = new GraphicalTextModel
											{
												Text = "Center Right 1",
												TextColor = Color.Black,
												FontName = FontNames.MonoBold
											}
										},
										new UiTextModel
										{
											Name = "CenterRightElement2",
											ResizeTexture = true,
											HorizontalSizeType = UiElementSizeType.Large,
											VerticalSizeType = UiElementSizeType.Medium,
											Margin = new UiMarginModel
											{
												TopMargin = 5,
												BottomMargin = 5,
												LeftMargin = 5,
												RightMargin = 5
											},
											Graphic = new SimpleImageModel
											{
												TextureName = "pallet",
												TextureRegion = new TextureRegionModel
												{
													TextureRegionType = TextureRegionType.Fill,
													TextureBox = PalletColorToTextureBoxHelper.GetPalletColorTextureBox(PalletColors.Hex_858585)
												}
											},
											Text = new GraphicalTextModel
											{
												Text = "Center Right 2",
												TextColor = Color.Black,
												FontName = FontNames.MonoBold
											}
										},
										new UiTextModel
										{
											Name = "CenterRightElement3",
											ResizeTexture = true,
											HorizontalSizeType = UiElementSizeType.Large,
											VerticalSizeType = UiElementSizeType.Medium,
											Margin = new UiMarginModel
											{
												TopMargin = 5,
												BottomMargin = 5,
												LeftMargin = 5,
												RightMargin = 5
											},
											Graphic = new SimpleImageModel
											{
												TextureName = "pallet",
												TextureRegion = new TextureRegionModel
												{
													TextureRegionType = TextureRegionType.Fill,
													TextureBox = PalletColorToTextureBoxHelper.GetPalletColorTextureBox(PalletColors.Hex_858585)
												}
											},
											Text = new GraphicalTextModel
											{
												Text = "Center Right 3",
												TextColor = Color.Black,
												FontName = FontNames.MonoBold
											}
										}
									]
								}
							]
						}
					]
				}
			];

			return result;
		}
	}
}

using BaseContent.BaseContentConstants.Fonts;
using BaseContent.BaseContentConstants.Images;
using Common.DiskModels.UserInterface;
using Common.DiskModels.UserInterface.Elements;
using Common.UserInterface.Enums;
using Engine.DiskModels.Drawing;
using Engine.Graphics.Enum;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserInterfaceTests.UiZoneJustificationTests
{
	static public class CenterTests
	{
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

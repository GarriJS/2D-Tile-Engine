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
					Name = "TopLeftZone",
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
					},
					Blocks =
					[
						new UiBlockModel
						{
							Name = "TopLeftZone",
							ResizeTexture = true,
							HorizontalJustificationType = UiHorizontalJustificationType.Left,
							VerticalJustificationType = UiVerticalJustificationType.Top,
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
									Name = "TopLeftRow",
									ResizeTexture = true,
									Margin = new UiMarginModel
									{
										TopMargin = 5,
										BottomMargin = 5,
										LeftMargin = 5,
										RightMargin = 5
									},
									HorizontalJustificationType = UiHorizontalJustificationType.Left,
									VerticalJustificationType = UiVerticalJustificationType.Top,
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
											Name = "TopLeftElement",
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
												Text = "Top Left",
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
					Name = "TopRightZone",
					ResizeTexture = true,
					VerticalJustificationType = UiVerticalJustificationType.Top,
					UiZonePositionType = UiZonePositionType.Row1Col4,
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
							Name = "TopRightBlock",
							ResizeTexture = true,
							HorizontalJustificationType = UiHorizontalJustificationType.Right,
							VerticalJustificationType = UiVerticalJustificationType.Top,
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
									Name = "TopRightRow",
									ResizeTexture = true,
									Margin = new UiMarginModel
									{
										TopMargin = 5,
										BottomMargin = 5,
										LeftMargin = 5,
										RightMargin = 5
									},
									HorizontalJustificationType = UiHorizontalJustificationType.Right,
									VerticalJustificationType = UiVerticalJustificationType.Top,
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
											Name = "TopRightElement",
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
												Text = "Top Right",
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
					Name = "BottomRightZone",
					ResizeTexture = true,
					VerticalJustificationType = UiVerticalJustificationType.Bottom,
					UiZonePositionType = UiZonePositionType.Row3Col4,
					BackgroundTexture = new SimpleImageModel
					{
						TextureName = "pallet",
						TextureRegion = new TextureRegionModel
						{
							TextureRegionType = TextureRegionType.Fill,
							TextureBox = PalletColorToTextureBoxHelper.GetPalletColorTextureBox(PalletColors.Hex_0098DC)
						}
					},
					Blocks =
					[
						new UiBlockModel
						{
							Name = "BottomRightBlock",
							ResizeTexture = true,
							HorizontalJustificationType = UiHorizontalJustificationType.Right,
							VerticalJustificationType = UiVerticalJustificationType.Bottom,
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
									Name = "BottomRightRow",
									ResizeTexture = true,
									Margin = new UiMarginModel
									{
										TopMargin = 5,
										BottomMargin = 5,
										LeftMargin = 5,
										RightMargin = 5
									},
									HorizontalJustificationType = UiHorizontalJustificationType.Right,
									VerticalJustificationType = UiVerticalJustificationType.Bottom,
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
											Name = "BottomRightElement",
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
												Text = "Bottom Right",
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
					Name = "BottomLeftZone",
					ResizeTexture = true,
					VerticalJustificationType = UiVerticalJustificationType.Bottom,
					UiZonePositionType = UiZonePositionType.Row3Col1,
					BackgroundTexture = new SimpleImageModel
					{
						TextureName = "pallet",
						TextureRegion = new TextureRegionModel
						{
							TextureRegionType = TextureRegionType.Fill,
							TextureBox = PalletColorToTextureBoxHelper.GetPalletColorTextureBox(PalletColors.Hex_00CDF9)
						}
					},
					Blocks =
					[
						new UiBlockModel
						{
							Name = "BottomLeftBLock",
							ResizeTexture = true,
							HorizontalJustificationType = UiHorizontalJustificationType.Left,
							VerticalJustificationType = UiVerticalJustificationType.Bottom,
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
									Name = "BottomLeftRow",
									ResizeTexture = true,
									Margin = new UiMarginModel
									{
										TopMargin = 5,
										BottomMargin = 5,
										LeftMargin = 5,
										RightMargin = 5
									},
									HorizontalJustificationType = UiHorizontalJustificationType.Left,
									VerticalJustificationType = UiVerticalJustificationType.Bottom,
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
											Name = "BottomLeftElement",
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
												Text = "Bottom Left",
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
			]; 
			
			return result;
		}

	}
}

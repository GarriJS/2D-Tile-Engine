using Common.DiskModels.Common.Tiling;
using Engine.DiskModels.Drawing;
using Engine.DiskModels.Physics;
using Engine.DiskModels.UI;
using Engine.DiskModels.UI.Elements;
using Engine.UI.Models.Enums;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace LevelEditor.Core.Initialization
{
	/// <summary>
	/// Represents a level editor initializer.
	/// </summary>
	public static class LevelEditorInitializer
	{
		/// <summary>
		/// Gets the initial disk models.
		/// </summary>
		/// <returns>The disk models.</returns>
		public static IList<object> GetInitialDiskModels()
		{
			return
			[
				new TileModel
				{
					Row = 1,
					Column = 1,
					Area = new SimpleAreaModel
					{
						Height = 64,
						Width = 64,
					},
					Sprite = new SpriteModel
					{
						SpritesheetBox = new Rectangle(0, 0, 64, 64),
						TextureName = "grass_tileset"
					}
				}
			];
		}

		/// <summary>
		/// Gets the initial user interface models.
		/// </summary>
		/// <returns>The user interface models.</returns>
		public static IList<UiGroupModel> GetInitialUiModels()
		{
			return
			[
				new UiGroupModel
				{
					UiGroupName = "foo",
					VisibilityGroupId = 1,
					IsVisible = true,
					UiZoneElements =
					[
						new UiZoneModel
						{
							UiZoneName = "foo1",
							UiZoneType = (int)UiScreenZoneTypes.Row1Col1,
							Background = new ImageModel
							{
								TextureName = "debug"
							},
							JustificationType = (int)UiZoneJustificationTypes.Center,
							ElementRows =
							[
								new UiRowModel
								{
									UiRowName = "foo1row1",
									TopPadding = 5,
									BottomPadding = 5,
									HorizontalJustificationType = (int)UiRowHorizontalJustificationTypes.Left,
									VerticalJustificationType = (int)UiRowVerticalJustificationTypes.Top,
									SubElements =
									[
										new UiButtonModel
										{
											UiElementName = "foo1button1",
											LeftPadding = 5,
											RightPadding = 5,
											BackgroundTextureName = "black",
											Text = "Push Me",
											SizeType = (int)UiElementSizeTypes.ExtraSmall,
											ClickableAreaScaler = new Vector2(.9f, .9f),
											ClickableAreaAnimation = new TriggeredAnimationModel
											{
												CurrentFrameIndex = 0,
												FrameDuration = 1000,
												RestingFrameIndex = 0,
												Frames =
												[
													new ImageModel
													{
														TextureName = "white",
													},
													new ImageModel
													{
														TextureName = "black",
													}
												]
											}
										},
										new UiButtonModel
										{
											UiElementName = "foo1button2",
											LeftPadding = 0,
											RightPadding = 0,
											BackgroundTextureName = "gray",
											Text = "Push Me",
											SizeType = (int)UiElementSizeTypes.ExtraLarge,
											ClickableAreaScaler = new Vector2(.95f, .95f),
											ClickableAreaAnimation = new TriggeredAnimationModel
											{
												CurrentFrameIndex = 0,
												FrameDuration = 1000,
												RestingFrameIndex = 0,
												Frames =
												[
													new ImageModel
													{
														TextureName = "white",
													},
													new ImageModel
													{
														TextureName = "gray",
													}
												]
											}
										},
										new UiButtonModel
										{
											UiElementName = "foo1button3",
											LeftPadding = 5,
											RightPadding = 5,
											BackgroundTextureName = "white",
											Text = "Push Me",
											SizeType = (int)UiElementSizeTypes.Small,
											ClickableAreaScaler = new Vector2(.9f, .9f),
											ClickableAreaAnimation = new TriggeredAnimationModel
											{
												CurrentFrameIndex = 0,
												FrameDuration = 1000,
												RestingFrameIndex = 0,
												Frames =
												[
													new ImageModel
													{
														TextureName = "black",
													},
													new ImageModel
													{
														TextureName = "white",
													}
												]
											}
										}
									]
								},
								new UiRowModel
								{
									UiRowName = "foo1row2",
									TopPadding = 5,
									BottomPadding = 5,
									HorizontalJustificationType =  (int)UiRowHorizontalJustificationTypes.Left,
									VerticalJustificationType = (int)UiRowVerticalJustificationTypes.Bottom,
									SubElements =
									[
										new UiButtonModel
										{
											UiElementName = "foo1button1",
											LeftPadding = 5,
											RightPadding = 0,
											BackgroundTextureName = "white",
											Text = "Push Me",
											SizeType = (int)UiElementSizeTypes.Medium,
											ClickableAreaScaler = new Vector2(.9f, .9f),
											ClickableAreaAnimation = new TriggeredAnimationModel
											{
												CurrentFrameIndex = 0,
												FrameDuration = 1000,
												RestingFrameIndex = 0,
												Frames =
												[
													new ImageModel
													{
														TextureName = "black",
													},
													new ImageModel
													{
														TextureName = "white",
													}
												]
											}
										},
										new UiButtonModel
										{
											UiElementName = "foo1button2",
											LeftPadding = 5,
											RightPadding = 5,
											BackgroundTextureName = "black",
											Text = "Push Me",
											SizeType = (int)UiElementSizeTypes.Large
										}
									]
								}
							]
						}
					]
				},
				new UiGroupModel
				{
					UiGroupName = "bar",
					VisibilityGroupId = 2,
					IsVisible = false,
					UiZoneElements =
					[
						new UiZoneModel
						{
							UiZoneName = "foo1",
							UiZoneType = (int)UiScreenZoneTypes.Row3Col2,
							Background = new ImageModel
							{
								TextureName = "debug"
							},
							JustificationType = (int)UiZoneJustificationTypes.Center,
							ElementRows =
							[
								new UiRowModel
								{
									UiRowName = "foo1row1",
									TopPadding = 5,
									BottomPadding = 5,
									HorizontalJustificationType = (int)UiRowHorizontalJustificationTypes.Right,
									VerticalJustificationType = (int)UiRowVerticalJustificationTypes.Top,
									SubElements =
									[
										new UiButtonModel
										{
											UiElementName = "foo1button1",
											LeftPadding = 5,
											RightPadding = 5,
											BackgroundTextureName = "black",
											Text = "Push Me",
											FixedSized = new Vector2(100, 100)
										},
										new UiButtonModel
										{
											UiElementName = "foo1button2",
											LeftPadding = 0,
											RightPadding = 0,
											BackgroundTextureName = "gray",
											Text = "Push Me",
											SizeType = (int)UiElementSizeTypes.ExtraLarge,
											ClickableAreaScaler = new Vector2(.9f, .9f),
											ClickableAreaAnimation = new TriggeredAnimationModel
											{
												CurrentFrameIndex = 0,
												FrameDuration = 1000,
												RestingFrameIndex = 0,
												Frames =
												[
													new ImageModel
													{
														TextureName = "black",
													},
													new ImageModel
													{
														TextureName = "white",
													}
												]
											}
										},
										new UiButtonModel
										{
											UiElementName = "foo1button3",
											LeftPadding = 5,
											RightPadding = 5,
											BackgroundTextureName = "white",
											Text = "Push Me",
											SizeType = (int)UiElementSizeTypes.Small,
											ClickableAreaScaler = new Vector2(1f, 1f),
										}
									]
								},
								new UiRowModel
								{
									UiRowName = "foo1row2",
									TopPadding = 5,
									BottomPadding = 5,
									HorizontalJustificationType =  (int)UiRowHorizontalJustificationTypes.Right,
									VerticalJustificationType = (int)UiRowVerticalJustificationTypes.Bottom,
									SubElements =
									[
										new UiButtonModel
										{
											UiElementName = "foo1button1",
											LeftPadding = 5,
											RightPadding = 0,
											BackgroundTextureName = "white",
											Text = "Push Me",
											SizeType = (int)UiElementSizeTypes.Medium,
											ClickableAreaScaler = new Vector2(1f, 1f),
										},
										new UiButtonModel
										{
											UiElementName = "foo1button2",
											LeftPadding = 5,
											RightPadding = 5,
											BackgroundTextureName = "black",
											Text = "Push Me",
											SizeType = (int)UiElementSizeTypes.Large,
											ClickableAreaScaler = new Vector2(1f, 1f),
										}
									]
								}
							]
						}
					]
				}
			];
		}
	}
}

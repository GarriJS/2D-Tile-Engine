using Common.DiskModels.UI;
using Common.DiskModels.UI.Elements;
using Common.UserInterface.Models.Contracts;
using Common.UserInterface.Enums;
using Engine.DiskModels.Drawing;
using LevelEditor.Core.Constants;
using LevelEditor.Spritesheets.Services.Contracts;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Common.UserInterface.Constants;
using Common.Controls.Cursors.Constants;
using Common.Controls.Cursors.Models;
using Common.Controls.Cursors.Services.Contracts;
using Common.Tiling.Services.Contracts;
using LevelEditor.Controls.Constants;

namespace LevelEditor.Core.Initialization
{
    /// <summary>
    /// Represents a level editor initializer.
    /// </summary>
    public static class LevelEditorInitializer
	{
		/// <summary>
		/// Gets the function providers. 
		/// </summary>
		/// <param name="gameServices">The game service.</param>
		/// <returns>The function providers.</returns>
		public static Dictionary<string, Delegate> GetFunctionProviders(GameServiceContainer gameServices)
		{
			var result = new Dictionary<string, Delegate>();

			foreach (var kv in GetCursorUpdaters(gameServices))
				result[kv.Key] = kv.Value;

			foreach (var kv in GetHoverEventProcessors(gameServices))
				result[kv.Key] = kv.Value;

			foreach (var kv in GetPressEventProcessors(gameServices))
				result[kv.Key] = kv.Value;

			foreach (var kv in GetClickEventProcessors(gameServices))
				result[kv.Key] = kv.Value;

			return result;
		}

		/// <summary>
		/// Gets the cursor updaters.
		/// </summary>
		/// <param name="gameService">The game service.</param>
		/// <returns>A dictionary of the cursor updaters.</returns>
		public static Dictionary<string, Action<Cursor, GameTime>> GetCursorUpdaters(GameServiceContainer gameService)
		{
			var spritesheetButtonService = gameService.GetService<ISpritesheetButtonService>();

			return new Dictionary<string, Action<Cursor, GameTime>>
			{
				[LevelEditorCursorUpdatersNames.SpritesheetButtonCursorUpdater] = spritesheetButtonService.SpritesheetButtonCursorUpdater,
			};
		}

		/// <summary>
		/// Gets the hover event processors.
		/// </summary>
		/// <param name="gameServices">The game services.</param>
		/// <returns>A dictionary of the hover event processors.</returns>
		public static Dictionary<string, Action<IAmAUiElement, Vector2>> GetHoverEventProcessors(GameServiceContainer gameServices)
		{
			var spritesheetButtonService = gameServices.GetService<ISpritesheetButtonService>();

			return new Dictionary<string, Action<IAmAUiElement, Vector2>>
			{

			};
		}

		/// <summary>
		/// Gets the press event processors.
		/// </summary>
		/// <param name="gameServices">The game services.</param>
		/// <returns>A dictionary of the press event processors.</returns>
		public static Dictionary<string, Action<IAmAUiElement, Vector2>> GetPressEventProcessors(GameServiceContainer gameServices)
		{
			return new Dictionary<string, Action<IAmAUiElement, Vector2>>
			{

			};
		}

		/// <summary>
		/// Gets the click event processors.
		/// </summary>
		/// <param name="gameServices">The game services.</param>
		/// <returns>A dictionary of the click event processors.</returns>
		public static Dictionary<string, Action<IAmAUiElement, Vector2>> GetClickEventProcessors(GameServiceContainer gameServices)
		{
			var spritesheetButtonService = gameServices.GetService<ISpritesheetButtonService>();

			return new Dictionary<string, Action<IAmAUiElement, Vector2>>
			{
				[UiEventName.SpritesheetButtonClick] = spritesheetButtonService.SpritesheetButtonClickEventProcessor
			};
		}

		/// <summary>
		/// Gets the initial user interface models.
		/// </summary>
		/// <param name="gameServices">The game services.</param>
		/// <returns>The user interface models.</returns>
		public static IList<object> GetInitialUiModels(GameServiceContainer gameServices)
		{
			var spritesheetButtonService = gameServices.GetService<ISpritesheetButtonService>();
			var spritesheetButtons = spritesheetButtonService.GetUiButtonsForSpritesheet("dark_grass_simplified", new Point(32, 32));
			var flattenedButtons = spritesheetButtons?.SelectMany(row => row).ToArray();

			return
			[
				new UiGroupModel
				{
					UiGroupName = "Level Editor Main UI",
					VisibilityGroupId = 1,
					IsVisible = true,
					UiZoneElements =
					[
						new UiZoneModel
						{
							UiZoneName = "Level Editor Label Row",
							UiZoneType = (int)UiScreenZoneTypes.Row1Col4,
							BackgroundTextureName = string.Empty,
							JustificationType = (int)UiZoneJustificationTypes.Top,
							ElementRows =
							[
								new UiRowModel
								{
									UiRowName = "Level Editor Label Row",
									TopPadding = 0,
									BottomPadding = 0,
									BackgroundTextureName = null,
									HorizontalJustificationType = (int)UiRowHorizontalJustificationTypes.Right,
									VerticalJustificationType = (int)UiRowVerticalJustificationTypes.Center,
									SubElements =
									[
										new UiTextModel
										{
											UiElementName = "Level Editor Label Element",
											LeftPadding = 0,
											RightPadding = 0,
											BackgroundTextureName = "gray",
											ElementHoverCursorName = CommonCursorNames.BasicCursorName,
											Text = "Level Editor",
											SizeType = (int)UiElementSizeTypes.Small
										}
									]
								}
							]
						},
						new UiZoneModel
						{
							UiZoneName = "Disc Zone",
							UiZoneType = (int)UiScreenZoneTypes.Row3Col1,
							BackgroundTextureName = string.Empty,
							JustificationType = (int)UiZoneJustificationTypes.Bottom,
							ElementRows =
							[
								new UiRowModel
								{
									UiRowName = "Disc Row 1",
									TopPadding = 0,
									BottomPadding = 0,
									BackgroundTextureName = null,
									RowHoverCursorName = CommonCursorNames.BasicCursorName,
									HorizontalJustificationType = (int)UiRowHorizontalJustificationTypes.Left,
									VerticalJustificationType = (int)UiRowVerticalJustificationTypes.Center,
									SubElements =
									[
										new UiTextModel
										{
											UiElementName = "TempElement",
											LeftPadding = 0,
											RightPadding = 0,
											BackgroundTextureName = "white",
											Text = "Disc Buttons to go here",
											SizeType = (int)UiElementSizeTypes.Full
										}
									]
								}
							]
						},
						new UiZoneModel
						{
							UiZoneName = "foo1",
							UiZoneType = (int)UiScreenZoneTypes.Row3Col2,
							BackgroundTextureName = null,
							JustificationType = (int)UiZoneJustificationTypes.Bottom,
							ElementRows =
							[
								new UiRowModel
								{
									UiRowName = "foo1row3",
									TopPadding = 15,
									BottomPadding = 15,
									BackgroundTextureName = "gray_transparent",
									RowHoverCursorName = CommonCursorNames.BasicCursorName,
									HorizontalJustificationType =  (int)UiRowHorizontalJustificationTypes.Center,
									VerticalJustificationType = (int)UiRowVerticalJustificationTypes.Bottom,
									SubElements = flattenedButtons
								}
							]
						}
					]
				}
			];

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
							UiZoneType = (int)UiScreenZoneTypes.Row2Col1,
							BackgroundTextureName = string.Empty,
							JustificationType = (int)UiZoneJustificationTypes.Bottom,
							ElementRows =
							[
								new UiRowModel
								{
									UiRowName = "foo1row1",
									TopPadding = 0,
									BottomPadding = 0,
									BackgroundTextureName = null,
									RowHoverCursorName = CommonCursorNames.BasicCursorName,
									HorizontalJustificationType = (int)UiRowHorizontalJustificationTypes.Left,
									VerticalJustificationType = (int)UiRowVerticalJustificationTypes.Center,
									SubElements =
									[
										new UiTextModel
										{
											UiElementName = "foo1button1",
											LeftPadding = 0,
											RightPadding = 0,
											BackgroundTextureName = "white",
											Text = "Welcome to the level editor!",
											SizeType = (int)UiElementSizeTypes.ExtraLarge
										}
									]
								}
							]
						},
						new UiZoneModel
						{
							UiZoneName = "foo1",
							UiZoneType = (int)UiScreenZoneTypes.Row3Col1,
							BackgroundTextureName = "gray_transparent",
							JustificationType = (int)UiZoneJustificationTypes.Center,
							ElementRows =
							[
								new UiRowModel
								{
									UiRowName = "foo1row1",
									TopPadding = 4,
									BottomPadding = 4,
									HorizontalJustificationType = (int)UiRowHorizontalJustificationTypes.Right,
									VerticalJustificationType = (int)UiRowVerticalJustificationTypes.Top,
									SubElements =
									[
										new UiButtonModel
										{
											UiElementName = "foo1button2",
											LeftPadding = 0,
											RightPadding = 0,
											BackgroundTextureName = "gray",
											ButtonText = "Push Me",
											SizeType = (int)UiElementSizeTypes.Fill,
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
									TopPadding = 4,
									BottomPadding = 4,
									HorizontalJustificationType =  (int)UiRowHorizontalJustificationTypes.Center,
									VerticalJustificationType = (int)UiRowVerticalJustificationTypes.Bottom,
									SubElements =
									[
										new UiButtonModel
										{
											UiElementName = "foo1button1",
											LeftPadding = 4,
											RightPadding = 2,
											BackgroundTextureName = "white",
											ButtonText = "Push Me 1",
											SizeType = (int)UiElementSizeTypes.Fill,
											ClickableAreaScaler = new Vector2(1f, 1f),
										},
										new UiButtonModel
										{
											UiElementName = "foo1button2",
											LeftPadding = 2,
											RightPadding = 4,
											BackgroundTextureName = "black",
											ButtonText = "Push Me 2",
											SizeType = (int)UiElementSizeTypes.Fill,
											ClickableAreaScaler = new Vector2(1f, 1f),
										}
									]
								}
							]
						},
						new UiZoneModel
						{
							UiZoneName = "foo1",
							UiZoneType = (int)UiScreenZoneTypes.Row3Col2,
							BackgroundTextureName = "gray_transparent",
							JustificationType = (int)UiZoneJustificationTypes.Top,
							ElementRows =
							[
								new UiRowModel
								{
									UiRowName = "foo1row3",
									TopPadding = 40,
									BottomPadding = 4,
									HorizontalJustificationType =  (int)UiRowHorizontalJustificationTypes.Center,
									VerticalJustificationType = (int)UiRowVerticalJustificationTypes.None,
									SubElements = flattenedButtons
								}
							]
						}
					]
				}
			];
		}
	}
}

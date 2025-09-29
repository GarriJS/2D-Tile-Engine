using Common.Controls.Cursors.Constants;
using Common.Controls.Cursors.Models;
using Common.DiskModels.UI;
using Common.DiskModels.UI.Elements;
using Common.UserInterface.Enums;
using Common.UserInterface.Models.Contracts;
using Engine.DiskModels.Drawing;
using LevelEditor.Controls.Constants;
using LevelEditor.Core.Constants;
using LevelEditor.Scenes.Services.Contracts;
using LevelEditor.Spritesheets.Services.Contracts;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

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
			var sceneEditService = gameServices.GetService<ISceneEditService>();

			return new Dictionary<string, Action<IAmAUiElement, Vector2>>
			{
				[UiEventName.SpritesheetButtonClick] = spritesheetButtonService.SpritesheetButtonClickEventProcessor,
				[UiEventName.CreateSceneClick] = sceneEditService.CreateSceneButtonClickEventProcessor,
				[UiEventName.ToggleTileGrid] = sceneEditService.ToggleTileGridClickEventProcessor,
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
							JustificationType = (int)UiZoneJustificationTypes.Top,
							ElementRows =
							[
								new UiRowModel
								{
									UiRowName = "Create Level",
									TopPadding = 0,
									BottomPadding = 0,
									BackgroundTextureName = "white",
									RowHoverCursorName = CommonCursorNames.BasicCursorName,
									HorizontalJustificationType = (int)UiRowHorizontalJustificationTypes.Center,
									VerticalJustificationType = (int)UiRowVerticalJustificationTypes.Center,
									SubElements =
									[
										new UiTextModel
										{
											UiElementName = "Create Element Label",
											LeftPadding = 10,
											RightPadding = 0,
											Text = "Create Level",
											SizeType = (int)UiElementSizeTypes.ExtraSmall
										},
										new UiButtonModel
										{
											UiElementName = "Create Element Button",
											LeftPadding = 0,
											RightPadding = 0,
											SizeType = (int)UiElementSizeTypes.Fit,
											ClickableAreaAnimation = new TriggeredAnimationModel
											{ 
												CurrentFrameIndex = 0,
												FrameDuration = 500,
												Frames =
												[
													new ImageModel
													{ 
														TextureName = "dark_blue_buttons",
														TextureBox = new Rectangle
														{ 
															X = 0,
															Y = 0,
															Width = 64,
															Height = 64,
														}
													},
													new ImageModel
													{
														TextureName = "dark_blue_buttons",
														TextureBox = new Rectangle
														{
															X = 64,
															Y = 0,
															Width = 64,
															Height = 64,
														}
													}
												],
												RestingFrameIndex = 0,
											},
											ClickableAreaScaler = new Vector2
											{
												X = 1,
												Y = 1
											},
											ButtonClickEventName = UiEventName.CreateSceneClick
										},
										new UiTextModel
										{
											UiElementName = "Save Element Label",
											LeftPadding = 10,
											RightPadding = 0,
											BackgroundTextureName = null,
											Text = "Save Level",
											SizeType = (int)UiElementSizeTypes.ExtraSmall
										},
										new UiButtonModel
										{
											UiElementName = "Save Element Button",
											LeftPadding = 0,
											RightPadding = 0,
											SizeType = (int)UiElementSizeTypes.Fit,
											ClickableAreaAnimation = new TriggeredAnimationModel
											{
												CurrentFrameIndex = 0,
												FrameDuration = 500,
												Frames =
												[
													new ImageModel
													{
														TextureName = "dark_blue_buttons",
														TextureBox = new Rectangle
														{
															X = 0,
															Y = 64,
															Width = 64,
															Height = 64,
														}
													},
													new ImageModel
													{
														TextureName = "dark_blue_buttons",
														TextureBox = new Rectangle
														{
															X = 64,
															Y = 64,
															Width = 64,
															Height = 64,
														}
													}
												],
												RestingFrameIndex = 0,
											},
											ClickableAreaScaler = new Vector2
											{
												X = 1,
												Y = 1
											},
										}
									]
								},
								new UiRowModel
								{
									UiRowName = "Levels Header",
									TopPadding = 0,
									BottomPadding = 0,
									BackgroundTextureName = "white",
									RowHoverCursorName = CommonCursorNames.BasicCursorName,
									HorizontalJustificationType = (int)UiRowHorizontalJustificationTypes.Center,
									VerticalJustificationType = (int)UiRowVerticalJustificationTypes.Center,
									SubElements =
									[
										new UiTextModel
										{
											UiElementName = "Saved Levels Element",
											LeftPadding = 0,
											RightPadding = 0,
											BackgroundTextureName = null,
											Text = "Saved Levels",
											SizeType = (int)UiElementSizeTypes.ExtraSmall
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

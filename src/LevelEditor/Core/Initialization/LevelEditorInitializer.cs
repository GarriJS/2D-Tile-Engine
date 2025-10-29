using BaseContent.BaseContentConstants.Fonts;
using BaseContent.BaseContentConstants.Images;
using Common.Controls.Cursors.Constants;
using Common.Controls.Cursors.Models;
using Common.DiskModels.UI;
using Common.DiskModels.UI.Elements;
using Common.UserInterface.Enums;
using Common.UserInterface.Models.Contracts;
using Engine.DiskModels.Drawing;
using Engine.DiskModels.Physics;
using Engine.Graphics.Enum;
using LevelEditor.Controls.Constants;
using LevelEditor.Core.Constants;
using LevelEditor.LevelEditorContent.Images.Manifests;
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
				[UiEventName.SaveSceneClick] = sceneEditService.SaveScene,
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
			var sceneEditService = gameServices.GetService<ISceneEditService>();

			var spritesheetButtons = spritesheetButtonService.GetUiButtonsForSpritesheet("dark_grass_simplified", new Point(32, 32));
			var flattenedButtons = spritesheetButtons?.SelectMany(row => row).ToArray();
			var savedTileMapsRowModel = sceneEditService.GetSavedTileMapUserInterfaceRows();

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
							BackgroundTexture = null,
							JustificationType = (int)UiZoneJustificationTypes.Top,
							ElementRows =
							[
								new UiRowModel
								{
									UiRowName = "Level Editor Label Row",
									ResizeTexture = true,
									HorizontalJustificationType = UiRowHorizontalJustificationType.Right,
									VerticalJustificationType = UiRowVerticalJustificationType.Center,
									SubElements =
									[
										new UiTextModel
										{
											UiElementName = "Level Editor Label Element",
											HoverCursorName = CommonCursorNames.BasicCursorName,
											InsidePadding = new UiPaddingModel
											{
												LeftPadding = 10,
												RightPadding = 10,
											},
											HorizontalSizeType = (int)UiElementSizeType.FitContent,
											VerticalSizeType = (int)UiElementSizeType.FitContent,
											Text = new GraphicalTextModel
											{
												Text = "Level Editor",
												TextColor = PalletColors.Hex_BF6F4A,
												FontName = FontNames.MonoBold
											},
											Graphic = new SimpleImageModel
											{
												TextureName = "pallet",
												TextureRegion = new TextureRegionModel
												{
													TextureRegionType = TextureRegionType.Fill,
													TextureBox = PalletColorToTextureBoxHelper.GetPalletColorTextureBox(PalletColors.Hex_C7CFDD)
												}
											}
										}
									]
								}
							]
						},
						new UiZoneModel
						{
							UiZoneName = "Disc Zone",
							UiZoneType = (int)UiScreenZoneTypes.Row3Col1,
							BackgroundTexture = null,
							JustificationType = (int)UiZoneJustificationTypes.Top,
							ElementRows =
							[
								new UiRowModel
								{
									UiRowName = "Create Level",
									InsidePadding = new UiPaddingModel
									{
										TopPadding = 10,
									},
									ResizeTexture = true,
									BackgroundTexture = new SimpleImageModel
									{
										TextureName = "pallet",
										TextureRegion = new TextureRegionModel
										{
											TextureRegionType = TextureRegionType.Fill,
											TextureBox = PalletColorToTextureBoxHelper.GetPalletColorTextureBox(PalletColors.Hex_C7CFDD)
										}
									},
									RowHoverCursorName = CommonCursorNames.BasicCursorName,
									HorizontalJustificationType = (int)UiRowHorizontalJustificationType.Center,
									VerticalJustificationType = (int)UiRowVerticalJustificationType.Center,
									SubElements =
									[
										new UiTextModel
										{
											UiElementName = "Create Level Label",
											InsidePadding = new UiPaddingModel
											{
												LeftPadding = 10,
												RightPadding = 5,
											},
											Text = new GraphicalTextModel
											{
												Text = "Create Level",
												TextColor = PalletColors.Hex_BF6F4A,
												FontName = FontNames.MonoBold
											},
											HorizontalSizeType = (int)UiElementSizeType.FitContent,
											VerticalSizeType = (int)UiElementSizeType.FitContent
										},
										new UiButtonModel
										{
											UiElementName = "Create Level Button",
											HorizontalSizeType = (int)UiElementSizeType.FitContent,
											VerticalSizeType = (int)UiElementSizeType.FitContent,
											ClickableAreaAnimation = new TriggeredAnimationModel
											{
												CurrentFrameIndex = 0,
												FrameDuration = 500,
												Frames =
												[
													DarkBlueButtonsManifest.UnpressedPlusButton,
													DarkBlueButtonsManifest.PressedPlusButton
												],
												RestingFrameIndex = 0,
											},
											ClickableAreaScaler = new Vector2
											{
												X = 1,
												Y = 1
											},
											ClickEventName = UiEventName.CreateSceneClick
										},
										new UiTextModel
										{
											UiElementName = "Save Element Label",
											InsidePadding = new UiPaddingModel
											{
												LeftPadding = 10,
												RightPadding = 5,
											},
											Text = new GraphicalTextModel
											{
												Text = "Save Level",
												TextColor = PalletColors.Hex_BF6F4A,
												FontName = FontNames.MonoBold
											},
											HorizontalSizeType = (int)UiElementSizeType.FitContent,
											VerticalSizeType = (int)UiElementSizeType.FitContent
										},
										new UiButtonModel
										{
											UiElementName = "Save Level Button",
											HorizontalSizeType = (int)UiElementSizeType.FitContent,
											VerticalSizeType = (int)UiElementSizeType.FitContent,
											ClickableAreaAnimation = new TriggeredAnimationModel
											{
												CurrentFrameIndex = 0,
												FrameDuration = 500,
												Frames =
												[
													DarkBlueButtonsManifest.UnpressedEnterButton,
													DarkBlueButtonsManifest.PressedEnterButton
												],
												RestingFrameIndex = 0,
											},
											ClickableAreaScaler = new Vector2
											{
												X = 1,
												Y = 1
											},
											ClickEventName = UiEventName.SaveSceneClick
										}
									]
								},
								new UiRowModel
								{
									UiRowName = "Levels Header",
									ResizeTexture = true,
									BackgroundTexture = new SimpleImageModel
									{
										TextureName = "pallet",
										TextureRegion = new TextureRegionModel
										{
											TextureRegionType = TextureRegionType.Fill,
											TextureBox = PalletColorToTextureBoxHelper.GetPalletColorTextureBox(PalletColors.Hex_C7CFDD)
										}
									},
									RowHoverCursorName = CommonCursorNames.BasicCursorName,
									HorizontalJustificationType = (int)UiRowHorizontalJustificationType.Center,
									VerticalJustificationType = (int)UiRowVerticalJustificationType.Center,
									SubElements =
									[
										new UiTextModel
										{
											UiElementName = "Saved Levels Element",
											Text = new GraphicalTextModel
											{
												Text = "Saved Levels",
												TextColor = PalletColors.Hex_BF6F4A,
												FontName = FontNames.MonoBold
											},
											HorizontalSizeType = (int)UiElementSizeType.FitContent,
											VerticalSizeType = (int)UiElementSizeType.FitContent
										}
									]
								},
								..savedTileMapsRowModel
							]
						}
					]
				}
			];
		}
	}
}

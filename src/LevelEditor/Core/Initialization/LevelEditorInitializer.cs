using BaseContent.BaseContentConstants.Fonts;
using BaseContent.BaseContentConstants.Images;
using Common.Controls.CursorInteraction.Models;
using Common.Controls.Cursors.Constants;
using Common.Controls.Cursors.Models;
using Common.DiskModels.UserInterface;
using Common.DiskModels.UserInterface.Elements;
using Common.UserInterface.Enums;
using Common.UserInterface.Models.Contracts;
using Engine.DiskModels.Drawing;
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
	static public class LevelEditorInitializer
	{
		/// <summary>
		/// Gets the function providers. 
		/// </summary>
		/// <param name="gameServices">The game service.</param>
		/// <returns>The function providers.</returns>
		static public Dictionary<string, Delegate> GetFunctionProviders(GameServiceContainer gameServices)
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
		static public Dictionary<string, Action<Cursor, GameTime>> GetCursorUpdaters(GameServiceContainer gameService)
		{
			var spritesheetButtonService = gameService.GetService<ISpritesheetButtonService>();


			var dict = new Dictionary<string, Action<Cursor, GameTime>>
			{
				[LevelEditorCursorUpdatersNames.SpritesheetButtonCursorUpdater] = spritesheetButtonService.SpritesheetButtonCursorUpdater,
			};

			return dict;
		}

		/// <summary>
		/// Gets the hover event processors.
		/// </summary>
		/// <param name="gameServices">The game services.</param>
		/// <returns>A dictionary of the hover event processors.</returns>
		static public Dictionary<string, Action<CursorInteraction<IAmAUiElement>>> GetHoverEventProcessors(GameServiceContainer gameServices)
		{
			var spritesheetButtonService = gameServices.GetService<ISpritesheetButtonService>();

			var dict = new Dictionary<string, Action<CursorInteraction<IAmAUiElement>>>
			{

			};

			return dict;
		}

		/// <summary>
		/// Gets the press event processors.
		/// </summary>
		/// <param name="gameServices">The game services.</param>
		/// <returns>A dictionary of the press event processors.</returns>
		static public Dictionary<string, Action<CursorInteraction<IAmAUiElement>>> GetPressEventProcessors(GameServiceContainer gameServices)
		{
			var dict = new Dictionary<string, Action<CursorInteraction<IAmAUiElement>>>
			{

			};

			return dict;
		}

		/// <summary>
		/// Gets the click event processors.
		/// </summary>
		/// <param name="gameServices">The game services.</param>
		/// <returns>A dictionary of the click event processors.</returns>
		static public Dictionary<string, Action<CursorInteraction<IAmAUiElement>>> GetClickEventProcessors(GameServiceContainer gameServices)
		{
			var spritesheetButtonService = gameServices.GetService<ISpritesheetButtonService>();
			var sceneEditService = gameServices.GetService<ISceneEditService>();

			var dict = new Dictionary<string, Action<CursorInteraction<IAmAUiElement>>>
			{
				[UiEventName.SpritesheetButtonClick] = spritesheetButtonService.SpritesheetButtonClickEventProcessor,
				[UiEventName.CreateSceneClick] = sceneEditService.CreateSceneButtonClickEventProcessor,
				[UiEventName.SaveSceneClick] = sceneEditService.SaveScene,
				[UiEventName.ToggleTileGridClick] = sceneEditService.ToggleTileGridClickEventProcessor,
				[UiEventName.LoadSceneClick] = sceneEditService.LoadSceneButtonClickEventProcessor,
			};

			return dict;
		}

		/// <summary>
		/// Gets the initial user interface models.
		/// </summary>
		/// <param name="gameServices">The game services.</param>
		/// <returns>The user interface models.</returns>
		static public IList<object> GetInitialUiModels(GameServiceContainer gameServices)
		{
			var spritesheetButtonService = gameServices.GetService<ISpritesheetButtonService>();
			var sceneEditService = gameServices.GetService<ISceneEditService>();

			var spritesheetButtons = spritesheetButtonService.GetUiButtonsForSpritesheet("dark_grass_simplified", new Point(32, 32));
			var flattenedButtons = spritesheetButtons?.SelectMany(row => row).ToArray();
			var savedTileMapsRowModel = sceneEditService.GetSavedTileMapUserInterfaceRows();

			return
			[
				new UiModalModel
				{
					Name = "Level Editor Label Modal",
					ResizeTexture = true,
					HorizontalJustificationType = UiHorizontalJustificationType.Center,
					VerticalJustificationType = UiVerticalJustificationType.Center,
					HorizontalModalSizeType = UiModalSizeType.Medium,
					VerticalModalSizeType = UiModalSizeType.Medium,
					BackgroundTexture = new SimpleImageModel
					{
						TextureName = "pallet",
						TextureRegion = new TextureRegionModel
						{
							TextureRegionType = TextureRegionType.Fill,
							TextureBox = PalletColorToTextureBoxHelper.GetPalletColorTextureBox(PalletColors.Hex_C7CFDD)
						}
					},
					Blocks =
					[
						new UiBlockModel
						{
							HorizontalJustificationType = UiHorizontalJustificationType.Center,
							VerticalJustificationType = UiVerticalJustificationType.Center,
							Rows =
							[
								new UiRowModel
								{
									Name = "Level Editor Label Row",
									ResizeTexture = true,
									HorizontalJustificationType = UiHorizontalJustificationType.Center,
									VerticalJustificationType = UiVerticalJustificationType.Center,
									Elements =
									[
										new UiTextModel
										{
											Name = "Level Editor Label Element",
											HoverCursorName = CommonCursorNames.BasicCursorName,
											Margin = new UiMarginModel
											{
												LeftMargin = 10,
												RightMargin = 10,
											},
											HorizontalSizeType = UiElementSizeType.FlexMin,
											VerticalSizeType = UiElementSizeType.FlexMin,
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
						}
					]
				},
			];

			return
			[
				new UiGroupModel
				{
					Name = "Level Editor Main UI",
					VisibilityGroupId = 1,
					IsVisible = true,
					Zones =
					[
						new UiZoneModel
						{
							Name = "Level Editor Label Row",
							UiZonePositionType = UiZonePositionType.Row1Col4,
							BackgroundTexture = null,
							VerticalJustificationType = UiVerticalJustificationType.Top,
							Blocks =
							[
								new UiBlockModel
								{
									Rows =
									[
										new UiRowModel
										{
											Name = "Level Editor Label Row",
											ResizeTexture = true,
											HorizontalJustificationType = UiHorizontalJustificationType.Right,
											VerticalJustificationType = UiVerticalJustificationType.Center,
											Elements =
											[
												new UiTextModel
												{
													Name = "Level Editor Label Element",
													HoverCursorName = CommonCursorNames.BasicCursorName,
													Margin = new UiMarginModel
													{
														LeftMargin = 10,
														RightMargin = 10,
													},
													HorizontalSizeType = UiElementSizeType.FlexMin,
													VerticalSizeType = UiElementSizeType.FlexMin,
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
								}
							]
						},
						new UiZoneModel
						{
							Name = "Disc Zone",
							UiZonePositionType = UiZonePositionType.Row3Col1,
							BackgroundTexture = null,
							VerticalJustificationType = UiVerticalJustificationType.Top,
							HoverCursorName = CommonCursorNames.BasicCursorName,
							Blocks =
							[
								new UiBlockModel
								{
									FlexRows = true,
									Rows =
									[
										new UiRowModel
										{
											Name = "Create Level Row",
											ExtendBackgroundToMargin = true,
											Margin = new UiMarginModel
											{
												TopMargin = 20,
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
											HoverCursorName = CommonCursorNames.BasicCursorName,
											HorizontalJustificationType = UiHorizontalJustificationType.Left,
											VerticalJustificationType = UiVerticalJustificationType.Center,
											Elements =
											[
												new UiTextModel
												{
													Name = "Create Level Label",
													Margin = new UiMarginModel
													{
														LeftMargin = 10,
														RightMargin = 5,
													},
													Text = new GraphicalTextModel
													{
														Text = "Create Level",
														TextColor = PalletColors.Hex_BF6F4A,
														FontName = FontNames.MonoBold
													},
													HorizontalSizeType = UiElementSizeType.FlexMin,
													VerticalSizeType = UiElementSizeType.FlexMin
												},
												new UiButtonModel
												{
													Name = "Create Level Button",
													HorizontalSizeType = UiElementSizeType.FlexMin,
													VerticalSizeType = UiElementSizeType.FlexMin,
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
												}
											]
										},
										new UiRowModel
										{
											Name = "Save Level Row",
											ExtendBackgroundToMargin = true,
											Margin = new UiMarginModel
											{
												TopMargin = 20,
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
											HoverCursorName = CommonCursorNames.BasicCursorName,
											HorizontalJustificationType = UiHorizontalJustificationType.Left,
											VerticalJustificationType = UiVerticalJustificationType.Center,
											Elements =
											[
												new UiTextModel
												{
													Name = "Save Element Label",
													Margin = new UiMarginModel
													{
														LeftMargin = 10,
														RightMargin = 5,
													},
													Text = new GraphicalTextModel
													{
														Text = "Save Level",
														TextColor = PalletColors.Hex_BF6F4A,
														FontName = FontNames.MonoBold
													},
													HorizontalSizeType = UiElementSizeType.FlexMin,
													VerticalSizeType = UiElementSizeType.FlexMin
												},
												new UiButtonModel
												{
													Name = "Save Level Button",
													HorizontalSizeType = UiElementSizeType.FlexMin,
													VerticalSizeType = UiElementSizeType.FlexMin,
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
										}
									]
								},
								new UiBlockModel
								{
									FlexRows = true,
									Rows =
									[
										new UiRowModel
										{
											Name = "Levels Header",
											ExtendBackgroundToMargin = true,
											Margin = new UiMarginModel
											{
												TopMargin = 10,
												BottomMargin = 10
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
											HoverCursorName = CommonCursorNames.BasicCursorName,
											HorizontalJustificationType = UiHorizontalJustificationType.Center,
											VerticalJustificationType = UiVerticalJustificationType.Center,
											Elements =
											[
												new UiTextModel
												{
													Name = "Saved Levels Element",
													Text = new GraphicalTextModel
													{
														Text = "Saved Levels",
														TextColor = PalletColors.Hex_BF6F4A,
														FontName = FontNames.MonoBold
													},
													HorizontalSizeType = UiElementSizeType.FlexMin,
													VerticalSizeType = UiElementSizeType.FlexMin
												}
											]
										},
										..savedTileMapsRowModel
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

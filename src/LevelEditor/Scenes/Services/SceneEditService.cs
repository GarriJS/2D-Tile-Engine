using BaseContent.BaseContentConstants.Fonts;
using BaseContent.BaseContentConstants.Images;
using Common.Controls.CursorInteraction.Models;
using Common.Controls.Cursors.Constants;
using Common.DiskModels.Tiling;
using Common.DiskModels.Tiling.Options;
using Common.DiskModels.UserInterface;
using Common.DiskModels.UserInterface.Elements;
using Common.Scenes.Models;
using Common.Tiling.Models;
using Common.Tiling.Services.Contracts;
using Common.UserInterface.Enums;
using Common.UserInterface.Models;
using Common.UserInterface.Models.Contracts;
using Common.UserInterface.Services.Contracts;
using Engine.Controls.Services.Contracts;
using Engine.Core.Files.Services.Contracts;
using Engine.DiskModels;
using Engine.DiskModels.Drawing;
using Engine.DiskModels.Physics;
using Engine.Graphics.Enum;
using Engine.Physics.Services.Contracts;
using Engine.RunTime.Services.Contracts;
using LevelEditor.Controls.Contexts;
using LevelEditor.Core.Constants;
using LevelEditor.LevelEditorContent;
using LevelEditor.LevelEditorContent.Images.Manifests;
using LevelEditor.Scenes.Models;
using LevelEditor.Scenes.Services.Contracts;
using LevelEditor.Spritesheets.Services.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LevelEditor.Scenes.Services
{
	/// <summary>
	/// Represents a scene edit service.
	/// </summary>
	/// <remarks>
	/// Initializes a new instance of the scene edit service.
	/// </remarks>
	/// <param name="gameServices"></param>
	public class SceneEditService(GameServiceContainer gameServices) : ISceneEditService
	{
		readonly private GameServiceContainer _gameServices = gameServices;

		/// <summary>
		/// Gets the current scene.
		/// </summary>
		public Scene CurrentScene { get; private set; }

		/// <summary>
		/// Gets the add tile Block.
		/// </summary>
		public AddTileComponent AddTileComponent { get; private set; }

		/// <summary>
		/// Loads the content.
		/// </summary>
		public void LoadContent()
		{
			this.AddTileComponent = new AddTileComponent(this._gameServices);
		}

		/// <summary>
		/// The create scene button click event processor.
		/// </summary>
		/// <param name="cursorInteraction">The cursor interaction.</param>
		public void CreateSceneButtonClickEventProcessor(CursorInteraction<IAmAUiElement> cursorInteraction)
		{
			_ = this.CreateNewScene(setCurrent: true, cursorInteraction.Subject.Name);
		}

		/// <summary>
		/// The load scene button click event processor.
		/// </summary>
		/// <param name="cursorInteraction">The cursor interaction.</param>
		public void LoadSceneButtonClickEventProcessor(CursorInteraction<IAmAUiElement> cursorInteraction)
		{
			var tileMapModel = this.LoadTileMapModel(cursorInteraction.Subject.Name);

			if (tileMapModel is null)
				return;

			var tileService = this._gameServices.GetService<ITileService>();
			var tileMap = tileService.GetTileMapFromModel(tileMapModel);
			this.CreateNewScene(true, tileMap, cursorInteraction.Subject.Name);
		}

		/// <summary>
		/// The toggle tile grid click event processor.
		/// </summary>
		/// <param name="cursorInteraction">The cursor interaction.</param>
		public void ToggleTileGridClickEventProcessor(CursorInteraction<IAmAUiElement> cursorInteraction)
		{
			this.AddTileComponent.ToggleBackgroundGraphic();
		}

		/// <summary>
		/// Gets the tile grid user interface zone.
		/// </summary>
		/// <returns>The user interface zone.</returns>
		public UiZone GetTileGridUserInterfaceZone()
		{
			var uiZoneService = this._gameServices.GetService<IUserInterfaceZoneService>();
			var uiZoneModel = new UiZoneModel
			{
				Name = "Tile Grid User Interface Zone",
				UiZonePositionType = UiZonePositionType.Row3Col3,
				BackgroundTexture = null,
				VerticalJustificationType = UiVerticalJustificationType.Bottom,
				Blocks =
				[
					new UiBlockModel
					{
						Rows =
						[
							new UiRowModel
							{
								Name = "Toggle Tile Grid Row",
								Margin = new UiMarginModel
								{
									TopMargin = 10,
									BottomMargin = 5,
								},
								HoverCursorName = CommonCursorNames.BasicCursorName,
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
								HorizontalJustificationType =  UiHorizontalJustificationType.Right,
								VerticalJustificationType = UiVerticalJustificationType.Center,
								Elements =
								[
									new UiTextModel
									{
										Name = "Create Element Label",
										Margin = new UiMarginModel
										{
											RightMargin = 5
										},
										Text = new GraphicalTextModel
										{
											Text = "Toggle Tile Grid",
											TextColor = PalletColors.Hex_BF6F4A,
											FontName = FontNames.MonoBold
										},
										HorizontalSizeType = UiElementSizeType.FlexMin,
										VerticalSizeType = UiElementSizeType.FlexMin,
									},
									new UiButtonModel
									{
										Name = "Toggle Tile Grid Button",
										Margin = new UiMarginModel
										{
											RightMargin = 10
										},
										HorizontalSizeType = UiElementSizeType.FlexMin,
										VerticalSizeType = UiElementSizeType.FlexMin,
										ClickableAreaAnimation = new TriggeredAnimationModel
										{
											CurrentFrameIndex = 0,
											FrameDuration = 500,
											Frames =
											[
												DarkBlueButtonsManifest.UnpressedTabButton,
												DarkBlueButtonsManifest.PressedTabButton
											],
											RestingFrameIndex = 0,
										},
										ClickableAreaScaler = new Vector2
										{
											X = 1,
											Y = 1
										},
										ClickEventName = UiEventName.ToggleTileGridClick
									},
								]
							}
						]
					}
				]
			};

			var uiZone = uiZoneService.GetUiZoneFromModel(uiZoneModel);

			return uiZone;
		}

		/// <summary>
		/// Gets the saved tile map user interface rows.
		/// </summary>
		/// <returns>The saved tile map user interface rows.</returns>
		public UiRowModel[] GetSavedTileMapUserInterfaceRows()
		{
			var savedTileMapNames = this.GetSavedTileMapNames();
			var uiRowModels = new UiRowModel[savedTileMapNames.Length];

			for (int i = 0; i < savedTileMapNames.Length; i++)
			{
				var rowModel = new UiRowModel
				{
					Name = $"{savedTileMapNames[i]} row",
					ResizeTexture = true,
					Margin = new UiMarginModel
					{
						TopMargin = 0,
						BottomMargin = 0,
					},
					HorizontalJustificationType = UiHorizontalJustificationType.Center,
					VerticalJustificationType = UiVerticalJustificationType.Center,
					BackgroundTexture = new SimpleImageModel
					{
						TextureName = "pallet",
						TextureRegion = new TextureRegionModel
						{
							TextureRegionType = TextureRegionType.Fill,
							TextureBox = PalletColorToTextureBoxHelper.GetPalletColorTextureBox(PalletColors.Hex_C7CFDD)
						}
					},
					Elements =
					[
						new UiButtonModel
						{
							Name = savedTileMapNames[i],
							ResizeTexture = true,
							HorizontalSizeType = UiElementSizeType.FlexMin,
							VerticalSizeType = UiElementSizeType.FlexMin,
							Margin = new UiMarginModel
							{
								TopMargin = 5,
								BottomMargin = 5,
							},
							ClickableAreaAnimation = new TriggeredAnimationModel
							{
								CurrentFrameIndex = 0,
								FrameDuration = 500,
								Frames =
								[
									DarkBlueButtonsManifest.GetUnpressedCompositeEmptyButton(75, 50),
									DarkBlueButtonsManifest.GetPressedCompositeEmptyButton(75, 50),
								],
								RestingFrameIndex = 0
							},
							Text = new GraphicalTextWithMarginModel
							{
								Text = savedTileMapNames[i],
								TextColor = PalletColors.Hex_BF6F4A,
								FontName = FontNames.MonoBold,
								Margin = new UiMarginModel
								{ 
									TopMargin = 10,
									BottomMargin = 15,
									LeftMargin = 50,
									RightMargin = 50
								}
							},
							ClickableAreaScaler = new Vector2
							{
								X = 1,
								Y = 1
							},
							ClickEventName = UiEventName.LoadSceneClick
						}
					]
				};

				uiRowModels[i] = rowModel;
			}

			return uiRowModels;
		}

		/// <summary>
		/// Creates a new scene.
		/// </summary>
		/// <param name="setCurrent">A value indicating whether to set the new scene as the current scene.</param>
		/// <param name="sceneName">The scene name.</param>
		/// <returns>The new scene.</returns>
		public Scene CreateNewScene(bool setCurrent, string sceneName = null)
		{
			var areaService = this._gameServices.GetService<IAreaService>();
			var areaModel = new AreaModel
			{
				Position = new PositionModel
				{
					X = 0,
					Y = 0,
				},
				Width = 0,
				Height = 0,
			};
			var area = areaService.GetAreaFromModel(areaModel);
			var tileMap = new TileMap
			{
				TileMapName = $"Test TileMap",
				Area = area,
				DrawLayer = 1,
			};
			var result = this.CreateNewScene(setCurrent, tileMap, sceneName);

			return result;
		}

		/// <summary>
		/// Creates a new scene.
		/// </summary>
		/// <param name="setCurrent">A value indicating whether to set the new scene as the current scene.</param>
		/// <param name="tileMap">The tile map of the scene.</param>
		/// <param name="sceneName">The scene name.</param>
		/// <returns>The new scene.</returns>
		public Scene CreateNewScene(bool setCurrent, TileMap tileMap, string sceneName = null)
		{
			var scene = new Scene
			{
				TileMap = tileMap,
				SceneName = sceneName,
			};

			if (true == setCurrent)
				this.SetCurrentScene(scene);

			return scene;
		}

		/// <summary>
		/// Sets the current scene.
		/// </summary>
		/// <param name="scene"></param>
		public void SetCurrentScene(Scene scene)
		{
			var runTimeDrawService = this._gameServices.GetService<IRuntimeDrawService>();
			var spritesheetButtonService = this._gameServices.GetService<ISpritesheetButtonService>();
			var uiGroupService = this._gameServices.GetService<IUserInterfaceGroupService>();
			var controlService = this._gameServices.GetService<IControlService>();
			var graphicDeviceService = this._gameServices.GetService<IGraphicsDeviceService>();
			var spritesheetButtonUiZone = spritesheetButtonService.GetUiZoneForSpritesheet("dark_grass_simplified", "gray_transparent", UiZonePositionType.Row3Col4);
			var tileGridUserInterfaceZone = this.GetTileGridUserInterfaceZone();
			uiGroupService.AddUserInterfaceZoneToUserInterfaceGroup(visibilityGroupId: 1, spritesheetButtonUiZone);
			uiGroupService.AddUserInterfaceZoneToUserInterfaceGroup(visibilityGroupId: 1, tileGridUserInterfaceZone);
			controlService.ControlContext = new SceneEditControlContext(this._gameServices);
			runTimeDrawService.AddDrawable(scene.TileMap);
			var simpleImageModel = new SimpleImageModel
			{
				TextureName = "tile_grid_light",
				TextureRegion = new TextureRegionModel
				{
					TextureRegionType = TextureRegionType.Tile,
					TextureBox = new Rectangle
					{
						X = 0,
						Y = 0,
						Width = 320,
						Height = 320
					},
					DisplayArea = new SubAreaModel
					{
						Width = graphicDeviceService.GraphicsDevice.Viewport.Width,
						Height = graphicDeviceService.GraphicsDevice.Viewport.Height
					}
				}
			};
			this.AddTileComponent.SetBackgroundGraphic(simpleImageModel);

			if (false == this.AddTileComponent.BackgroundGraphicActive)
				this.AddTileComponent.ToggleBackgroundGraphic();

			this.CurrentScene = scene;
		}

		/// <summary>
		/// Saves the scene.
		/// </summary>
		/// <param name="cursorInteraction">The cursor interaction.</param>
		public void SaveScene(CursorInteraction<IAmAUiElement> cursorInteraction)
		{
			if (this.CurrentScene is null)
				return;

			var jsonService = this._gameServices.GetService<IJsonService>();
			var tileMapName = "TestMap";
			var filePath = jsonService.GetJsonFilePath(ContentManagerParams.ContentManagerName, "TileMaps", tileMapName, createDirectoryIfDoesNotExist: true);
			var serializer = new ModelSerializer<TileMapModel>();
			var tileMapModel = this.CurrentScene.TileMap.ToModel();
			serializer.Serialize(filePath, tileMapModel, TilingOptions.TileMapOptions);
		}

		/// <summary>
		/// Loads the tile map model.
		/// </summary>
		/// <param name="tileMapName">The tile map name.</param>
		/// <returns>The tile map model.</returns>
		public TileMapModel LoadTileMapModel(string tileMapName)
		{
			var jsonService = this._gameServices.GetService<IJsonService>();
			var serializer = new ModelSerializer<TileMapModel>();
			using var tileMapStream = jsonService.GetJsonFileStream(ContentManagerParams.ContentManagerName, "TileMaps", tileMapName);
			var tileMapModel = serializer.Deserialize(tileMapStream, TilingOptions.TileMapOptions);

			return tileMapModel;
		}

		/// <summary>
		/// Gets the saved tile map names.
		/// </summary>
		/// <returns></returns>
		private string[] GetSavedTileMapNames()
		{
			var jsonService = this._gameServices.GetService<IJsonService>();
			var result = jsonService.GetJsonFileNames(ContentManagerParams.ContentManagerName, "TileMaps");

			return result;
		}
	}
}

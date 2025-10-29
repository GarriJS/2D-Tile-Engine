using BaseContent.BaseContentConstants.Fonts;
using BaseContent.BaseContentConstants.Images;
using Common.Controls.Cursors.Constants;
using Common.DiskModels.Tiling;
using Common.DiskModels.Tiling.Options;
using Common.DiskModels.UI;
using Common.DiskModels.UI.Elements;
using Common.Scenes.Models;
using Common.Tiling.Models;
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
		private readonly GameServiceContainer _gameServices = gameServices;

		/// <summary>
		/// Gets the current scene.
		/// </summary>
		public Scene CurrentScene { get; private set; }

		/// <summary>
		/// Gets the add tile component.
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
		/// <param name="element">The element.</param>
		/// <param name="elementLocation">The element location.</param>
		public void CreateSceneButtonClickEventProcessor(IAmAUiElement element, Vector2 elementLocation)
		{
			var runTimeDrawService = this._gameServices.GetService<IRuntimeDrawService>();
			var spritesheetButtonService = this._gameServices.GetService<ISpritesheetButtonService>();
			var uiService = this._gameServices.GetService<IUserInterfaceService>();
			var controlService = this._gameServices.GetService<IControlService>();
			var graphicDeviceService = this._gameServices.GetService<IGraphicsDeviceService>();

			var scene = this.CreateNewScene(setCurrent: true);
			var spritesheetButtonUiZone = spritesheetButtonService.GetUiZoneForSpritesheet("dark_grass_simplified", "gray_transparent", UiScreenZoneTypes.Row3Col4);
			var tileGridUserInterfaceZone = this.GetTileGridUserInterfaceZone();
			uiService.AddUserInterfaceZoneToUserInterfaceGroup(visibilityGroupId: 1, spritesheetButtonUiZone);
			uiService.AddUserInterfaceZoneToUserInterfaceGroup(visibilityGroupId: 1, tileGridUserInterfaceZone);
			controlService.ControlContext = new SceneEditControlContext(this._gameServices);
			runTimeDrawService.AddDrawable(scene.TileMap);
			var textureRegionImageModel = new SimpleImageModel
			{
				TextureName = "tile_grid_light",
				TextureRegion = new TextureRegionModel
				{
					TextureRegionType = TextureRegionType.Fill,
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
			this.AddTileComponent.SetBackgroundGraphic(textureRegionImageModel);

			if (false == this.AddTileComponent.BackgroundGraphicActive)
			{
				this.AddTileComponent.ToggleBackgroundGraphic();
			}
		}

		/// <summary>
		/// The toggle tile grid click event processor.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <param name="elementLocation">The element location.</param>
		public void ToggleTileGridClickEventProcessor(IAmAUiElement element, Vector2 elementLocation)
		{
			this.AddTileComponent.ToggleBackgroundGraphic();
		}

		/// <summary>
		/// Gets the tile grid user interface zone.
		/// </summary>
		/// <returns>The user interface zone.</returns>
		public UiZone GetTileGridUserInterfaceZone()
		{
			var uiService = this._gameServices.GetService<IUserInterfaceService>();

			var uiZoneModel = new UiZoneModel
			{
				UiZoneName = "Tile Grid User Interface Zone",
				UiZoneType = (int)UiScreenZoneTypes.Row3Col3,
				BackgroundTexture = null,
				JustificationType = (int)UiZoneJustificationTypes.Bottom,
				ElementRows =
				[
					new UiRowModel
					{
						UiRowName = "Toggle Tile Grid Row",
						InsidePadding = new UiPaddingModel
						{
							TopPadding = 10,
							BottomPadding = 5,
						},
						RowHoverCursorName = CommonCursorNames.BasicCursorName,
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
						HorizontalJustificationType =  UiRowHorizontalJustificationType.Right,
						VerticalJustificationType = UiRowVerticalJustificationType.Center,
						SubElements =
						[
							new UiTextModel
							{
								UiElementName = "Create Element Label",
								InsidePadding = new UiPaddingModel
								{
									RightPadding = 5
								},
								Text = new GraphicalTextModel
								{
									Text = "Toggle Tile Grid",
									TextColor = PalletColors.Hex_BF6F4A,
									FontName = FontNames.MonoBold
								},
								HorizontalSizeType = UiElementSizeType.FitContent,
								VerticalSizeType = UiElementSizeType.FitContent,
							},
							new UiButtonModel
							{
								UiElementName = "Toggle Tile Grid Button",
								InsidePadding = new UiPaddingModel
								{
									RightPadding = 10
								},
								HorizontalSizeType = UiElementSizeType.FitContent,
								VerticalSizeType = UiElementSizeType.FitContent,
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
								ClickEventName = UiEventName.ToggleTileGrid
							},
						]
					}
				]
			};

			var uiZone = uiService.GetUiZone(uiZoneModel);

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
					UiRowName = $"{savedTileMapNames[i]} row",
					ResizeTexture = true,
					InsidePadding = new UiPaddingModel
					{
						TopPadding = 10,
						BottomPadding = 10,
					},
					HorizontalJustificationType = UiRowHorizontalJustificationType.Center,
					VerticalJustificationType = UiRowVerticalJustificationType.Center,
					BackgroundTexture = new SimpleImageModel
					{
						TextureName = "pallet",
						TextureRegion = new TextureRegionModel
						{
							TextureRegionType = TextureRegionType.Fill,
							TextureBox = PalletColorToTextureBoxHelper.GetPalletColorTextureBox(PalletColors.Hex_C7CFDD)
						}
					},
					SubElements =
					[
						new UiButtonModel
						{
							UiElementName = savedTileMapNames[i],
							ResizeTexture = true,
							HorizontalSizeType = UiElementSizeType.FitContent,
							VerticalSizeType = UiElementSizeType.FitContent,
							InsidePadding = new UiPaddingModel
							{
								TopPadding = 10,
								BottomPadding = 10,
							},
							Graphic = new SimpleImageModel
							{
								TextureName = "pallet",
								TextureRegion = new TextureRegionModel
								{
									TextureRegionType = TextureRegionType.Fill,
									TextureBox = PalletColorToTextureBoxHelper.GetPalletColorTextureBox(PalletColors.Hex_1A1932)
								}
							},
							Text = new GraphicalTextModel
							{
								Text = savedTileMapNames[i],
								TextColor = PalletColors.Hex_3D3D3D,
								FontName = FontNames.MonoBold
							},
							//ClickEventName = 
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
			var scene = new Scene
			{
				TileMap = tileMap,
				SceneName = sceneName,
			};

			if (true == setCurrent)
			{
				this.CurrentScene = scene;
			}

			return scene;
		}

		/// <summary>
		/// Saves the scene.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <param name="elementLocation">The element location.</param>
		public void SaveScene(IAmAUiElement element, Vector2 elementLocation)
		{
			if (null == this.CurrentScene)
			{
				return;
			}

			var jsonService = this._gameServices.GetService<IJsonService>();

			var filePath = jsonService.GetJsonFilePath(ContentManagerParams.ContentManagerName, "TileMaps", "TestMap", createDirectoryIfDoesNotExist: true);
			var serializer = new ModelSerializer<TileMapModel>();
			var tileMapModel = this.CurrentScene.TileMap.ToModel();
			serializer.Serialize(filePath, tileMapModel, TilingOptions.TileMapOptions);
		}

		/// <summary>
		/// Gets the saved tile map names.
		/// </summary>
		/// <returns></returns>
		public string[] GetSavedTileMapNames()
		{
			var jsonService = this._gameServices.GetService<IJsonService>();

			return jsonService.GetJsonFileNames(ContentManagerParams.ContentManagerName, "TileMaps");
		}
	}
}

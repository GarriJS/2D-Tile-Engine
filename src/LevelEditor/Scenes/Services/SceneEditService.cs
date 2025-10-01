using BaseContent.BaseContentConstants.Fonts;
using BaseContent.BaseContentConstants.Images;
using Common.Controls.Cursors.Constants;
using Common.DiskModels.UI;
using Common.DiskModels.UI.Elements;
using Common.Scenes.Models;
using Common.Tiling.Models;
using Common.UserInterface.Enums;
using Common.UserInterface.Models;
using Common.UserInterface.Models.Contracts;
using Common.UserInterface.Services.Contracts;
using Engine.Controls.Services.Contracts;
using Engine.DiskModels.Drawing;
using Engine.DiskModels.Physics;
using Engine.Physics.Services.Contracts;
using Engine.RunTime.Services.Contracts;
using LevelEditor.Controls.Contexts;
using LevelEditor.Core.Constants;
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
			var fillImageModel = new TiledImageModel
			{
				TextureName = "tile_grid_light",
				TextureBox = new Rectangle
				{
					X = 0,
					Y = 0,
					Width = 320,
					Height = 320
				},
				FillBox = new Vector2
				{
					X = graphicDeviceService.GraphicsDevice.Viewport.Width,
					Y = graphicDeviceService.GraphicsDevice.Viewport.Height
				}
			};
			this.AddTileComponent.SetBackgroundGraphic(fillImageModel);

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
						TopPadding = 10,
						BottomPadding = 5,
						RowHoverCursorName = CommonCursorNames.BasicCursorName,               
						ResizeTexture = true,
						BackgroundTexture = new FillImageModel
						{
							TextureName = "pallet",
							TextureBox = PalletColorToTextureBoxHelper.GetPalletColorTextureBox(PalletColors.Hex_C7CFDD)
						},
						HorizontalJustificationType =  (int)UiRowHorizontalJustificationTypes.Right,
						VerticalJustificationType = (int)UiRowVerticalJustificationTypes.Center,
						SubElements =
						[
							new UiTextModel
							{
								UiElementName = "Create Element Label",
								RightPadding = 5,
								Text = new GraphicalTextModel
								{
									Text = "Toggle Tile Grid",
									TextColor = PalletColors.Hex_BF6F4A,
									FontName = FontNames.MonoBold
								},
								SizeType = (int)UiElementSizeTypes.Fit
							},
							new UiButtonModel
							{
								UiElementName = "Toggle Tile Grid Button",
								RightPadding = 10,
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
												Y = 128,
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
												Y = 128,
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
		/// Creates a new scene.
		/// </summary>
		/// <param name="setCurrent">A value indicating whether to set the new scene as the current scene.</param>
		/// <param name="sceneName">The scene name.</param>
		/// <returns>The new scene.</returns>
		public Scene CreateNewScene(bool setCurrent, string sceneName = null)
		{
			var areaService = this._gameServices.GetService<IAreaService>();

			var areaModel = new SimpleAreaModel
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
				TileMapName = $"{sceneName} TileMap",
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
	}
}

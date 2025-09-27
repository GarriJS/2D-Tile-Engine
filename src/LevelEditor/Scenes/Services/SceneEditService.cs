using Common.Scenes.Models;
using Common.Tiling.Models;
using Common.UserInterface.Enums;
using Common.UserInterface.Models.Contracts;
using Common.UserInterface.Services.Contracts;
using Engine.Controls.Services.Contracts;
using Engine.DiskModels.Physics;
using Engine.Physics.Models;
using Engine.Physics.Services.Contracts;
using Engine.RunTime.Services.Contracts;
using LevelEditor.Controls.Contexts;
using LevelEditor.Scenes.Models;
using LevelEditor.Scenes.Services.Contracts;
using LevelEditor.Spritesheets.Services.Contracts;
using Microsoft.Xna.Framework;

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

			var scene = this.CreateNewScene(setCurrent: true);
			var spritesheetButtonUiZone = spritesheetButtonService.GetUiZoneForSpritesheet("dark_grass_simplified", "gray_transparent", UiScreenZoneTypes.Row3Col4);
			uiService.AddUserInterfaceZoneToUserInterfaceGroup(visibilityGroupId: 1, spritesheetButtonUiZone);
			controlService.ControlContext = new SceneEditControlContext(this._gameServices);
			runTimeDrawService.AddDrawable(scene.TileMap);
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

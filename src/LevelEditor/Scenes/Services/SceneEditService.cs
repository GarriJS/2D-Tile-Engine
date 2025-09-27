using Common.Scenes.Models;
using Common.Tiling.Models;
using Common.UserInterface.Models.Contracts;
using LevelEditor.Scenes.Services.Contracts;
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
		/// The create scene button click event processor.
		/// </summary>
		/// <param name="element">The element.</param>
		/// <param name="elementLocation">The element location.</param>
		public void CreateSceneButtonClickEventProcessor(IAmAUiElement element, Vector2 elementLocation)
		{
			_ = this.CreateNewScene(setCurrent: true);
		}

		/// <summary>
		/// Creates a new scene.
		/// </summary>
		/// <param name="setCurrent">A value indicating whether to set the new scene as the current scene.</param>
		/// <param name="sceneName">The scene name.</param>
		/// <returns>The new scene.</returns>
		public Scene CreateNewScene(bool setCurrent, string sceneName = null)
		{
			var tileMap = new TileMap
			{
				TileMapName = $"{sceneName} TileMap",
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

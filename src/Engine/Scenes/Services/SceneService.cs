using DiscModels.Engine.Scenes;
using Engine.Core.Contracts;
using Engine.Scenes.Models;
using Engine.Tiling.Services.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Engine.Scenes.Services
{
	/// <summary>
	/// Represents a scene service.
	/// </summary>
	/// <remarks>
	/// Initializes a new instance of the scene service.
	/// </remarks>
	/// <param name="gameServices"></param>
	public class SceneService(GameServiceContainer gameServices) : ILoadContent
	{
		private readonly GameServiceContainer _gameServices = gameServices;

		/// <summary>
		/// Gets or sets the scene models.
		/// </summary>
		public Dictionary<string, SceneModel> SceneModels { get; set; } = [];

		/// <summary>
		/// Loads the scene content.
		/// </summary>
		public void LoadContent()
		{
			var contentManager = this._gameServices.GetService<ContentManager>();
			var scenePath = $@"{contentManager.RootDirectory}\Scenes";
			string[] sceneFiles = Directory.GetFiles(scenePath);

			if (false == sceneFiles.Any())
			{
				return;
			}

			foreach (string sceneFile in sceneFiles)
			{
				try
				{
					var sceneName = Path.GetFileNameWithoutExtension(sceneFile);
					var scene = contentManager.Load<SceneModel>($@"Scenes\{sceneName}");
					this.SceneModels.Add(scene.SceneName, scene);
				}
				catch (Exception ex)
				{
					Debug.WriteLine($"Loading images failed for {sceneFile}: {ex.Message}");
				}
			}
		}

		/// <summary>
		/// Gets the scene.
		/// </summary>
		/// <param name="scene">The scene model.</param>
		/// <returns>The scene.</returns>
		public Scene GetScene(SceneModel scene)
		{
			var tileService = this._gameServices.GetService<ITileService>();
			var tileMap = tileService.GetTileMap(scene.TileMap);

			return new Scene
			{
				SceneName = scene.SceneName,
				TileMap = tileMap,
			};
		}
	}
}

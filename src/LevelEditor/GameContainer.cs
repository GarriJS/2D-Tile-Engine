using Engine;
using Content;
using Engine.Core.Initialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace LevelEditor
{
	/// <summary>
	/// The game container.
	/// </summary>
	internal static class GameContainer
	{
		/// <summary>
		/// Gets or sets the game.
		/// </summary>
		internal static Game1 Game { get; set; }

		/// <summary>
		/// Gets the game services.
		/// </summary>
		internal static GameServiceContainer GameService { get => Game.Services; }

		/// <summary>
		/// Gets the launch settings.
		/// </summary>
		/// <returns>The launch settings.</returns>
		internal static LaunchSettings GetLaunchSettings()
		{ 
			var contentManager = new Dictionary<string, ContentManager>
			{
				{ Content.ContentExporter.ContentManagerName, Content.ContentExporter.ContentManager }
			};

			return new LaunchSettings
			{
				ContentManagers = contentManager
			};
		}
	}
}

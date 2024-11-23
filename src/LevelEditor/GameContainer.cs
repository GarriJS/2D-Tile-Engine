using Engine;
using Engine.Core.Initialization.Models;
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
		/// Gets the loading instructions.
		/// </summary>
		/// <param name="graphicsDeviceManager">The graphics device manager.</param>
		/// <returns>The loading instructions.</returns>
		internal static LoadingInstructions GetLoadingInstructions(GraphicsDeviceManager graphicsDeviceManager)
		{ 
			var basicContentManager = BaseContent.ContentExporter.InitializeContentManager(graphicsDeviceManager);
			var contentManager = new Dictionary<string, ContentManager>
			{
				{ BaseContent.ContentExporter.ContentManagerName, basicContentManager }
			};

			var spritesheetLinkages = new List<ContentManagerLinkage>();
			var spritesheetNames = BaseContent.ContentExporter.GetSpritesheetNames();
			
			foreach (var spritesheetName in spritesheetNames)
			{
				spritesheetLinkages.Add(new ContentManagerLinkage
				{
					ContentManagerName = BaseContent.ContentExporter.ContentManagerName,
					ContentName = spritesheetName
				});
			}

			return new LoadingInstructions
			{
				ContentManagers = contentManager,
				SpritesheetLinkages = spritesheetLinkages
			};
		}
	}
}

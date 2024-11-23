using Engine.Core.Initialization.Models;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Core.Initialization
{
	/// <summary>
	/// Represents a loading instructions container.
	/// </summary>
	internal static class LoadingInstructionsContainer
	{
		/// <summary>
		/// Gets or sets the loading instructions settings.
		/// </summary>
		internal static LoadingInstructions LoadingInstructions { get; set; }

		/// <summary>
		/// Tries to get the content manager.
		/// </summary>
		/// <param name="contentManagerName">The content manager name.</param>
		/// <param name="contentManager">The content manager.</param>
		/// <returns>A value indicating whether the content manager was returned.</returns>
		internal static bool TryGetContentManager(string contentManagerName, out ContentManager contentManager)
		{ 
			return LoadingInstructions.ContentManagers.TryGetValue(contentManagerName, out contentManager);
		}

		/// <summary>
		/// Gets the content manager names.
		/// </summary>
		/// <returns>A list of content manager names.</returns>
		internal static IList<string> GetContentManagerNames()
		{
			var contentList = LoadingInstructions?.TilesetLinkages?.Select(e => e.ContentManagerName)?
																   .ToList();

			return contentList ?? [];
		}

		/// <summary>
		/// Gets the tile set names for the content manager.
		/// </summary>
		/// <param name="contentManagerName">The content manager name.</param>
		/// <returns>A list of tile set names for the content manager name.</returns>
		internal static IList<string> GetTileSetNamesForContentManager(string contentManagerName)
		{
			var contentList = LoadingInstructions?.TilesetLinkages?.Where(e => e.ContentManagerName == contentManagerName)?
																   .Select(e => e.ContentName)?
																   .ToList();
			
			return contentList ?? [];	
		}

		/// <summary>
		/// Gets the image names for the content manager.
		/// </summary>
		/// <param name="contentManagerName">The content manager name.</param>
		/// <returns>A list of image names for the content manager name.</returns>
		internal static IList<string> GetImageNamesForContentManager(string contentManagerName)
		{
			var contentList = LoadingInstructions?.ImageLinkages?.Where(e => e.ContentManagerName == contentManagerName)?
																 .Select(e => e.ContentName)?
																 .ToList();

			return contentList ?? [];
		}
	}
}

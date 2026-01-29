using Engine.Core.Initialization.Models;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Core.Initialization
{
	/// <summary>
	/// Represents a loading instructions container.
	/// </summary>
	static internal class LoadingInstructionsContainer
	{
		/// <summary>
		/// Gets or sets the loading instructions settings.
		/// </summary>
		static internal LoadingInstructions LoadingInstructions { get; set; }

		/// <summary>
		/// Tries to get the content manager.
		/// </summary>
		/// <param name="contentManagerName">The content manager name.</param>
		/// <param name="contentManager">The content manager.</param>
		/// <returns>A value indicating whether the content manager was returned.</returns>
		static internal bool TryGetContentManager(string contentManagerName, out ContentManager contentManager)
		{ 
			var result = LoadingInstructions.ContentManagers.TryGetValue(contentManagerName, out contentManager);

			return result;
		}

		/// <summary>
		/// Gets the content manager names.
		/// </summary>
		/// <returns>A list of content manager names.</returns>
		static internal IList<string> GetContentManagerNames()
		{
			var controlNames = LoadingInstructions?.ContentManagers?.Keys?.ToList();

			return controlNames ?? [];
		}

		/// <summary>
		/// Gets the control names for the content manager.
		/// </summary>
		/// <param name="contentManagerName">The content manager name.</param>
		/// <returns>A list of control names for the content manager name.</returns>
		static internal IList<string> GetControlNamesForContentManager(string contentManagerName)
		{
			var controlList = LoadingInstructions?.ControlLinkages?.Where(e => e.ContentManagerName == contentManagerName)?
																   .Select(e => e.ContentName)?
																   .ToList();

			return controlList ?? [];
		}

		/// <summary>
		/// Gets the font names for the content manager.
		/// </summary>
		/// <param name="contentManagerName">The content manager name.</param>
		/// <returns>A list of font names for the content manager name.</returns>
		static internal IList<string> GetFontNamesForContentManager(string contentManagerName)
		{
			var fontList = LoadingInstructions?.FontLinkages?.Where(e => e.ContentManagerName == contentManagerName)?
															 .Select(e => e.ContentName)?
															 .ToList();

			return fontList ?? [];
		}

		/// <summary>
		/// Gets the tile set names for the content manager.
		/// </summary>
		/// <param name="contentManagerName">The content manager name.</param>
		/// <returns>A list of tile set names for the content manager name.</returns>
		static internal IList<string> GetTileSetNamesForContentManager(string contentManagerName)
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
		static internal IList<string> GetImageNamesForContentManager(string contentManagerName)
		{
			var contentList = LoadingInstructions?.ImageLinkages?.Where(e => e.ContentManagerName == contentManagerName)?
																 .Select(e => e.ContentName)?
																 .ToList();

			return contentList ?? [];
		}
	}
}

using BaseContent;
using Engine.Core.Initialization.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace LevelEditor
{
	/// <summary>
	/// The game container.
	/// </summary>
	static internal class GameContainer
	{
		/// <summary>
		/// Gets or sets the game.
		/// </summary>
		static internal Engine.Engine Game { get; set; }

		/// <summary>
		/// Gets the game services.
		/// </summary>
		static internal GameServiceContainer GameService { get; }

		/// <summary>
		/// Gets or sets the content exporters
		/// </summary>
		readonly static private List<IAmAContentExporter> ContentExporters = 
		[
			new ContentExporter(BaseContent.BaseContent.ContentManagerParams.ContentManagerName)
		];

		/// <summary>
		/// Gets the loading instructions.
		/// </summary>
		/// <param name="graphicsDeviceManager">The graphics device manager.</param>
		/// <returns>The loading instructions.</returns>
		static internal LoadingInstructions GetLoadingInstructions(GraphicsDeviceManager graphicsDeviceManager)
		{
			var contentManagers = new Dictionary<string, ContentManager>();
			var controlLinkages = new List<ContentManagerLinkage>();
			var fontLinkages = new List<ContentManagerLinkage>();
			var tilesetLinkages = new List<ContentManagerLinkage>();
			var imageLinkages = new List<ContentManagerLinkage>();

			foreach (var contentExporter in ContentExporters)
			{
				var contentManager = contentExporter.InitializeContentManager(graphicsDeviceManager);
				contentManagers.Add(contentExporter.ContentManagerName, contentManager);
				var controlNames = contentExporter.GetControlNames();
				var fontNames = contentExporter.GetFontNames();
				var tilesetNames = contentExporter.GetTilesetNames();
				var imageNames = contentExporter.GetImageNames();

				foreach (var controlName in controlNames)
				{
					controlLinkages.Add(new ContentManagerLinkage
					{
						ContentManagerName = contentExporter.ContentManagerName,
						ContentName = controlName
					});
				}

				foreach (var fontName in fontNames)
				{
					fontLinkages.Add(new ContentManagerLinkage
					{
						ContentManagerName = contentExporter.ContentManagerName,
						ContentName = fontName
					});
				}

				foreach (var tilesetName in tilesetNames)
				{
					tilesetLinkages.Add(new ContentManagerLinkage
					{
						ContentManagerName = contentExporter.ContentManagerName,
						ContentName = tilesetName
					});
				}

				foreach (var imageName in imageNames)
				{
					imageLinkages.Add(new ContentManagerLinkage
					{
						ContentManagerName = contentExporter.ContentManagerName,
						ContentName = imageName
					});
				}
			}

			var result = new LoadingInstructions
			{
				ContentManagers = contentManagers,
				FontLinkages = fontLinkages,
				ImageLinkages = imageLinkages,
				TilesetLinkages = tilesetLinkages,
				ControlLinkages = controlLinkages
			};

			return result;
		}
	}
}

using BaseContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LevelEditor
{
	/// <summary>
	/// Represents a content exporter
	/// </summary>
	public class ContentExporter : IAmAContentExporter
	{
		/// <summary>
		/// Gets or sets the content manager name.
		/// </summary>
		public string ContentManagerName { get; } = "BaseContent";

		/// <summary>
		/// Gets the control names.
		/// </summary>
		/// <returns>The control names.</returns>
		public List<string> GetControlNames()
		{
			var controlPath = $@"{Directory.GetCurrentDirectory()}\{this.ContentManagerName}\Controls";
			string[] controlPaths = Directory.GetFiles(controlPath);
			var controlName = controlPaths?.Select(e => Path.GetFileNameWithoutExtension(e))?
									  .ToList();

			return controlName ?? [];
		}

		/// <summary>
		/// Get the font names.
		/// </summary>
		/// <returns>The list of font names.</returns>
		public List<string> GetFontNames()
		{
			var fontPath = $@"{Directory.GetCurrentDirectory()}\{this.ContentManagerName}\Fonts";
			string[] fontPaths = Directory.GetFiles(fontPath);
			var fontNames = fontPaths?.Select(e => Path.GetFileNameWithoutExtension(e))?
									  .ToList();

			return fontNames ?? [];
		}

		/// <summary>
		/// Gets the tile set names.
		/// </summary>
		/// <returns>A list of tile set names.</returns>
		public List<string> GetImageNames()
		{
			var imagePath = $@"{Directory.GetCurrentDirectory()}\{this.ContentManagerName}\Images";
			string[] imagePaths = Directory.GetFiles(imagePath);
			var imageNames = imagePaths?.Select(e => Path.GetFileNameWithoutExtension(e))?
									    .ToList();

			return imageNames ?? [];
		}

		/// <summary>
		/// Gets the tile set names.
		/// </summary>
		/// <returns>A list of tile set names.</returns>
		public List<string> GetTilesetNames()
		{
			var tileSetPath = $@"{Directory.GetCurrentDirectory()}\{this.ContentManagerName}\TileSets";
			string[] tileSetPaths = Directory.GetFiles(tileSetPath);
			var tileSetNames = tileSetPaths?.Select(e => Path.GetFileNameWithoutExtension(e))?
										    .ToList();

			return tileSetNames ?? [];
		}

		/// <summary>
		///	Initializes the content manager.
		/// </summary>
		/// <param name="graphicsDeviceManager">The graphics device manager.</param>
		/// <returns>The content manager.</returns>
		public ContentManager InitializeContentManager(GraphicsDeviceManager graphicsDeviceManager)
		{
			var serviceProvider = new BasicServiceProvider(graphicsDeviceManager);

			return new ContentManager(serviceProvider, Directory.GetCurrentDirectory());
		}
	}
}

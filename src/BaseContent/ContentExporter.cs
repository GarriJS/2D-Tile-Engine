using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace BaseContent
{
	/// <summary>
	/// Represents a content exporter
	/// </summary>
	public static class ContentExporter
	{
		/// <summary>
		/// Gets or sets the content manager name.
		/// </summary>
		public static string ContentManagerName { get; } = "Base";

		/// <summary>
		/// Get the font names.
		/// </summary>
		/// <returns>The list of font names.</returns>
		public static List<string> GetFontNames()
		{
			var fontPath = $@"{Directory.GetCurrentDirectory()}\Fonts";
			string[] fontPaths = Directory.GetFiles(fontPath);
			var fontNames = fontPaths?.Select(e => Path.GetFileNameWithoutExtension(e))
									  .ToList();

			return fontNames ?? [];
		}

		/// <summary>
		/// Gets the tile set names.
		/// </summary>
		/// <returns>A list of tile set names.</returns>
		public static List<string> GetImageNames()
		{
			var imagePath = $@"{Directory.GetCurrentDirectory()}\Images";
			string[] imagePaths = Directory.GetFiles(imagePath);
			var imageNames = imagePaths?.Select(e => Path.GetFileNameWithoutExtension(e))
									    .ToList();

			return imageNames ?? [];
		}

		/// <summary>
		/// Gets the tile set names.
		/// </summary>
		/// <returns>A list of tile set names.</returns>
		public static List<string> GetTilesetNames()
		{
			var tileSetPath = $@"{Directory.GetCurrentDirectory()}\TileSets";
			string[] tileSetPaths = Directory.GetFiles(tileSetPath);
			var tileSetNames = tileSetPaths?.Select(e => Path.GetFileNameWithoutExtension(e))
										    .ToList();

			return tileSetNames ?? [];
		}

		/// <summary>
		///	Initializes the content manager.
		/// </summary>
		/// <param name="graphicsDeviceManager">The graphics device manager.</param>
		/// <returns>The content manager.</returns>
		public static ContentManager InitializeContentManager(GraphicsDeviceManager graphicsDeviceManager)
		{
			var serviceProvider = new BasicServiceProvider(graphicsDeviceManager);

			return new ContentManager(serviceProvider, Directory.GetCurrentDirectory());
		}
	}
}

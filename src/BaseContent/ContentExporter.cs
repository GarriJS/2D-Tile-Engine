using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BaseContent
{
	/// <summary>
	/// Represents a content exporter
	/// </summary>
	/// <remarks>
	/// Initializes a new instance of the content exporter.
	/// </remarks>
	/// <param name="contentMangerName">The content manager name.</param>
	public class ContentExporter(string contentMangerName) : IAmAContentExporter
	{
		/// <summary>
		/// Gets or sets the content manager name.
		/// </summary>
		public string ContentManagerName { get; } = contentMangerName;

		/// <summary>
		/// Gets or sets the initial disc models.
		/// </summary>
		public List<object> InitialDiscModels { get; set; }

		/// <summary>
		/// Gets the control names.
		/// </summary>
		/// <returns>The control names.</returns>
		public List<string> GetControlNames()
		{
			var controlPath = $@"{Directory.GetCurrentDirectory()}\{this.ContentManagerName}\Controls";

			if (false == Directory.Exists(controlPath))
			{
				return [];
			}

			string[] controlPaths = Directory.GetFiles(controlPath);
			var controlName = controlPaths?.Select(Path.GetFileNameWithoutExtension)?
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

			if (false == Directory.Exists(fontPath))
			{
				return [];
			}

			string[] fontPaths = Directory.GetFiles(fontPath);
			var fontNames = fontPaths?.Select(Path.GetFileNameWithoutExtension)?
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

			if (false == Directory.Exists(imagePath))
			{
				return [];
			}

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

			if (false == Directory.Exists(tileSetPath))
			{
				return [];
			}

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
			var serviceProvider = new BaseServiceProvider(graphicsDeviceManager);

			return new ContentManager(serviceProvider, Directory.GetCurrentDirectory());
		}
	}
}

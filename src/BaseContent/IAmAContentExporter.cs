using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace BaseContent
{
	/// <summary>
	/// Represents a content exporter
	/// </summary>
	public interface IAmAContentExporter
	{
		/// <summary>
		/// Gets or sets the content manager name.
		/// </summary>
		public string ContentManagerName { get; }

		/// <summary>
		/// Get the font names.
		/// </summary>
		/// <returns>The list of font names.</returns>
		public List<string> GetFontNames();

		/// <summary>
		/// Gets the tile set names.
		/// </summary>
		/// <returns>A list of tile set names.</returns>
		public List<string> GetImageNames();

		/// <summary>
		/// Gets the tile set names.
		/// </summary>
		/// <returns>A list of tile set names.</returns>
		public List<string> GetTilesetNames();

		/// <summary>
		///	Initializes the content manager.
		/// </summary>
		/// <param name="graphicsDeviceManager">The graphics device manager.</param>
		/// <returns>The content manager.</returns>
		public ContentManager InitializeContentManager(GraphicsDeviceManager graphicsDeviceManager);
	}
}

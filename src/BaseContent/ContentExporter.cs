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
		/// Gets the sprite sheet names.
		/// </summary>
		/// <returns>A list of spritesheet names.</returns>
		public static List<string> GetSpritesheetNames()
		{
			return
			[
				"dark_grass_simplified"
			];
		}

		/// <summary>
		///	Initializes the content manager.
		/// </summary>
		/// <param name="graphicsDeviceManager">The graphics device manager.</param>
		/// <returns>The content manager.</returns>
		public static ContentManager InitializeContentManager(GraphicsDeviceManager graphicsDeviceManager)
		{
			string rootDirectory = Directory.GetCurrentDirectory();
			var serviceProvider = new BasicServiceProvider(graphicsDeviceManager);
			return new ContentManager(serviceProvider, rootDirectory);
		}
	}
}

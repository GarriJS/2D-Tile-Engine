using Microsoft.Xna.Framework.Content;

namespace Content
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
		/// Gets or sets the hidden content manager.
		/// </summary>
		private static ContentManager? HiddenContentManager { get; set; }

		/// <summary>
		/// Gets the content manager.
		/// </summary>
		public static ContentManager ContentManager { get => HiddenContentManager ?? InitializeContentManager(); }

		/// <summary>
		///	Initializes the content manager.
		/// </summary>
		/// <returns>The content manager.</returns>
		private static ContentManager InitializeContentManager()
		{
			string rootDirectory = Directory.GetCurrentDirectory();
			var serviceProvider = new BasicServerProvider();
			HiddenContentManager = new ContentManager(serviceProvider, rootDirectory);

			return HiddenContentManager;
		}
	}
}

using Engine.Core.Fonts.Services.Contracts;
using Engine.Core.Initialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Engine.Core.Fonts.Services
{
	/// <summary>s
	/// Represents a font service.
	/// </summary>
	/// <remarks>
	/// Initializes a new instance of the font service.
	/// </remarks>
	/// <param name="gameServices">The game services.</param>
	public class FontService(GameServiceContainer gameServices) : IFontService
	{
		private readonly GameServiceContainer _gameServices = gameServices;

		/// <summary>
		/// Gets or sets the sprite fonts.
		/// </summary>
		private Dictionary<string, SpriteFont> SpriteFonts { get; set; } = [];

		/// <summary>
		/// Gets or sets the debug sprite font name.
		/// </summary>
		private string DebugSpriteFontName { get; set; }

		/// <summary>
		/// Gets or sets the debug sprite font.
		/// </summary>
		public SpriteFont DebugSpriteFont { get; set; }
		
		/// <summary>
		/// Configures the service.
		/// </summary>
		public void ConfigureService()
		{
			//TODO make configurable
			this.DebugSpriteFontName = "Monolight";
		}

		/// <summary>
		/// Loads the font content.
		/// </summary>
		public void LoadContent()
		{
			var contentManagerNames = LoadingInstructionsContainer.GetContentManagerNames();

			foreach (var contentManagerName in contentManagerNames)
			{
				if (false == LoadingInstructionsContainer.TryGetContentManager(contentManagerName, out var contentManager))
					continue;

				var managerFontNames = LoadingInstructionsContainer.GetFontNamesForContentManager(contentManagerName);

				foreach (var managerFontName in managerFontNames)
				{
					var font = contentManager.Load<SpriteFont>($@"{contentManagerName}\Fonts\{managerFontName}");
					this.SpriteFonts.Add(managerFontName, font);
				}
			}

			this.SetDebugSpriteFont(this.DebugSpriteFontName);
		}

		/// <summary>
		/// Gets the sprite font.
		/// </summary>
		/// <param name="spriteFontName">The sprite font name.</param>
		/// <returns>The sprite font.</returns>
		public SpriteFont GetSpriteFont(string spriteFontName)
		{
			if (true == this.SpriteFonts.TryGetValue(spriteFontName, out var font))
				return font;

			//LOGGING
			return null;
		}

		/// <summary>
		/// Sets the debug sprite front.
		/// </summary>
		/// <param name="spriteFontName">The sprite font name.</param>
		public void SetDebugSpriteFont(string spriteFontName)
		{ 
			this.DebugSpriteFont = this.GetSpriteFont(spriteFontName);
		}
	}
}

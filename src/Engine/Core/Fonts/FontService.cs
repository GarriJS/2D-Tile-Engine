using Engine.Core.Fonts.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Engine.Core.Fonts
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
		private readonly GameServiceContainer _gameServiceContainer = gameServices;

		/// <summary>
		/// Gets or sets the sprite fonts.
		/// </summary>
		private Dictionary<string, SpriteFont> SpriteFonts { get; set; } = new Dictionary<string, SpriteFont>();

		/// <summary>
		/// Loads the font content.
		/// </summary>
		public void LoadContent()
		{
			var contentManager = this._gameServiceContainer.GetService<ContentManager>();
			var spriteFontPath = $@"{contentManager.RootDirectory}\Fonts";
			string[] spriteFontFiles = Directory.GetFiles(spriteFontPath);

			if (false == spriteFontFiles.Any())
			{
				return;
			}

			foreach (string spriteFontFile in spriteFontFiles)
			{
				try
				{
					var spriteFontNames = Path.GetFileNameWithoutExtension(spriteFontFile);
					var spriteFont = contentManager.Load<SpriteFont>($@"Fonts\{spriteFontNames}");
					this.SpriteFonts.Add(spriteFontNames, spriteFont);
				}
				catch (Exception ex)
				{
					Debug.WriteLine($"Loading font for {spriteFontFile}: {ex.Message}");
				}
			}
		}

		/// <summary>
		/// Gets the sprite font.
		/// </summary>
		/// <param name="spriteFontName">The sprite font name.</param>
		/// <returns>The sprite font.</returns>
		public SpriteFont GetSpriteFont(string spriteFontName)
		{
			if (true == this.SpriteFonts.TryGetValue(spriteFontName, out var font))
			{ 
				return font;
			}

			Debug.WriteLine($"Sprite font {spriteFontName} was not found.");
			return null;
		}
	}
}

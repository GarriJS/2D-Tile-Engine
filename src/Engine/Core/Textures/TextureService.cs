using Engine.Core.Constants;
using Engine.Core.Textures.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Engine.Core.Textures
{
	/// <summary>
	/// Represents a texture service.
	/// </summary>
	/// <remarks>
	/// Initializes a new instance of the texture service.
	/// </remarks>
	/// <param name="gameServices">The game services.</param>
	public class TextureService(GameServiceContainer gameServices) : ITextureService
	{
		private readonly GameServiceContainer _gameServices = gameServices;

		/// <summary>
		/// Gets or sets the textures by texture name.
		/// </summary>
		private Dictionary<string, Texture2D> Textures { get; set; } = new Dictionary<string, Texture2D>();

		/// <summary>
		/// Gets or sets the sprite sheets. 
		/// </summary>
		private Dictionary<string, Texture2D> Spritesheets { get; set; } = new Dictionary<string, Texture2D>();

		/// <summary>
		/// Loads the texture content.
		/// </summary>
		public void LoadContent()
		{
			this.LoadImages();
			this.LoadTileSets();
		}

		/// <summary>
		/// Gets the texture.
		/// </summary>
		/// <param name="textureName">The texture name.</param>
		/// <param name="spritesheetName">The spritesheet name.</param>
		/// <returns>The texture.</returns>
		public Texture2D GetTexture(string textureName, string spritesheetName = null)
		{
			if (true == this.Textures.TryGetValue(textureName, out var texture))
			{
				return texture;
			}
			else if ((false == string.IsNullOrEmpty(spritesheetName)) &&
				     (true == this.Spritesheets.TryGetValue(spritesheetName, out var spritesheet)))
			{ 
				return spritesheet;
			}

			Debug.WriteLine($"Texture {textureName} and Spritesheet {spritesheetName} were not found.");
			return null;
		}

		/// <summary>
		/// Gets the texture name.
		/// </summary>
		/// <param name="spritesheet">The sprite sheet.</param>
		/// <param name="spritesheetBox">The spritesheet box.</param>
		/// <returns>The texture name.</returns>
		public string GetTextureName(string spritesheet, Rectangle spritesheetBox)
		{
			return $"{spritesheet}_{spritesheetBox.X}_{spritesheetBox.Y}_{spritesheetBox.Width}_{spritesheetBox.Height}";
		}

		/// <summary>
		/// Loads the image.
		/// </summary>
		private void LoadImages()
		{
			var contentManager = this._gameServices.GetService<ContentManager>();
			var imagesPath = $@"{contentManager.RootDirectory}\Images";
			string[] imagesFiles = Directory.GetFiles(imagesPath);

			if (false == imagesFiles.Any())
			{
				return;
			}

			foreach (string imagesFile in imagesFiles)
			{
				try
				{
					var imageName = Path.GetFileNameWithoutExtension(imagesFile);
					var texture = contentManager.Load<Texture2D>($@"Images\{imageName}");
					this.Spritesheets.Add(imageName, texture);
				}
				catch (Exception ex)
				{
					Debug.WriteLine($"Loading images failed for {imagesFile}: {ex.Message}");
				}
			}
		}

		/// <summary>
		/// Loads the tile sets.
		/// </summary>
		private void LoadTileSets()
		{
			var contentManager = this._gameServices.GetService<ContentManager>();
			var tileSetsPath = $@"{contentManager.RootDirectory}\TileSets";
			string[] tileSetFiles = Directory.GetFiles(tileSetsPath);

			if (false == tileSetFiles.Any())
			{
				return;
			}

			foreach (string tileSetFile in tileSetFiles)
			{
				try
				{
					var textureName = Path.GetFileNameWithoutExtension(tileSetFile);
					var texture = contentManager.Load<Texture2D>($@"TileSets\{textureName}");
					this.Spritesheets.Add(textureName, texture);
					this.LoadTileTextures(textureName);
				}
				catch (Exception ex)
				{
					Debug.WriteLine($"Loading tile set failed for {tileSetFile}: {ex.Message}");
				}
			}
		}

		/// <summary>
		/// Loads the tile textures.
		/// </summary>
		private void LoadTileTextures(string tileSetName)
		{
			var spritesheet = this.Spritesheets[tileSetName];

			for (int x = 0; x < spritesheet.Width; x += TileConstants.TILE_SIZE)
			{
				for (int y = 0; y < spritesheet.Height; y += TileConstants.TILE_SIZE)
				{
					try
					{
						var sourceRectangle = new Rectangle(x, y, TileConstants.TILE_SIZE, TileConstants.TILE_SIZE);
						var textureData = new Color[TileConstants.TILE_SIZE * TileConstants.TILE_SIZE];
						spritesheet.GetData(0, sourceRectangle, textureData, 0, textureData.Length);
						var textureName = this.GetTextureName(tileSetName, sourceRectangle);
						var extendedTexture = this.ExtendTexture(sourceRectangle.Width, sourceRectangle.Height, textureData, TextureConstants.TEXTURE_EXTENSION_AMOUNT);
						this.Textures.Add(textureName, extendedTexture);
					}
					catch (Exception ex)
					{
						Debug.WriteLine($"Loading tile textures failed {tileSetName}: {ex.Message}");
					}
				}
			}
		}

		/// <summary>
		/// Extends the texture.
		/// </summary>
		/// <param name="sourceTextureHeight">The source texture height.</param>
		/// <param name="sourceTextureWidth">The source texture width.</param>
		/// <param name="textureData">The texture data.</param>
		/// <param name="extendAmount">The extend amount.</param>
		/// <returns>The extended texture.</returns>
		private Texture2D ExtendTexture(int sourceTextureHeight, int sourceTextureWidth, Color[] textureData, int extendAmount)
		{
			var extendedWidth = sourceTextureWidth + (extendAmount * 2);
			var extendedHeight = sourceTextureHeight + (extendAmount * 2);
			var extendedTextureData = new Color[extendedWidth * extendedHeight];

			for (int i = 0; i < textureData.Length; i++)
			{
				int originalX = i % sourceTextureWidth;
				int originalY = i / sourceTextureWidth;
				int x = originalX + extendAmount;
				int y = originalY + extendAmount;

				if (originalY == 0)
				{
					if (originalX == 0)
					{
						// Extend top left corner
						for (int j = 0; j < extendAmount + 1; j++)
						{
							for (int k = 0; k < extendAmount + 1; k++)
							{
								extendedTextureData[(x - k) + ((y - j) * extendedWidth)] = textureData[i];
							}
						}
					}
					else if (originalX == sourceTextureWidth - 1)
					{
						// Extend top right corner
						for (int j = 0; j < extendAmount + 1; j++)
						{
							for (int k = 0; k < extendAmount + 1; k++)
							{
								extendedTextureData[(x + k) + ((y - j) * extendedWidth)] = textureData[i];
							}
						}
					}
					else
					{
						// Extend top edge
						for (int j = 0; j < extendAmount + 1; j++)
						{
							extendedTextureData[x + ((y - j) * extendedWidth)] = textureData[i];
						}
					}
				}
				else if (originalY == sourceTextureHeight - 1)
				{
					if (originalX == 0)
					{
						// Extend bottom left corner
						for (int j = 0; j < extendAmount + 1; j++)
						{
							for (int k = 0; k < extendAmount + 1; k++)
							{
								extendedTextureData[(x - k) + ((y + j) * extendedWidth)] = textureData[i];
							}
						}
					}
					else if (originalX == sourceTextureWidth - 1)
					{
						// Extend bottom right corner
						for (int j = 0; j < extendAmount + 1; j++)
						{
							for (int k = 0; k < extendAmount + 1; k++)
							{
								extendedTextureData[(x + k) + ((y + j) * extendedWidth)] = textureData[i];
							}
						}
					}
					else
					{
						// Extend bottom edge
						for (int j = 0; j < extendAmount + 1; j++)
						{
							extendedTextureData[x + ((y + j) * extendedWidth)] = textureData[i];
						}
					}
				}
				else if (originalX == 0)
				{
					// Extend left edge
					for (int k = 0; k < extendAmount + 1; k++)
					{
						extendedTextureData[(x - k) + (y * extendedWidth)] = textureData[i];
					}
				}
				else if (originalX == sourceTextureWidth - 1)
				{
					// Extend right edge
					for (int k = 0; k < extendAmount + 1; k++)
					{
						extendedTextureData[(x + k) + (y * extendedWidth)] = textureData[i];
					}
				}
				else
				{
					// Fill the middle
					extendedTextureData[x + (y * extendedWidth)] = textureData[i];
				}
			}

			var graphicsDeviceService = this._gameServices.GetService<IGraphicsDeviceService>();
			var extendedTexture = new Texture2D(graphicsDeviceService.GraphicsDevice, extendedWidth, extendedHeight);
			extendedTexture.SetData(extendedTextureData);

			return extendedTexture;
		}
	}
}

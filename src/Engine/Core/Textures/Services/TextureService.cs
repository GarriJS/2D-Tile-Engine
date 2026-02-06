using Engine.Core.Constants;
using Engine.Core.Initialization;
using Engine.Core.Textures.Services.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Engine.Core.Textures.Services
{
	/// <summary>
	/// Represents a texture service.
	/// </summary>
	/// <remarks>
	/// Initializes a new instance of the texture service.
	/// </remarks>
	/// <param name="gameServices">The game services.</param>
	sealed public class TextureService(GameServiceContainer gameServices) : ITextureService
	{
		readonly private GameServiceContainer _gameServices = gameServices;

		/// <summary>
		/// Gets the debug texture.
		/// </summary>
		public Texture2D DebugTexture { get; private set; }

		/// <summary>
		/// Gets or sets the images.
		/// </summary>
		private Dictionary<string, Texture2D> Images { get; set; } = [];

		/// <summary>
		/// Gets or sets the tile sets. 
		/// </summary>
		private Dictionary<string, Texture2D> Tilesets { get; set; } = [];

		/// <summary>
		/// Gets or sets the tiles.
		/// </summary>
		private Dictionary<string, Texture2D> Tiles { get; set; } = [];

		/// <summary>
		/// Loads the texture content.
		/// </summary>
		public void LoadContent()
		{
			this.DebugTexture = this.GetDebugTexture();

			this.LoadImages();
			this.LoadTilesets();
		}

		/// <summary>
		/// Tries to get the texture.
		/// </summary>
		/// <param name="textureName">The texture name.</param>
		/// <param name="texture">The texture.</param>
		/// <returns>A value indicating whether the texture was found.</returns>
		public bool TryGetTexture(string textureName, out Texture2D texture)
		{
			if ((true == this.Images.TryGetValue(textureName, out texture)) ||
				(true == this.Tilesets.TryGetValue(textureName, out texture)) ||
				(true == this.Tiles.TryGetValue(textureName, out texture)))
				return true;

			return false;
		}

		/// <summary>
		/// Gets the texture name.
		/// </summary>
		/// <param name="spritesheet">The sprite sheet.</param>
		/// <param name="spritesheetBox">The spritesheet box.</param>
		/// <returns>The texture name.</returns>
		public string GetTextureName(string spritesheet, Rectangle spritesheetBox)
		{
			var result = $"{spritesheet}_{spritesheetBox.X}_{spritesheetBox.Y}_{spritesheetBox.Width}_{spritesheetBox.Height}";

			return result;
		}

		/// <summary>
		/// Extends the texture.
		/// </summary>
		/// <param name="textureData">The texture data.</param>
		/// <param name="sourceTextureWidth">The source texture width.</param>
		/// <param name="sourceTextureHeight">The source texture height.</param>
		/// <param name="extendAmount">The extend amount.</param>
		/// <returns>The extended texture.</returns>
		public Texture2D ExtendTexture(Color[] textureData, int sourceTextureWidth, int sourceTextureHeight, int extendAmount)
		{
			var graphicsDeviceService = this._gameServices.GetService<IGraphicsDeviceService>();
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
						// Extend top left corner
						for (int j = 0; j < extendAmount + 1; j++)
							for (int k = 0; k < extendAmount + 1; k++)
								extendedTextureData[(x - k) + ((y - j) * extendedWidth)] = textureData[i];
					else if (originalX == sourceTextureWidth - 1)
						// Extend top right corner
						for (int j = 0; j < extendAmount + 1; j++)
							for (int k = 0; k < extendAmount + 1; k++)
								extendedTextureData[(x + k) + ((y - j) * extendedWidth)] = textureData[i];
					else
						// Extend top edge
						for (int j = 0; j < extendAmount + 1; j++)
							extendedTextureData[x + ((y - j) * extendedWidth)] = textureData[i];
				}
				else if (originalY == sourceTextureHeight - 1)
				{
					if (originalX == 0)
						// Extend bottom left corner
						for (int j = 0; j < extendAmount + 1; j++)
							for (int k = 0; k < extendAmount + 1; k++)
								extendedTextureData[(x - k) + ((y + j) * extendedWidth)] = textureData[i];
					else if (originalX == sourceTextureWidth - 1)
						// Extend bottom right corner
						for (int j = 0; j < extendAmount + 1; j++)
							for (int k = 0; k < extendAmount + 1; k++)
								extendedTextureData[(x + k) + ((y + j) * extendedWidth)] = textureData[i];
					else
						// Extend bottom edge
						for (int j = 0; j < extendAmount + 1; j++)
							extendedTextureData[x + ((y + j) * extendedWidth)] = textureData[i];
				}
				else if (originalX == 0)
				{
					// Extend left edge
					for (int k = 0; k < extendAmount + 1; k++)
						extendedTextureData[(x - k) + (y * extendedWidth)] = textureData[i];
				}
				else if (originalX == sourceTextureWidth - 1)
				{
					// Extend right edge
					for (int k = 0; k < extendAmount + 1; k++)
						extendedTextureData[(x + k) + (y * extendedWidth)] = textureData[i];
				}
				else
				{
					// Fill the middle
					extendedTextureData[x + (y * extendedWidth)] = textureData[i];
				}
			}

			var extendedTexture = new Texture2D(graphicsDeviceService.GraphicsDevice, extendedWidth, extendedHeight);
			extendedTexture.SetData(extendedTextureData);

			return extendedTexture;
		}

		/// <summary>
		/// Loads the image.
		/// </summary>
		private void LoadImages()
		{
			var contentManagerNames = LoadingInstructionsContainer.GetContentManagerNames();

			foreach (var contentManagerName in contentManagerNames)
			{
				if (false == LoadingInstructionsContainer.TryGetContentManager(contentManagerName, out var contentManager))
					continue;

				var managerImageNames = LoadingInstructionsContainer.GetImageNamesForContentManager(contentManagerName);

				foreach (var managerImageName in managerImageNames)
				{
					var image = contentManager.Load<Texture2D>($@"{contentManagerName}\Images\{managerImageName}");
					this.Images.Add(managerImageName, image);
				}
			}
		}

		/// <summary>
		/// Loads the tile sets.
		/// </summary>
		private void LoadTilesets()
		{
			var contentManagerNames = LoadingInstructionsContainer.GetContentManagerNames();

			foreach (var contentManagerName in contentManagerNames)
			{
				if (false == LoadingInstructionsContainer.TryGetContentManager(contentManagerName, out var contentManager))
					continue;

				var managerTilesetNames = LoadingInstructionsContainer.GetTileSetNamesForContentManager(contentManagerName);

				foreach (var managerTilesetName in managerTilesetNames)
				{
					var tileset = contentManager.Load<Texture2D>($@"{contentManagerName}\TileSets\{managerTilesetName}");
					this.Tilesets.Add(managerTilesetName, tileset);
					this.LoadTiles(managerTilesetName, tileset);
				}
			}
		}

		/// <summary>
		/// Loads the tiles.
		/// </summary>
		/// <param name="tilesetName">The tile set name.</param>
		/// <param name="tilesetTexture">The tile set texture.</param>
		private void LoadTiles(string tilesetName, Texture2D tilesetTexture = null)
		{
			if (null == tilesetTexture)
				tilesetTexture = this.Tilesets[tilesetName];

			for (int x = 0; x < tilesetTexture.Width; x += TileConstants.TILE_SIZE)
			{
				for (int y = 0; y < tilesetTexture.Height; y += TileConstants.TILE_SIZE)
				{
					var sourceRectangle = new Rectangle
					{
						X = x,
						Y = y,
						Width = TileConstants.TILE_SIZE,
						Height = TileConstants.TILE_SIZE
					};
					var textureData = new Color[TileConstants.TILE_SIZE * TileConstants.TILE_SIZE];
					tilesetTexture.GetData(0, sourceRectangle, textureData, 0, textureData.Length);
					var tileName = this.GetTextureName(tilesetName, sourceRectangle);
					var extendedTexture = this.ExtendTexture(textureData, sourceRectangle.Width, sourceRectangle.Height, TextureConstants.TEXTURE_EXTENSION_AMOUNT);
					this.Tiles.Add(tileName, extendedTexture);
				}
			}
		}

		/// <summary>
		/// Gets the debug texture.
		/// </summary>
		/// <returns>The debug texture.</returns>
		private Texture2D GetDebugTexture()
		{
			var graphicsDeviceService = this._gameServices.GetService<IGraphicsDeviceService>();
			var texture = new Texture2D(graphicsDeviceService.GraphicsDevice, 1, 1);
			Color[] colorData = [ Color.MonoGameOrange ];
			texture.SetData(colorData);

			return texture;
		}
	}
}

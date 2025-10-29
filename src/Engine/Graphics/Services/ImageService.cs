﻿using Engine.Core.Constants;
using Engine.Core.Textures.Services.Contracts;
using Engine.DiskModels.Drawing;
using Engine.DiskModels.Drawing.Contracts;
using Engine.Graphics.Enum;
using Engine.Graphics.Models;
using Engine.Graphics.Models.Contracts;
using Engine.Graphics.Services.Contracts;
using Engine.Physics.Models.SubAreas;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;

namespace Engine.Graphics.Services
{
	/// <summary>
	/// Represents a image service.
	/// </summary>
	/// <remarks>
	/// Initializes the image service.
	/// </remarks>
	/// <param name="gameService">The game services.</param>
	public class ImageService(GameServiceContainer gameService) : IImageService
	{
		private readonly GameServiceContainer _gameServices = gameService;

		/// <summary>
		/// Gets the image from the model.
		/// </summary>
		/// <param name="imageModel">The image model.</param>
		/// <returns>The image.</returns>
		public IAmAImage GetImageFromModel(IAmAImageModel imageModel)
		{
			return this.GetImageFromModel<SimpleImage>(imageModel);
		}

		/// <summary>
		/// Gets the image from the model.
		/// </summary>
		/// <param name="imageModel">The image model.</param>
		/// <returns>The image.</returns>
		public T GetImageFromModel<T>(IAmAImageModel imageModel) where T : IAmAImage
		{
			var textureService = this._gameServices.GetService<ITextureService>();
			var graphicService = this._gameServices.GetService<IGraphicService>();

			if (false == textureService.TryGetTexture(imageModel.TextureName, out var texture))
			{
				texture = textureService.DebugTexture;
			}

			IAmAImage image = imageModel switch
			{
				CompositeImageModel => new CompositeImage
				{
					TextureName = imageModel.TextureName,
					Texture = texture
				},
				_ => new SimpleImage
				{
					TextureName = imageModel.TextureName,
					Texture = texture
				}
			};

			if (image is SimpleImage simpleImage)
			{
				var simpleImageModel = imageModel as SimpleImageModel;
				simpleImage.TextureRegion = graphicService.GetTextureRegionFromModel(simpleImageModel.TextureRegion);
			}

			if (image is CompositeImage imageByParts)
			{
				var imageByPartsModel = imageModel as CompositeImageModel;

				if ((imageByParts.TextureRegions is null) ||
					(imageByParts.TextureRegions.Any(e => e is null || e.Length != imageByPartsModel.TextureRegions[0].Length)) ||
					(imageByParts.TextureRegions.Length != imageByParts.TextureRegions[0].Length))
				{
					throw new ArgumentException("Image by parts does not have valid texture regions. Texture regions must have equal length columns and rows.");
				}

				var firstRowTotalWidth = imageByPartsModel.TextureRegions[0].Sum(e => e.DisplayArea.Width);

				foreach (var textureRegionRow in imageByPartsModel.TextureRegions)
				{
					var rowWidth = textureRegionRow.Sum(e => e.DisplayArea.Width);

					if (firstRowTotalWidth != rowWidth)
					{
						throw new ArgumentException("Image by parts does not have valid texture regions. Row widths are not equal.");
					}
				}

				var firstColumnTotalHeight = imageByPartsModel.TextureRegions.Sum(e => e[0].DisplayArea.Height);

				for (int col = 0; col < imageByPartsModel.TextureRegions[0].Length; col++)
				{
					var columnHeight = imageByPartsModel.TextureRegions.Sum(row => row[col].DisplayArea.Height);

					if (firstColumnTotalHeight != columnHeight)
					{
						throw new ArgumentException("Image by parts does not have valid texture regions. Column heights are not equal.");
					}
				}

				var textureRegions = new TextureRegion[imageByPartsModel.TextureRegions.Length][];

				for (int i = 0; i < imageByPartsModel.TextureRegions.Length; i++)
				{
					textureRegions[i] = new TextureRegion[imageByPartsModel.TextureRegions[i].Length];

					for (int j = 0; j < imageByPartsModel.TextureRegions.Length; j++)
					{
						textureRegions[i][j] = graphicService.GetTextureRegionFromModel(imageByPartsModel.TextureRegions[i][j]);
					}
				}

				imageByParts.TextureRegions = textureRegions;
			}

			return (T)image;
		}

		/// <summary>
		/// Gets the image.
		/// </summary>
		/// <param name="textureName">The texture name.</param>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <returns>The image.</returns>
		public SimpleImage GetImage(string textureName, int width, int height)
		{
			if (true == string.IsNullOrEmpty(textureName))
			{
				return null;
			}

			var textureService = this._gameServices.GetService<ITextureService>();

			if (false == textureService.TryGetTexture(textureName, out var texture))
			{
				texture = textureService.DebugTexture;
			}

			var result = new SimpleImage
			{
				TextureName = textureName,
				TextureRegion = new TextureRegion
				{ 
					TextureRegionType = TextureRegionType.Simple,
					TextureBox = new Rectangle
					{
						X = TextureConstants.TEXTURE_EXTENSION_AMOUNT,
						Y = TextureConstants.TEXTURE_EXTENSION_AMOUNT,
						Width = width,
						Height = height
					},
					DisplayArea = new SubArea
					{
						Width = TextureConstants.TEXTURE_EXTENSION_AMOUNT,
						Height = TextureConstants.TEXTURE_EXTENSION_AMOUNT
					}
				},
				Texture = texture
			};

			return result;
		}

		/// <summary>
		/// Combines the image textures into one texture.
		/// </summary>
		/// <param name="images">The image.</param>
		/// <returns>The combined texture.</returns>
		public Texture2D CombineImageTextures(SimpleImage[][] images)
		{
			if ((images is null) ||
				(0 == images.Length) ||
				(true == images.Any(e => e is null || 0 == e.Length)) ||
				(false == images.All(e => e.Length == images[0].Length)))
			{
				return null;
			}

			var graphicDeviceService = this._gameServices.GetService<IGraphicsDeviceService>();
			var textureService = this._gameServices.GetService<ITextureService>();

			var graphicsDevice = graphicDeviceService.GraphicsDevice;
			int totalWidth = 0;
			int totalHeight = 0;

			for (int c = 0; c < images[0].Length; c++)
			{
				totalWidth += images[0][c].TextureRegion.TextureBox.Width;
			}

			for (int r = 0; r < images.Length; r++)
			{
				totalHeight += images[r][0].TextureRegion.TextureBox.Height;
			}

			var renderTarget = new RenderTarget2D(graphicsDevice, totalWidth, totalHeight);
			var spriteBatch = new SpriteBatch(graphicsDevice);

			graphicsDevice.SetRenderTarget(renderTarget);
			graphicsDevice.Clear(Color.Transparent);
			spriteBatch.Begin(samplerState: SamplerState.PointClamp);
			int verticalOffset = 0;

			for (int r = 0; r < images.Length; r++)
			{
				int horizontalOffset = 0;

				for (int c = 0; c < images[0].Length; c++)
				{
					var img = images[r][c];

					if (img?.Texture == null)
					{
						horizontalOffset += img?.TextureRegion.TextureBox.Width ?? 0;

						continue;
					}

					var destinationRectangle = new Rectangle
					{
						X = horizontalOffset,
						Y = verticalOffset,
						Width = img.TextureRegion.TextureBox.Width,
						Height = img.TextureRegion.TextureBox.Height
					};
					spriteBatch.Draw(
						img.Texture,
						destinationRectangle,
						img.TextureRegion.TextureBox,
						Color.White
					);
					horizontalOffset += img.TextureRegion.TextureBox.Width;
				}

				verticalOffset += images[r][0].TextureRegion.TextureBox.Height;
			}

			spriteBatch.End();
			graphicsDevice.SetRenderTarget(null);

			Color[] data = new Color[renderTarget.Width * renderTarget.Height];
			renderTarget.GetData(data);
			renderTarget.Dispose();
			var finalTexture = textureService.ExtendTexture(data, totalWidth, totalHeight, TextureConstants.TEXTURE_EXTENSION_AMOUNT);

			return finalTexture;
		}
	}
}

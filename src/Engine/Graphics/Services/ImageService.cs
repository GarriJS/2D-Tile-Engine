using Engine.Core.Constants;
using Engine.Core.Textures.Services.Contracts;
using Engine.DiskModels.Drawing;
using Engine.Graphics.Models;
using Engine.Graphics.Services.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
		public Image GetImageFromModel(ImageModel imageModel)
		{
			return this.GetImageFromModel<Image>(imageModel);
		}

		/// <summary>
		/// Gets the image from the model.
		/// </summary>
		/// <param name="imageModel">The image model.</param>
		/// <returns>The image.</returns>
		public T GetImageFromModel<T>(ImageModel imageModel) where T : Image
		{
			var textureService = this._gameServices.GetService<ITextureService>();

			if (false == textureService.TryGetTexture(imageModel.TextureName, out var texture))
			{
				texture = textureService.DebugTexture;
			}

			var image = imageModel switch
			{   
				TiledImageModel fillImageModel => new TiledImage
				{
					TextureName = imageModel.TextureName,
					TextureBox = imageModel.TextureBox,
					Texture = texture,
					FillBox = fillImageModel.FillBox,
				},
				FillImageModel stretchImageModel => new FillImage
				{
					TextureName = imageModel.TextureName,
					TextureBox = imageModel.TextureBox,
					Texture = texture,
					FillBox = stretchImageModel.FillBox,
				},
				_ => new Image
				{
					TextureName = imageModel.TextureName,
					TextureBox = imageModel.TextureBox,
					Texture = texture
				}
			};

			return image as T;
		}

		/// <summary>
		/// Gets the image.
		/// </summary>
		/// <param name="textureName">The texture name.</param>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <returns>The image.</returns>
		public Image GetImage(string textureName, int width, int height)
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

			return new Image
			{
				TextureName = textureName,
				TextureBox = new Rectangle
				{
					X = TextureConstants.TEXTURE_EXTENSION_AMOUNT,
					Y = TextureConstants.TEXTURE_EXTENSION_AMOUNT,
					Width = width,
					Height = height
				},
				Texture = texture
			};
		}

		/// <summary>
		/// Gets the image from the image by parts model.
		/// </summary>
		/// <param name="imageByPartsModel">The image by parts model.</param>
		/// <returns>The image.</returns>
		public Image GetImageFromImageByPartsModel(ImageByPartsModel imageByPartsModel)
		{
			if ((imageByPartsModel?.Images is null) ||
				(0 == imageByPartsModel.Images.Length) ||
				(true == imageByPartsModel.Images.Any(e => e is null || 0 == e.Length)) ||
				(false == imageByPartsModel.Images.All(e => e.Length == imageByPartsModel.Images[0].Length)))
			{
				return null;
			}

			var images = new Image[imageByPartsModel.Images.Length][];

			for (int i = 0; i < images.Length; i++)
			{
				images[i] = new Image[imageByPartsModel.Images[i].Length];

				for (int j = 0; j < images[i].Length; j++)
				{
					images[i][j] = this.GetImageFromModel(imageByPartsModel.Images[i][j]);
				}
			}

			var combinedTexture = this.CombineTextures(images);
			var result = new Image
			{
				TextureName = "Combined_Image",
				TextureBox = new Rectangle
				{
					X = TextureConstants.TEXTURE_EXTENSION_AMOUNT,
					Y = TextureConstants.TEXTURE_EXTENSION_AMOUNT,
					Width = combinedTexture.Width - (TextureConstants.TEXTURE_EXTENSION_AMOUNT * 2),
					Height = combinedTexture.Height - (TextureConstants.TEXTURE_EXTENSION_AMOUNT * 2)
				},
				Texture = combinedTexture
			};

			return result;
		}

		/// <summary>
		/// Combines the textures into one texture.
		/// </summary>
		/// <param name="images">The image.</param>
		/// <returns>The combined texture.</returns>
		public Texture2D CombineTextures(Image[][] images)
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
				totalWidth += images[0][c].TextureBox.Width;
			}

			for (int r = 0; r < images.Length; r++)
			{
				totalHeight += images[r][0].TextureBox.Height;
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
						horizontalOffset += img?.TextureBox.Width ?? 0;

						continue;
					}

					var destinationRectangle = new Rectangle
					{
						X = horizontalOffset,
						Y = verticalOffset,
						Width = img.TextureBox.Width,
						Height = img.TextureBox.Height
					};
					spriteBatch.Draw(
						img.Texture,
						destinationRectangle,
						img.TextureBox,
						Color.White
					);
					horizontalOffset += img.TextureBox.Width;
				}

				verticalOffset += images[r][0].TextureBox.Height;
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

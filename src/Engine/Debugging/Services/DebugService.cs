using Engine.Debugging.Services.Contracts;
using Engine.Drawing.Models;
using Engine.Physics.Models;
using Engine.RunTime.Services.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Engine.Debugging.Services
{
	/// <summary>
	/// Represents a debug service.
	/// </summary>
	/// <remarks>
	/// Initializes a new instance of the debug service.
	/// </remarks>
	/// <param name="gameServices">The game services.</param>
	public class DebugService(GameServiceContainer gameServices) : IDebugService
	{
		private readonly GameServiceContainer _gameServices = gameServices;

		/// <summary>
		/// Gets or sets a value indicating whether the screen area indicators are enabled.
		/// </summary>
		private bool ScreenAreaIndicatorsEnabled { get => this.ScreenAreaIndicatorImages.Count != 0; }

		/// <summary>
		/// Gets or sets the screen area indicator images.
		/// </summary>
		private List<IndependentImage> ScreenAreaIndicatorImages { get; set; } = []; 

		/// <summary>
		/// Toggles the screen area indicators.
		/// </summary>
		public void ToggleScreenAreaIndicators()
		{
			var runtimeDrawingService = this._gameServices.GetService<IRuntimeDrawService>();

			if (true == this.ScreenAreaIndicatorsEnabled)
			{
				foreach (var screenAreaIndicatorImage in this.ScreenAreaIndicatorImages)
				{
					runtimeDrawingService.RemoveDrawable(screenAreaIndicatorImage.DrawLayer, screenAreaIndicatorImage);
				}

				this.ScreenAreaIndicatorImages = [];
			}
			else
			{
				var graphicsDeviceService = this._gameServices.GetService<IGraphicsDeviceService>();
				var screenWidth = graphicsDeviceService.GraphicsDevice.PresentationParameters.BackBufferWidth;
				var screenHeight = graphicsDeviceService.GraphicsDevice.PresentationParameters.BackBufferHeight;
				var widthTexture = this.GetLineTexture(false, screenWidth, Color.MonoGameOrange);
				var heightTexture = this.GetLineTexture(false, screenHeight, Color.MonoGameOrange);

				for (int i = screenHeight / 3; i < screenHeight; i += screenHeight / 3)
				{
					var widthImage = new IndependentImage
					{
						TextureName = "screen width line",
						TextureBox = new Rectangle(0, 0, screenWidth, 1),
						Texture = widthTexture,
						Position = new Position
						{
							Coordinates = new Vector2(0, i)
						},
						DrawLayer = 0,
					}; 
					
					var nextWidthImage = new IndependentImage
					{
						TextureName = "screen width line",
						TextureBox = new Rectangle(0, 0, screenWidth, 1),
						Texture = widthTexture,
						Position = new Position
						{
							Coordinates = new Vector2(0, i + 1)
						},
						DrawLayer = 0,
					};

					runtimeDrawingService.AddDrawable(widthImage.DrawLayer, widthImage);
					runtimeDrawingService.AddDrawable(nextWidthImage.DrawLayer, nextWidthImage);
					this.ScreenAreaIndicatorImages.Add(widthImage);
					this.ScreenAreaIndicatorImages.Add(nextWidthImage);
				}

				for (int i = screenWidth / 4; i < screenWidth; i += screenWidth / 4)
				{
					var heightImage = new IndependentImage
					{
						TextureName = "screen height line",
						TextureBox = new Rectangle(0, 0, 1, screenHeight),
						Texture = heightTexture,
						Position = new Position
						{
							Coordinates = new Vector2(i, 0)
						},
						DrawLayer = 0,
					};

					var nextHeightImage = new IndependentImage
					{
						TextureName = "screen height line",
						TextureBox = new Rectangle(0, 0, 1, screenHeight),
						Texture = heightTexture,
						Position = new Position
						{
							Coordinates = new Vector2(i + 1, 0)
						},
						DrawLayer = 0,
					};

					runtimeDrawingService.AddDrawable(heightImage.DrawLayer, heightImage);
					runtimeDrawingService.AddDrawable(nextHeightImage.DrawLayer, nextHeightImage);
					this.ScreenAreaIndicatorImages.Add(heightImage);
					this.ScreenAreaIndicatorImages.Add(nextHeightImage);
				}
			}
		}

		/// <summary>
		/// Gets the line texture.
		/// </summary>
		/// <param name="verticalLine">A value indicating whether the line texture will be vertical.</param>
		/// <param name="length">The length.</param>
		/// <param name="color">The color.</param>
		/// <returns>The line texture.</returns>
		public Texture2D GetLineTexture(bool verticalLine, int length, Color color)
		{
			var graphicsDeviceService = this._gameServices.GetService<IGraphicsDeviceService>();
			int width = 1, height = 1;

			if (true == verticalLine)
			{
				height = length;
			}
			else
			{
				width = length;
			}

			var texture = new Texture2D(graphicsDeviceService.GraphicsDevice, width, height);
			var colorData = new Color[length];

			for (int i = 0; i < colorData.Length; i++)
			{
				colorData[i] = color;
			}

			texture.SetData(colorData);

			return texture;
		}
	}
}

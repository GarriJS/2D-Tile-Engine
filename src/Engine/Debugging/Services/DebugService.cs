using Engine.Core.Fonts.Contracts;
using Engine.Debugging.Models;
using Engine.Debugging.Services.Contracts;
using Engine.Graphics.Models;
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
		/// Gets or sets the performance rate counter.
		/// </summary>
		private PerformanceRateCounter PerformanceRateCounter { get; set; }

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
					runtimeDrawingService.RemoveDrawable(screenAreaIndicatorImage);
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

					runtimeDrawingService.AddDrawable(widthImage);
					runtimeDrawingService.AddDrawable(nextWidthImage);
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

					runtimeDrawingService.AddDrawable(heightImage);
					runtimeDrawingService.AddDrawable(nextHeightImage);
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

		/// <summary>
		/// Toggles the performance rate counter.
		/// </summary>
		public void TogglePerformanceRateCounter()
		{
			var runtimeDrawingService = this._gameServices.GetService<IRuntimeDrawService>();
			var runtimeUpdateService = this._gameServices.GetService<IRuntimeUpdateService>();
			var fontService = this._gameServices.GetService<IFontService>();

			if (null == this.PerformanceRateCounter)
			{
				var spriteFont = fontService.DebugSpriteFont;

				if (null == spriteFont)
				{
					return;
				}

				this.PerformanceRateCounter = new PerformanceRateCounter
				{
					IsActive = true,
					DrawLayer = 0,
					UpdateOrder = 0,
					FpsText = "0",
					LastFrameTime = null,
					Font = spriteFont,
					Position = new Position
					{
						Coordinates = new Vector2(5, 0)
					}
				};
			}

			if (true == this.PerformanceRateCounter.IsActive)
			{
				runtimeDrawingService.AddOverlaidDrawable(this.PerformanceRateCounter);
				runtimeUpdateService.AddUpdateable(this.PerformanceRateCounter);
			}
			else
			{
				runtimeDrawingService.RemoveOverlaidDrawable(this.PerformanceRateCounter);
				runtimeUpdateService.RemoveUpdateable(this.PerformanceRateCounter);
			}
		}
	}
}

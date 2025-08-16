using Engine.Core.Fonts.Contracts;
using Engine.Debugging.Models;
using Engine.Debugging.Services.Contracts;
using Engine.DiskModels.Drawing;
using Engine.DiskModels.Physics;
using Engine.Graphics.Models;
using Engine.Graphics.Services.Contracts;
using Engine.Physics.Services.Contracts;
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
		private bool ScreenAreaIndicatorsEnabled { get => 0 != this.ScreenAreaIndicatorImages.Count; }

		/// <summary>
		/// Gets the debug draw layer.
		/// </summary>
		public int DebugDrawLayer { get; } = 999;

		/// <summary>
		/// Gets the debug update layer.
		/// </summary>
		public int DebugUpdateOrder { get; } = 999;

		/// <summary>
		/// Gets or sets the FPS counter;
		/// </summary>
		private FpsCounter FpsCounter { get; set; }

		/// <summary>
		/// Gets or sets TPS counter.
		/// </summary>
		private TpsCounter TpsCounter { get; set; }

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
				var imageService = this._gameServices.GetService<IImageService>();

				var screenWidth = graphicsDeviceService.GraphicsDevice.PresentationParameters.BackBufferWidth;
				var screenHeight = graphicsDeviceService.GraphicsDevice.PresentationParameters.BackBufferHeight;

				for (int i = screenHeight / 3; i < screenHeight; i += screenHeight / 3)
				{
					var widthImageModel = new IndependentImageModel
					{
						TextureName = "debug",
						TextureBox = new Rectangle
						{
							X = 0,
							Y = 0,
							Width = screenWidth, 
							Height = 1
						},
						Position = new PositionModel
						{
							X = 0,
							Y = i
						}
					};

					var widthImage = imageService.GetImageFromModel<IndependentImage>(widthImageModel);
					var nextWidthImageModel = new IndependentImageModel
					{
						TextureName = "debug",
						TextureBox = new Rectangle
						{
							X = 0,
							Y = 0,
							Width = screenWidth,
							Height = 1
						},
						Position = new PositionModel
						{
							X = 0,
							Y = i + 1
						}
					};

					var nextWidthImage = imageService.GetImageFromModel<IndependentImage>(nextWidthImageModel);
					runtimeDrawingService.AddDrawable(widthImage);
					runtimeDrawingService.AddDrawable(nextWidthImage);
					this.ScreenAreaIndicatorImages.Add(widthImage);
					this.ScreenAreaIndicatorImages.Add(nextWidthImage);
				}

				for (int i = screenWidth / 4; i < screenWidth; i += screenWidth / 4)
				{
					var heightImageModel = new IndependentImageModel
					{
						TextureName = "debug",
						TextureBox = new Rectangle
						{
							X = 0,
							Y = 0,
							Width = 1,
							Height = screenHeight
						},
						Position = new PositionModel
						{
							X = i,
							Y = 0
						}
					};

					var heightImage = imageService.GetImageFromModel<IndependentImage>(heightImageModel);
					var nextHeightImageModel = new IndependentImageModel
					{
						TextureName = "debug",
						TextureBox = new Rectangle
						{
							X = 0,
							Y = 0,
							Width = 1,
							Height = screenHeight
						},
						Position = new PositionModel
						{
							X = i + 1,
							Y = 0
						}
					};

					var nextHeightImage = imageService.GetImageFromModel<IndependentImage>(nextHeightImageModel);
					runtimeDrawingService.AddDrawable(heightImage);
					runtimeDrawingService.AddDrawable(nextHeightImage);
					this.ScreenAreaIndicatorImages.Add(heightImage);
					this.ScreenAreaIndicatorImages.Add(nextHeightImage);
				}
			}
		}

		/// <summary>
		/// Toggles the performance rate counter.
		/// </summary>
		public void TogglePerformanceRateCounter()
		{
			var runtimeOverlaidDrawService = this._gameServices.GetService<IRuntimeOverlaidDrawService>();
			var runtimeUpdateService = this._gameServices.GetService<IRuntimeUpdateService>();
			var fontService = this._gameServices.GetService<IFontService>();
			var positionService = this._gameServices.GetService<IPositionService>();

			if ((null == this.FpsCounter) ||
				(null == this.TpsCounter))
			{
				var spriteFont = fontService.DebugSpriteFont;

				if (null == spriteFont)
				{
					return;
				}

				var fpsPositionModel = new PositionModel
				{
					X = 5,
					Y = 0,
				};

				var fpsPosition = positionService.GetPositionFromModel(fpsPositionModel);
				this.FpsCounter = new FpsCounter
				{
					IsActive = true,
					DrawLayer = this.DebugDrawLayer,
					LastFrameTime = null,
					Font = spriteFont,
					Position = fpsPosition
				};

				var tpsPositionModel = new PositionModel
				{
					X = 5,
					Y = 0,
				};

				var tpsPosition = positionService.GetPositionFromModel(tpsPositionModel);

				this.TpsCounter = new TpsCounter
				{
					IsActive = true,
					DrawLayer = this.DebugDrawLayer,
					UpdateOrder = this.DebugUpdateOrder,
					Font = spriteFont,
					Position = tpsPosition
				};
			}

			if (true == this.TpsCounter.IsActive)
			{
				runtimeOverlaidDrawService.AddDrawable(this.FpsCounter);
				runtimeOverlaidDrawService.AddDrawable(this.TpsCounter);
				runtimeUpdateService.AddUpdateable(this.TpsCounter);
			}
			else
			{
				runtimeOverlaidDrawService.RemoveDrawable(this.FpsCounter);
				runtimeOverlaidDrawService.RemoveDrawable(this.TpsCounter);
				runtimeUpdateService.RemoveUpdateable(this.TpsCounter);
			}
		}
	}
}

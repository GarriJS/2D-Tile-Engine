using Engine.Core.Fonts.Services.Contracts;
using Engine.Debugging.Models;
using Engine.Debugging.Services.Contracts;
using Engine.DiskModels.Drawing;
using Engine.DiskModels.Physics;
using Engine.Graphics.Enum;
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
		/// Gets or sets a value indicating whether the performance counters are active.
		/// </summary>
		private bool PerformanceCountersActive { get; set; } = false;

		/// <summary>
		/// Gets or sets the FPS counter;
		/// </summary>
		private FpsCounter FpsCounter { get; set; }

		/// <summary>
		/// Gets or sets TPS counter.
		/// </summary>
		private TpsCounter TpsCounter { get; set; }

		/// <summary>
		/// Gets or sets the debug draw runtime.
		/// </summary>
		public DebugDrawRuntime DebugDrawRuntime { get; private set; }
		
		/// <summary>
		/// Gets or sets the debug overlaid draw runtime.
		/// </summary>
		public DebugOverlaidDrawRuntime DebugOverlaidDrawRuntime { get; private set; }
		
		/// <summary>
		/// Gets or sets the debug update runtime.
		/// </summary>
		public DebugUpdateRuntime DebugUpdateRuntime { get; private set; }

		/// <summary>
		/// Gets or sets the screen area indicator images.
		/// </summary>
		private List<IndependentGraphic> ScreenAreaIndicatorImages { get; set; } = [];

		/// <summary>
		/// Loads the content.
		/// </summary>
		public void LoadContent()
		{
			var runTimeDrawService = this._gameServices.GetService<IRuntimeDrawService>();
			var runTimeOverlaidDrawService = this._gameServices.GetService<IRuntimeOverlaidDrawService>();
			var runtimeUpdateSerive = this._gameServices.GetService<IRuntimeUpdateService>();
			this.DebugDrawRuntime = new();
			this.DebugOverlaidDrawRuntime = new();
			this.DebugUpdateRuntime = new();
			this.DebugDrawRuntime.DrawLayer = IDebugService.DebugDrawLayer;
			this.DebugOverlaidDrawRuntime.DrawLayer = IDebugService.DebugDrawLayer;
			this.DebugUpdateRuntime.UpdateOrder = IDebugService.DebugUpdateOrder;
			runTimeDrawService.AddDrawable(this.DebugDrawRuntime);
			runTimeOverlaidDrawService.AddDrawable(this.DebugOverlaidDrawRuntime);
			runtimeUpdateSerive.AddUpdateable(this.DebugUpdateRuntime);
		}

		/// <summary>
		/// Toggles the screen area indicators.
		/// </summary>
		public void ToggleScreenAreaIndicators()
		{
			var runtimeOverlaidDrawService = this._gameServices.GetService<IRuntimeOverlaidDrawService>();

			if (true == this.ScreenAreaIndicatorsEnabled)
			{
				foreach (var screenAreaIndicatorImage in this.ScreenAreaIndicatorImages)
					runtimeOverlaidDrawService.RemoveDrawable(screenAreaIndicatorImage);

				this.ScreenAreaIndicatorImages = [];
			}
			else
			{
				var graphicsDeviceService = this._gameServices.GetService<IGraphicsDeviceService>();
				var independentGraphicService = this._gameServices.GetService<IIndependentGraphicService>();
				var screenWidth = graphicsDeviceService.GraphicsDevice.PresentationParameters.BackBufferWidth;
				var screenHeight = graphicsDeviceService.GraphicsDevice.PresentationParameters.BackBufferHeight;

				for (int i = screenHeight / 3; i < screenHeight; i += screenHeight / 3)
				{
					var widthImageModel = new IndependentGraphicModel
					{
						Position = new PositionModel
						{
							X = 0,
							Y = i
						},
						Graphic = new SimpleImageModel
						{
							TextureName = "debug",
							TextureRegion = new TextureRegionModel
							{
								TextureRegionType = TextureRegionType.Simple,
								TextureBox = new Rectangle
								{
									X = 0,
									Y = 0,
									Width = screenWidth,
									Height = 1
								},
								DisplayArea = new SubAreaModel
								{ 
									Width = screenWidth,
									Height = i	
								}
							}
						}
					};
					var widthImage = independentGraphicService.GetIndependentGraphicFromModel(widthImageModel);
					var nextWidthImageModel = new IndependentGraphicModel
					{
						Position = new PositionModel
						{
							X = 0,
							Y = i + 1
						},
						Graphic = new SimpleImageModel
						{
							TextureName = "debug",
							TextureRegion = new TextureRegionModel
							{
								TextureRegionType = TextureRegionType.Simple,
								TextureBox = new Rectangle
								{
									X = 0,
									Y = 0,
									Width = screenWidth,
									Height = 1
								},
								DisplayArea = new SubAreaModel
								{
									Width = screenWidth,
									Height = i
								}
							}
						}
					};
					var nextWidthImage = independentGraphicService.GetIndependentGraphicFromModel(nextWidthImageModel);
					runtimeOverlaidDrawService.AddDrawable(widthImage);
					runtimeOverlaidDrawService.AddDrawable(nextWidthImage);
					this.ScreenAreaIndicatorImages.Add(widthImage);
					this.ScreenAreaIndicatorImages.Add(nextWidthImage);
				}

				for (int i = screenWidth / 4; i < screenWidth; i += screenWidth / 4)
				{
					var heightImageModel = new IndependentGraphicModel
					{
						Position = new PositionModel
						{
							X = i,
							Y = 0
						},
						Graphic = new SimpleImageModel
						{
							TextureName = "debug",
							TextureRegion = new TextureRegionModel
							{
								TextureRegionType = TextureRegionType.Simple,
								TextureBox = new Rectangle
								{
									X = 0,
									Y = 0,
									Width = 1,
									Height = screenHeight
								},
								DisplayArea = new SubAreaModel
								{
									Width = 1,
									Height = screenHeight
								}
							}
						}
					};
					var heightImage = independentGraphicService.GetIndependentGraphicFromModel(heightImageModel);
					var nextHeightImageModel = new IndependentGraphicModel
					{
						Position = new PositionModel
						{
							X = i + 1,
							Y = 0
						},
						Graphic = new SimpleImageModel
						{
							TextureName = "debug",
							TextureRegion = new TextureRegionModel
							{
								TextureRegionType = TextureRegionType.Simple,
								TextureBox = new Rectangle
								{
									X = 0,
									Y = 0,
									Width = 1,
									Height = screenHeight
								},
								DisplayArea = new SubAreaModel
								{
									Width = 1,
									Height = screenHeight
								}
							}
						}
					};
					var nextHeightImage = independentGraphicService.GetIndependentGraphicFromModel(nextHeightImageModel);
					runtimeOverlaidDrawService.AddDrawable(heightImage);
					runtimeOverlaidDrawService.AddDrawable(nextHeightImage);
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
			var fontService = this._gameServices.GetService<IFontService>();
			var positionService = this._gameServices.GetService<IPositionService>();

			if ((null == this.FpsCounter) ||
				(null == this.TpsCounter))
			{
				var spriteFont = fontService.DebugSpriteFont;

				if (null == spriteFont)
					return;

				var fpsPositionModel = new PositionModel
				{
					X = 5,
					Y = 0,
				};
				var fpsPosition = positionService.GetPositionFromModel(fpsPositionModel);
				this.FpsCounter = new FpsCounter
				{
					DrawLayer = IDebugService.DebugDrawLayer,
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
					DrawLayer = IDebugService.DebugDrawLayer,
					UpdateOrder = IDebugService.DebugUpdateOrder,
					Font = spriteFont,
					Position = tpsPosition
				};
			}

			if (false == this.PerformanceCountersActive)
			{
				this.DebugOverlaidDrawRuntime.AddDrawable(this.FpsCounter);
				this.DebugOverlaidDrawRuntime.AddDrawable(this.TpsCounter);
				this.DebugUpdateRuntime.AddUpdateable(this.TpsCounter);
				this.PerformanceCountersActive = true;
			}
			else
			{
				this.DebugOverlaidDrawRuntime.RemoveDrawable(this.FpsCounter);
				this.DebugOverlaidDrawRuntime.RemoveDrawable(this.TpsCounter);
				this.DebugUpdateRuntime.RemoveUpdateable(this.TpsCounter);
				this.PerformanceCountersActive = false;
			}
		}
	}
}

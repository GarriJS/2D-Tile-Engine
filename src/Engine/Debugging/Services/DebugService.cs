using Engine.Core.Fonts.Services.Contracts;
using Engine.Core.State.Contracts;
using Engine.Debugging.Models;
using Engine.Debugging.Services.Contracts;
using Engine.DiskModels.Physics;
using Engine.Physics.Services.Contracts;
using Engine.RunTime.Services.Contracts;
using Microsoft.Xna.Framework;

namespace Engine.Debugging.Services
{
	/// <summary>
	/// Represents a debug service.
	/// </summary>
	/// <remarks>
	/// Initializes a new instance of the debug service.
	/// </remarks>
	/// <param name="gameServices">The game services.</param>
	sealed public class DebugService(GameServiceContainer gameServices) : IDebugService
	{
		readonly private GameServiceContainer _gameServices = gameServices;

		/// <summary>
		/// The debug flag name. TODO find a proper place.
		/// </summary>
		static readonly public string DebugFlagName = "DebugModeActive";

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
		/// Configures the service.
		/// </summary>
		public void ConfigureService()
		{
			var runTimeDrawService = this._gameServices.GetService<IRuntimeDrawService>();
			var runTimeOverlaidDrawService = this._gameServices.GetService<IRuntimeOverlaidDrawService>();
			var runtimeUpdateSerive = this._gameServices.GetService<IRuntimeUpdateService>();
			this.DebugDrawRuntime = new DebugDrawRuntime 
			{
				DrawLayer = IDebugService.DebugDrawLayer
			};
			this.DebugOverlaidDrawRuntime = new DebugOverlaidDrawRuntime
			{
				DrawLayer = IDebugService.DebugDrawLayer
			};
			this.DebugUpdateRuntime = new DebugUpdateRuntime
			{
				UpdateOrder = IDebugService.DebugUpdateOrder
			};
			runTimeDrawService.AddDrawable(this.DebugDrawRuntime);
			runTimeOverlaidDrawService.AddDrawable(this.DebugOverlaidDrawRuntime);
			runtimeUpdateSerive.AddUpdateable(this.DebugUpdateRuntime);
			var gameStateService = this._gameServices.GetService<IGameStateService>();
			gameStateService.SubscribeToFlag(DebugFlagName, this.DebugFlagSubscription);
		}

		/// <summary>
		/// Does the post load initialization.
		/// </summary>
		public void PostLoadInitialize()
		{
			var fontService = this._gameServices.GetService<IFontService>();
			var positionService = this._gameServices.GetService<IPositionService>();
			var spriteFont = fontService.DebugSpriteFont;

			if (spriteFont is null)
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
				LastTickTime = null,
				Position = tpsPosition
			};
			var gameStateService = this._gameServices.GetService<IGameStateService>();

			if (true == gameStateService.CheckGameStateFlagValue(DebugService.DebugFlagName, out var debugModeActive))
				this.SetPerformanceRateCounterEnabled(debugModeActive);
		}

		/// <summary>
		/// The debug flag subscription.
		/// </summary>
		/// <param name="flagValue">The flag value.</param>
		private void DebugFlagSubscription(bool flagValue)
		{
			this.SetPerformanceRateCounterEnabled(flagValue);
		}

		/// <summary>
		/// Sets the performance rate counter activity.
		/// </summary>
		/// <param name="enable">A value indicating whether to enable the performance counters.</param>
		public void SetPerformanceRateCounterEnabled(bool enable)
		{
			if (enable == this.PerformanceCountersActive)
				return;

			if (false == this.PerformanceCountersActive)
			{
				this.DebugOverlaidDrawRuntime.AddDrawable(this.FpsCounter);
				this.DebugOverlaidDrawRuntime.AddDrawable(this.TpsCounter);
				this.DebugUpdateRuntime.AddUpdateable(this.TpsCounter);
			}
			else
			{
				this.DebugOverlaidDrawRuntime.RemoveDrawable(this.FpsCounter);
				this.DebugOverlaidDrawRuntime.RemoveDrawable(this.TpsCounter);
				this.DebugUpdateRuntime.RemoveUpdateable(this.TpsCounter);
			}

			this.PerformanceCountersActive = enable;
		}
	}
}

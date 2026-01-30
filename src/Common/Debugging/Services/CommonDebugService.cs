using Common.Debugging.Services.Contracts;
using Common.UserInterface.Services.Contracts;
using Engine.Core.State.Contracts;
using Engine.Debugging.Services;
using Engine.Debugging.Services.Contracts;
using Engine.Monogame;
using Engine.RunTime.Models;
using Microsoft.Xna.Framework;
using System.Linq;

namespace Common.Debugging.Services
{
	/// <summary>
	/// Represents a common debug service.
	/// </summary>
	/// <remarks>
	/// Initializes a new instance of the common debug service.
	/// </remarks>
	/// <param name="gameServices">The game services.</param>
	public class CommonDebugService(GameServiceContainer gameServices) : ICommonDebugService
	{
		private readonly GameServiceContainer _gameServices = gameServices;

		/// <summary>
		/// Gets or sets a value indicating whether the screen area indicators are enabled.
		/// </summary>
		private bool ScreenAreaIndicatorsEnabled { get; set; } = false;

		/// <summary>
		/// Gets or sets the screen area drawable rectangles.
		/// </summary>
		private DrawableRectangle[] ScreenAreaDrawableRectangles { get; set; } = [];

		/// <summary>
		/// Does the post load initialization.
		/// </summary>
		public void PostLoadInitialize()
		{
			var uiScreenAreaService = this._gameServices.GetService<IUserInterfaceScreenZoneService>();
			var uiScreenZonesRectangles = uiScreenAreaService.UserInterfaceScreenZones.Values.Select(e => e.Area.ToRectangle);
			var drawableRectangles = uiScreenZonesRectangles.SelectMany(e => e.GetEdgeRectangles(1)).Select(e => new DrawableRectangle
			{
				DrawLayer = IDebugService.DebugDrawLayer,
				Color = Color.MonoGameOrange,
				Rectangle = e
			}).ToArray();
			this.ScreenAreaDrawableRectangles = drawableRectangles;
			var gameStateService = this._gameServices.GetService<IGameStateService>();

			if (true == gameStateService.CheckGameStateFlagValue(DebugService.DebugFlagName, out var debugModeActive))
				this.DebugFlagSubscription(debugModeActive);
		}

		/// <summary>
		/// The debug flag subscription.
		/// </summary>
		/// <param name="flagValue">The flag value.</param>
		private void DebugFlagSubscription(bool flagValue)
		{
			this.SetScreenAreaIndicatorsEnabled(flagValue);
		}

		/// <summary>
		/// Sets the performance rate counter activity.
		/// </summary>
		/// <param name="enable">A value indicating whether to enable the performance counters.</param>
		public void SetScreenAreaIndicatorsEnabled(bool enable)
		{
			if (enable == this.ScreenAreaIndicatorsEnabled)
				return;

			var debugService = this._gameServices.GetService<IDebugService>();

			if (true == enable)
				foreach (var drawableRectangle in this.ScreenAreaDrawableRectangles ?? [])
					debugService.DebugOverlaidDrawRuntime.AddDrawable(drawableRectangle);
			else
				foreach (var drawableRectangle in this.ScreenAreaDrawableRectangles ?? [])
					debugService.DebugOverlaidDrawRuntime.RemoveDrawable(drawableRectangle);

			this.ScreenAreaIndicatorsEnabled = enable;
		}
	}
}

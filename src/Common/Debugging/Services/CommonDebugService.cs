using Common.Debugging.Services.Contracts;
using Common.UserInterface.Models;
using Common.UserInterface.Services.Contracts;
using Engine.Core.State.Contracts;
using Engine.Debugging.Services;
using Engine.Debugging.Services.Contracts;
using Engine.Monogame;
using Engine.RunTime.Models;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
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
		/// Gets or sets a list of the active debug user interface zones.
		/// </summary>
		private List<UiZone> ActiveDebugUserInterfaceZones { get; set; } = [];

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
			gameStateService.SubscribeToFlag(DebugService.DebugFlagName, this.DebugFlagSubscription);

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

		/// <summary>
		/// Adds the user interface zone from debugging.
		/// </summary>
		/// <param name="uiZone">The user interface zone.</param>
		public void AddDebugUserInterfaceZone(UiZone uiZone)
		{
			if (true == this.ActiveDebugUserInterfaceZones.Contains(uiZone))
				return;

			var debugService = this._gameServices.GetService<IDebugService>();
			debugService.DebugOverlaidDrawRuntime.AddDrawable(uiZone);
			this.ActiveDebugUserInterfaceZones.Add(uiZone);		
		}

		/// <summary>
		/// Removes the user interface zone from debugging.
		/// </summary>
		/// <param name="uiZone">The user interface zone.</param>
		public void RemoveDebugUserInterfaceZone(UiZone uiZone)
		{
			if (false == this.ActiveDebugUserInterfaceZones.Contains(uiZone))
				return;

			var debugService = this._gameServices.GetService<IDebugService>();
			debugService.DebugOverlaidDrawRuntime.RemoveDrawable(uiZone);
			this.ActiveDebugUserInterfaceZones.Remove(uiZone);
		}

		/// <summary>
		/// Adds the user interface zone rectangles.
		/// </summary>
		public void ClearDebugUserInterfaceZones()
		{
			var debugService = this._gameServices.GetService<IDebugService>();

			foreach (var uiZone in this.ActiveDebugUserInterfaceZones ?? [])
				debugService.DebugOverlaidDrawRuntime.RemoveDrawable(uiZone);

			this.ActiveDebugUserInterfaceZones.Clear();
		}
	}
}

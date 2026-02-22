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
	sealed public class CommonDebugService(GameServiceContainer gameServices) : ICommonDebugService
	{
		readonly private GameServiceContainer _gameServices = gameServices;

		/// <summary>
		/// Gets or sets a value indicating whether the screen area indicators are enabled.
		/// </summary>
		private bool ScreenAreaIndicatorsEnabled { get; set; } = false;

		/// <summary>
		/// The screen area drawable rectangles.
		/// </summary>
		readonly private List<DrawableRectangle> _screenAreaDrawableRectangles = [];

		/// <summary>
		/// The list of the active debug user interface zones.
		/// </summary>
		readonly private List<UiZone> _activeDebugUserInterfaceZones = [];

		/// <summary>
		/// The list of the active debug user interface modals.
		/// </summary>
		readonly private List<UiModal> _activeDebugUserInterfaceModals = [];

		/// <summary>
		/// Does the post load initialization.
		/// </summary>
		public void PostLoadInitialize()
		{
			var uiScreenService = this._gameServices.GetService<IUiScreenService>();
			var uiScreenZonesRectangles = uiScreenService.UiScreenZones.Values.Select(e => e.Area.ToRectangle);
			var drawableRectangles = uiScreenZonesRectangles.SelectMany(e => e.GetEdgeRectangles(1)).Select(e => new DrawableRectangle
			{
				DrawLayer = IDebugService.DebugDrawLayer,
				Color = Color.MonoGameOrange,
				Rectangle = e
			}).ToArray();
			this._screenAreaDrawableRectangles.AddRange(drawableRectangles);
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
				foreach (var drawableRectangle in this._screenAreaDrawableRectangles ?? [])
					debugService.DebugOverlaidDrawRuntime.AddDrawable(drawableRectangle);
			else
				foreach (var drawableRectangle in this._screenAreaDrawableRectangles ?? [])
					debugService.DebugOverlaidDrawRuntime.RemoveDrawable(drawableRectangle);

			this.ScreenAreaIndicatorsEnabled = enable;
		}

		/// <summary>
		/// Adds the user interface zone from debugging.
		/// </summary>
		/// <param name="uiZone">The user interface zone.</param>
		public void AddDebugUserInterfaceZone(UiZone uiZone)
		{
			if (true == this._activeDebugUserInterfaceZones.Contains(uiZone))
				return;

			var debugService = this._gameServices.GetService<IDebugService>();
			debugService.DebugOverlaidDrawRuntime.AddDrawable(uiZone);
			this._activeDebugUserInterfaceZones.Add(uiZone);		
		}

		/// <summary>
		/// Removes the user interface zone from debugging.
		/// </summary>
		/// <param name="uiZone">The user interface zone.</param>
		public void RemoveDebugUserInterfaceZone(UiZone uiZone)
		{
			if (false == this._activeDebugUserInterfaceZones.Contains(uiZone))
				return;

			var debugService = this._gameServices.GetService<IDebugService>();
			debugService.DebugOverlaidDrawRuntime.RemoveDrawable(uiZone);
			this._activeDebugUserInterfaceZones.Remove(uiZone);
		}

		/// <summary>
		/// Clears the debug user interface zones.
		/// </summary>
		public void ClearDebugUserInterfaceZones()
		{
			var debugService = this._gameServices.GetService<IDebugService>();

			foreach (var uiZone in this._activeDebugUserInterfaceZones ?? [])
				debugService.DebugOverlaidDrawRuntime.RemoveDrawable(uiZone);

			this._activeDebugUserInterfaceZones.Clear();
		}

		/// <summary>
		/// Adds the user interface modal from debugging.
		/// </summary>
		/// <param name="uiModal">The user interface modal.</param>
		public void AddDebugUserInterfaceModal(UiModal uiModal)
		{
			if (true == this._activeDebugUserInterfaceModals.Contains(uiModal))
				return;

			var debugService = this._gameServices.GetService<IDebugService>();
			debugService.DebugOverlaidDrawRuntime.AddDrawable(uiModal);
			this._activeDebugUserInterfaceModals.Add(uiModal);
		}

		/// <summary>
		/// Removes the user interface modal from debugging.
		/// </summary>
		/// <param name="uiModal">The user interface modal.</param>
		public void RemoveDebugUserInterfaceModal(UiModal uiModal)
		{
			if (false == this._activeDebugUserInterfaceModals.Contains(uiModal))
				return;

			var debugService = this._gameServices.GetService<IDebugService>();
			debugService.DebugOverlaidDrawRuntime.RemoveDrawable(uiModal);
			this._activeDebugUserInterfaceModals.Remove(uiModal);
		}

		/// <summary>
		/// Clears the debug user interface modals.
		/// </summary>
		public void ClearDebugUserInterfaceModals()
		{
			var debugService = this._gameServices.GetService<IDebugService>();

			foreach (var uiZone in this._activeDebugUserInterfaceModals ?? [])
				debugService.DebugOverlaidDrawRuntime.RemoveDrawable(uiZone);

			this._activeDebugUserInterfaceModals.Clear();
		}
	}
}

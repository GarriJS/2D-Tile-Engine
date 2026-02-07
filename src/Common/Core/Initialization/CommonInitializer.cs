using Common.Controls.Cursors.Constants;
using Common.Controls.Cursors.Models;
using Common.Tiling.Services.Contracts;
using Common.UserInterface.Models;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Common.Core.Initialization
{
    /// <summary>
    /// Represents a level editor initializer.
    /// </summary>
    static public class CommonInitializer
	{
		/// <summary>
		/// Gets the function providers. 
		/// </summary>
		/// <param name="gameServices">The game service.</param>
		/// <returns>The function providers.</returns>
		static public Dictionary<string, Delegate> GetFunctionProviders(GameServiceContainer gameServices)
		{
			var result = new Dictionary<string, Delegate>();

			foreach (var kv in GetCursorUpdaters(gameServices))
				result[kv.Key] = kv.Value;

			foreach (var kv in GetHoverEventProcessors(gameServices))
				result[kv.Key] = kv.Value;

			return result;
		}

		/// <summary>
		/// Gets the cursor updaters.
		/// </summary>
		/// <param name="gameService">The game service.</param>
		/// <returns>A dictionary of the cursor updaters.</returns>
		static public Dictionary<string, Action<Cursor, GameTime>> GetCursorUpdaters(GameServiceContainer gameService)
		{
			var tileService = gameService.GetService<ITileService>();
			var result = new Dictionary<string, Action<Cursor, GameTime>>
			{
				[CommonCursorUpdatersNames.TileGridCursorUpdater] = tileService.TileGridCursorUpdater
			};

			return result;
		}

		/// <summary>
		/// Gets the hover event processors.
		/// </summary>
		/// <param name="gameServices">The game services.</param>
		/// <returns>A dictionary of the hover event processors.</returns>
		static public Dictionary<string, Action<UiZone, Vector2>> GetHoverEventProcessors(GameServiceContainer gameServices)
		{
			var result = new Dictionary<string, Action<UiZone, Vector2>>
			{

			};

			return result;
		}
	}
}

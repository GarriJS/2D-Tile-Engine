using Common.Controls.Cursors.Models;
using Common.Controls.Cursors.Services.Contracts;
using Common.UserInterface.Constants;
using Common.UserInterface.Models;
using Common.UserInterface.Services.Contracts;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Common.Core.Initialization
{
    /// <summary>
    /// Represents a level editor initializer.
    /// </summary>
    public static class CommonInitializer
	{
		/// <summary>
		/// Gets the function providers. 
		/// </summary>
		/// <param name="gameServices">The game service.</param>
		/// <returns>The function providers.</returns>
		public static Dictionary<string, Delegate> GetFunctionProviders(GameServiceContainer gameServices)
		{
			var result = new Dictionary<string, Delegate>();

			foreach (var kv in GetInitialHoverEventProcessors(gameServices))
			{
				result[kv.Key] = kv.Value;
			}

			return result;
		}

		public static Dictionary<string, Action<Cursor, GameTime>> GetCursorUpdaters(GameServiceContainer gameService)
		{
			var cursorService = gameService.GetService<ICursorService>();

			return new Dictionary<string, Action<Cursor, GameTime>>
			{
				[] = cursorService.BasicCursorUpdater
			};
		}

		/// <summary>
		/// Gets the initial hover event processors.
		/// </summary>
		/// <param name="gameServices">The game services.</param>
		/// <returns>A dictionary of the hover event processors.</returns>
		public static Dictionary<string, Action<UiZone, Vector2>> GetInitialHoverEventProcessors(GameServiceContainer gameServices)
		{
			var uiService = gameServices.GetService<IUserInterfaceService>();

			return new Dictionary<string, Action<UiZone, Vector2>>
			{
				[CommonUiEventName.UserInterfaceZoneHover] = uiService.BasicUiZoneHoverEventProcessor
			};
		}
	}
}

using Common.Core.Constants;
using Common.UI.Models;
using Common.UI.Services.Contracts;
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
				result[kv.Key] = kv.Value;

			return result;
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
				[UiEventNameConstants.UserInterfaceZoneHover] = uiService.BasicUiZoneHoverEventProcessor,
			};
		}
	}
}

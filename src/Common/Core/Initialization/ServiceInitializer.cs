using Common.Tiling.Services;
using Common.Tiling.Services.Contracts;
using Engine;
using Engine.Core.Contracts;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace LevelEditor.Core.Initialization
{
	/// <summary>
	/// Represents a service initializer
	/// </summary>
	public static class ServiceInitializer
	{
		/// <summary>
		/// Gets the loadables.
		/// </summary>
		public static List<ILoadContent> Loadables { get; } = [];

		/// <summary>
		/// Initializes the game services.
		/// </summary>
		/// <param name="game">The game.</param>
		/// <returns>A value indicating whether if all services were initialized.</returns>
		public static bool InitializeServices(Game1 game)
		{
			var serviceContractPairs = GetServiceContractPairs(game);
			bool success = true;

			foreach (var (type, provider) in serviceContractPairs)
			{
				try
				{
					game.Services.AddService(type, provider);

					if (provider is GameComponent gameComponent)
					{
						game.Components.Add(gameComponent);
					}

					if (provider is ILoadContent loadable)
					{
						Loadables.Add(loadable);
					}
				}
				catch (Exception ex)
				{
					Debug.WriteLine($"Service initialization failed for {type}: {ex.Message}");
					success = false;
				}
			}

			return success;
		}

		/// <summary>
		/// Gets the service contract pairs.
		/// </summary>
		/// <param name="game">The game.</param>
		/// <returns>The service contract pairs.</returns>
		private static (Type type, object provider)[] GetServiceContractPairs(Game1 game)
		{
			return
			[
				(typeof(ITileService), new TileService(game.Services)),
			];
		}
	}
}

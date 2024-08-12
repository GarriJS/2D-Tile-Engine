using Game.Drawing.Services;
using Game.Drawing.Services.Contracts;
using Game.RunTime.Managers;
using Game.RunTime.Services;
using Game.RunTime.Services.Contracts;
using System;

namespace Game.Core.Initialization
{
	/// <summary>
	/// Represents a service initializer
	/// </summary>
	public static class ServiceInitializer
	{
		/// <summary>
		/// Gets the service contract pairs.
		/// </summary>
		/// <param name="game">The game.</param>
		/// <returns>The service contract pairs.</returns>
		private static (Type type, object provider)[] GetServiceContractPairs(Game1 game)
		{
			return new (Type type, object provider)[]
			{
				(typeof(IRuntimeUpdateService), new RuntimeUpdateManager(game)),
				(typeof(IRuntimeDrawService), new RuntimeDrawManager(game)),
				(typeof(IRandomService), new RandomService()),
				(typeof(IDrawingService), new DrawingService(game.Services)),
				(typeof(IUpdateService), new UpdateService(game.Services)),
			};
		}

		/// <summary>
		/// Initializes the game services.
		/// </summary>
		/// <param name="game">The game.</param>
		/// <returns>A value indicating whether if all services were initialized.</returns>
		public static bool InitializeServices(Game1 game)
		{
			var serviceContractPairs = GetServiceContractPairs(game);
			bool success = true;

			try
			{
				foreach (var (type, provider) in serviceContractPairs)
				{
					game.Services.AddService(type, provider);
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Service initialization failed: {ex.Message}");
				success = false;
			}

			return success;
		}
	}
}

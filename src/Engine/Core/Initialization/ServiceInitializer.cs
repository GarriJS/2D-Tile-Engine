using Engine.Core.Textures;
using Engine.Core.Textures.Contracts;
using Engine.Drawing.Services;
using Engine.Drawing.Services.Contracts;
using Engine.RunTime.Managers;
using Engine.RunTime.Services;
using Engine.RunTime.Services.Contracts;
using System;

namespace Engine.Core.Initialization
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
				(typeof(ITextureService), new TextureService(game.Services)),
				(typeof(IRandomService), new RandomService()),
				(typeof(IDrawingService), new DrawingService(game.Services)),
				(typeof(IUpdateService), new UpdateService(game.Services)),
				(typeof(ISpriteService), new SpriteService(game.Services)),
				(typeof(IAnimationService), new AnimationService(game.Services)),
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

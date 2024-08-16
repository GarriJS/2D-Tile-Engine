using Engine.Controls.Services;
using Engine.Controls.Services.Contracts;
using Engine.Core.Textures;
using Engine.Core.Textures.Contracts;
using Engine.Drawing.Services;
using Engine.Drawing.Services.Contracts;
using Engine.Physics.Services;
using Engine.Physics.Services.Contracts;
using Engine.RunTime.Managers;
using Engine.RunTime.Services;
using Engine.RunTime.Services.Contracts;
using Engine.Terminal.Services;
using Engine.Terminal.Services.Contracts;
using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;

namespace Engine.Core.Initialization
{
	/// <summary>
	/// Represents a service initializer
	/// </summary>
	public static class ServiceInitializer
	{
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
			return new (Type type, object provider)[]
			{
				(typeof(IRuntimeUpdateService), new RuntimeUpdateManager(game)),
				(typeof(IRuntimeDrawService), new RuntimeDrawManager(game)),
				(typeof(ITextureService), new TextureManager(game)),
				(typeof(IControlService), new ControlManager(game)),
				(typeof(IConsoleService), new ConsoleManager(game)),
				(typeof(IRandomService), new RandomService()),
				(typeof(IActionControlServices), new ActionControlService(game.Services)),
				(typeof(IDrawingService), new DrawingService(game.Services)),
				(typeof(IUpdateService), new UpdateService(game.Services)),
				(typeof(ISpriteService), new SpriteService(game.Services)),
				(typeof(IAnimationService), new AnimationService(game.Services)),
				(typeof(IImageService), new ImageService(game.Services)),
				(typeof(IPositionService), new PositionService(game.Services)),
			};
		}
	}
}

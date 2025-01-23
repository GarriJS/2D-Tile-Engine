using Engine.Controls.Services;
using Engine.Controls.Services.Contracts;
using Engine.Core.Contracts;
using Engine.Core.Fonts;
using Engine.Core.Fonts.Contracts;
using Engine.Core.Textures;
using Engine.Core.Textures.Contracts;
using Engine.Debugging.Services;
using Engine.Debugging.Services.Contracts;
using Engine.Drawing.Services;
using Engine.Drawing.Services.Contracts;
using Engine.Physics.Services;
using Engine.Physics.Services.Contracts;
using Engine.RunTime.Managers;
using Engine.RunTime.Services;
using Engine.RunTime.Services.Contracts;
using Engine.UI.Services;
using Engine.UI.Services.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Diagnostics;
using System.Linq;

namespace Engine.Core.Initialization
{
	/// <summary>
	/// Represents a service initializer.
	/// </summary>
	internal static class ServiceInitializer
	{
		/// <summary>
		/// Starts the engine services.
		/// </summary>
		/// <param name="game">The game.</param>
		/// <returns>A value indicating whether if all services were started.</returns>
		internal static bool StartEngineServices(Game1 game)
		{
			return StartServices(game, GetEngineServiceContractPairs);
		}

		/// <summary>
		/// Starts the game services.
		/// </summary>
		/// <param name="game">The game.</param>
		/// <param name="serviceProvider">The service provider.</param>
		/// <returns>A value indicating whether if all services were started.</returns>
		internal static bool StartServices(Game1 game, Func<Game, (Type type, object provider)[]> serviceProvider)
		{
			var success = true;
			var serviceContractPairs = serviceProvider?.Invoke(game);

			if (true != serviceContractPairs?.Any())
			{ 
				return success;
			}

			foreach (var (type, provider) in serviceContractPairs)
			{
				try
				{
					game.Services.AddService(type, provider);

					if (provider is GameComponent gameComponent)
					{
						game.Components.Add(gameComponent);
					}

					if (provider is INeedInitialization initializations)
					{
						game.Initializations.Add(initializations);
					}

					if (provider is ILoadContent loadable)
					{
						game.Loadables.Add(loadable);
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
		private static (Type type, object provider)[] GetEngineServiceContractPairs(Game game)
		{
			return
			[
				(typeof(ContentManager), game.Content),
				(typeof(IRuntimeUpdateService), new RuntimeUpdateManager(game)),
				(typeof(IRuntimeDrawService), new RuntimeDrawManager(game)),
				(typeof(IControlService), new ControlManager(game)),
				(typeof(IDebugService), new DebugService(game.Services)),
				(typeof(IUserInterfaceScreenZoneService), new UserInterfaceScreenZoneService(game.Services)),
				(typeof(IUserInterfaceElementService), new UserInterfaceElementService(game.Services)),
				(typeof(IUserInterfaceService), new UserInterfaceService(game.Services)),
				(typeof(ITextureService), new TextureService(game.Services)),
				(typeof(IActionControlServices), new ActionControlService(game.Services)),
				(typeof(IFontService), new FontService(game.Services)),
				(typeof(IDrawingService), new DrawingService(game.Services)),
				(typeof(IWritingService), new WritingService(game.Services)),
				(typeof(IUpdateService), new UpdateService(game.Services)),
				(typeof(IImageService), new ImageService(game.Services)),
				(typeof(ISpriteService), new SpriteService(game.Services)),
				(typeof(IAnimationService), new AnimationService(game.Services)),
				(typeof(IIndependentImageService), new IndependentImageService(game.Services)),
				(typeof(IPositionService), new PositionService(game.Services)),
				(typeof(IAreaService), new AreaService(game.Services)),
				(typeof(IRandomService), new RandomService()),
			];
		}
	}
}

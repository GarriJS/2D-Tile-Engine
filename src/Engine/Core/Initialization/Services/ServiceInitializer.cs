using Engine.Controls.Managers;
using Engine.Controls.Services;
using Engine.Controls.Services.Contracts;
using Engine.Core.Contracts;
using Engine.Core.Files.Services;
using Engine.Core.Files.Services.Contracts;
using Engine.Core.Fonts.Services;
using Engine.Core.Fonts.Services.Contracts;
using Engine.Core.Initialization.Services.Contracts;
using Engine.Core.Textures.Services;
using Engine.Core.Textures.Services.Contracts;
using Engine.Debugging.Services;
using Engine.Debugging.Services.Contracts;
using Engine.Graphics.Services;
using Engine.Graphics.Services.Contracts;
using Engine.Physics.Services;
using Engine.Physics.Services.Contracts;
using Engine.RunTime.Managers;
using Engine.RunTime.Services;
using Engine.RunTime.Services.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Diagnostics;
using System.Linq;

namespace Engine.Core.Initialization.Services
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
		internal static bool StartEngineServices(Engine game)
		{
			return StartServices(game, GetEngineServiceContractPairs);
		}

		/// <summary>
		/// Starts the game services.
		/// </summary>
		/// <param name="game">The game.</param>
		/// <param name="serviceProvider">The service provider.</param>
		/// <returns>A value indicating whether if all services were started.</returns>
		internal static bool StartServices(Engine game, Func<Game, (Type type, object provider)[]> serviceProvider)
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
		static private (Type type, object provider)[] GetEngineServiceContractPairs(Game game)
		{
			return
			[
				(typeof(ContentManager), game.Content),
				(typeof(IRuntimeUpdateService), new RuntimeUpdateManager(game)),
				(typeof(IRuntimeDrawService), new RuntimeDrawManager(game)),               
				(typeof(IRuntimeOverlaidDrawService), new RuntimeOverlaidDrawManager(game)),
				(typeof(IControlService), new ControlManager(game)),
				(typeof(IFunctionService), new FunctionService(game.Services)),
				(typeof(IJsonService), new JsonService(game.Services)),
				(typeof(IDebugService), new DebugService(game.Services)),
				(typeof(ITextureService), new TextureService(game.Services)),
				(typeof(IActionControlServices), new ActionControlService(game.Services)),
				(typeof(IFontService), new FontService(game.Services)),
				(typeof(IDrawingService), new DrawingService(game.Services)),
				(typeof(IWritingService), new WritingService(game.Services)),
				(typeof(IGraphicService), new GraphicService(game.Services)),
				(typeof(IImageService), new ImageService(game.Services)),
				(typeof(IIndependentGraphicService), new IndependentGraphicService(game.Services)),
				(typeof(IGraphicTextService), new GraphicTextService(game.Services)),
				(typeof(IAnimationService), new AnimationService(game.Services)),
				(typeof(IPositionService), new PositionService(game.Services)),
				(typeof(IAreaService), new AreaService(game.Services)),
				(typeof(IRandomService), new RandomService()),
			];
		}
	}
}

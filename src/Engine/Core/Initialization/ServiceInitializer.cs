using Engine.Controls.Managers;
using Engine.Controls.Services;
using Engine.Controls.Services.Contracts;
using Engine.Core.Contracts;
using Engine.Core.Files.Services;
using Engine.Core.Files.Services.Contracts;
using Engine.Core.Fonts.Services;
using Engine.Core.Fonts.Services.Contracts;
using Engine.Core.Initialization.Services;
using Engine.Core.Initialization.Services.Contracts;
using Engine.Core.State;
using Engine.Core.State.Contracts;
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

namespace Engine.Core.Initialization
{
	/// <summary>
	/// Represents a service initializer.
	/// </summary>
	static internal class ServiceInitializer
	{
		/// <summary>
		/// Starts the engine services.
		/// </summary>
		/// <param name="engine">The engine.</param>
		/// <returns>A value indicating whether if all services were started.</returns>
		static internal bool StartEngineServices(Engine engine)
		{
			var result = StartServices(engine, GetEngineServiceContractPairs);

			return result;
		}

		/// <summary>
		/// Starts the engine services.
		/// </summary>
		/// <param name="engine">The engine.</param>
		/// <param name="serviceProvider">The service provider.</param>
		/// <returns>A value indicating whether if all services were started.</returns>
		static internal bool StartServices(Engine engine, Func<Engine, (Type type, object provider)[]> serviceProvider)
		{
			var success = true;
			var serviceContractPairs = serviceProvider?.Invoke(engine);

			foreach (var (type, provider) in serviceContractPairs ?? [])
			{
				try
				{
					engine.Services.AddService(type, provider);

					if (provider is GameComponent gameComponent)
						engine.Components.Add(gameComponent);

					if (provider is IDoConfiguration initializations)
						engine.Configurables.Add(initializations);

					if (provider is ILoadContent loadable)
						engine.Loadables.Add(loadable);

					if (provider is IPostLoadInitialize postInitialize)
						engine.PostLoadInitializers.Add(postInitialize);
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
		/// <param name="engine">The engine.</param>
		/// <returns>The service contract pairs.</returns>
		static private (Type type, object provider)[] GetEngineServiceContractPairs(Engine engine)
		{
			(Type type, object provider)[] pairs =
			[
				(typeof(ContentManager), engine.Content),
				(typeof(IPreRenderService), new PreRenderManager(engine)),
				(typeof(IRuntimeUpdateService), new RuntimeUpdateManager(engine)),
				(typeof(IRuntimeDrawService), new RuntimeDrawManager(engine)),               
				(typeof(IRuntimeOverlaidDrawService), new RuntimeOverlaidDrawManager(engine)),
				(typeof(IControlService), new ControlManager(engine)),
				(typeof(IFunctionService), new FunctionService(engine.Services)),
				(typeof(IJsonService), new JsonService(engine.Services)),
				(typeof(IGameStateService), new GameStateService(engine.Services)),
				(typeof(IDebugService), new DebugService(engine.Services)),
				(typeof(ITextureService), new TextureService(engine.Services)),
				(typeof(IActionControlServices), new ActionControlService(engine.Services)),
				(typeof(IFontService), new FontService(engine.Services)),
				(typeof(IDrawingService), new DrawingService(engine.Services)),
				(typeof(IWritingService), new WritingService(engine.Services)),
				(typeof(IGraphicService), new GraphicService(engine.Services)),
				(typeof(IImageService), new ImageService(engine.Services)),
				(typeof(IIndependentGraphicService), new IndependentGraphicService(engine.Services)),
				(typeof(IGraphicTextService), new GraphicTextService(engine.Services)),
				(typeof(IAnimationService), new AnimationService(engine.Services)),
				(typeof(IPositionService), new PositionService(engine.Services)),
				(typeof(IAreaService), new AreaService(engine.Services)),
				(typeof(IRandomService), new RandomService()),
			];

			return pairs;
		}
	}
}

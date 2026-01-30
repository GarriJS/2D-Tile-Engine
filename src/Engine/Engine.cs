using Engine.Controls.Models;
using Engine.Controls.Services.Contracts;
using Engine.Controls.Typing;
using Engine.Core.Contracts;
using Engine.Core.Initialization;
using Engine.Core.Initialization.Services.Contracts;
using Engine.Core.State.Contracts;
using Engine.Debugging.Services;
using Engine.DiskModels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

// LOGGING
namespace Engine
{
	/// <summary>
	/// Represents a engine.
	/// </summary>
	sealed public partial class Engine : Game
	{
		public GraphicsDeviceManager _graphics;

		/// <summary>
		/// Gets or sets the initial control context type.
		/// </summary>
		private Type InitialControlContextType { get; set; }

		/// <summary>
		/// Gets the external services.
		/// </summary>
		private List<Func<Game, (Type type, object provider)[]>> ExternalServiceProviders { get; } = [];

		/// <summary>
		/// Gets the external model processor map providers.
		/// </summary>
		private List<Func<GameServiceContainer, (Type typeIn, Delegate)[]>> ExternalModelProcessorMapProviders { get; } = [];

		/// <summary>
		/// Gets the initial models.
		/// </summary>
		private List<Func<GameServiceContainer, IList<object>>> InitialModelsProviders { get; } = [];

		/// <summary>
		/// Get the function providers
		/// </summary>
		private List<Func<GameServiceContainer, Dictionary<string, Delegate>>> FunctionProviders { get; } = [];

		/// <summary>
		/// Gets the initializations. 
		/// </summary>
		internal List<IDoConfiguration> Configurables { get; } = [];

		/// <summary>
		/// Gets the loadables.
		/// </summary>
		internal List<ILoadContent> Loadables { get; } = [];

		/// <summary>
		/// Gets the post load initializers.
		/// </summary>
		internal List<IPostLoadInitialize> PostLoadInitializers { get; } = [];

		protected override void Initialize()
		{
			// Start services
			_ = ServiceInitializer.StartEngineServices(this);

			foreach (var externalServiceProvider in this.ExternalServiceProviders ?? [])
				_ = ServiceInitializer.StartServices(this, externalServiceProvider);

			this.ExternalServiceProviders.Clear();

			// Do initializations
			this._graphics.PreferredBackBufferWidth = 1080;
			this._graphics.PreferredBackBufferHeight = 720;
			this._graphics.ApplyChanges();

			foreach (var configurable in this.Configurables ?? [])
				configurable.ConfigureService();

			// Load model processors
			ModelMapper.LoadEngineModelProcessingMappings(this.Services);

			foreach (var externalModelProcessorMapProvider in this.ExternalModelProcessorMapProviders ?? [])
				ModelMapper.LoadModelProcessingMappings(this.Services, externalModelProcessorMapProvider);

			this.ExternalModelProcessorMapProviders.Clear();

			// Loads the game functions
			var functionService = this.Services.GetService<IFunctionService>();

			foreach (var functionProvider in this.FunctionProviders ?? [])
			{
				var functionKpvs = functionProvider.Invoke(this.Services);

				foreach (var functionKpv in functionKpvs ?? [])
					functionService.TryAddFunction(functionKpv.Key, functionKpv.Value);
			}

			this.FunctionProviders.Clear();

			// ConfigureService game managers
			base.Initialize();

			// Debug to be moved
			var gameStateService = this.Services.GetService<IGameStateService>();
			gameStateService.CreateGameStateFlag(DebugService.DebugFlagName, this.InDebugMode);

			// Do post initializer

			foreach (var initializer in this.PostLoadInitializers ?? [])
				initializer.PostLoadInitialize();
		}

		// Is called by base.ConfigureService in ConfigureService()
		protected override void LoadContent()
		{
			// Do any content loading
			foreach (var loadable in this.Loadables)
				loadable.LoadContent();

			this.Loadables.Clear();

			// Load the initial models
			foreach (var initialModelsProvider in this.InitialModelsProviders)
				ModelProcessor.ProcessInitialModels(initialModelsProvider, this.Services);

			this.InitialModelsProviders.Clear();
			var controlService = this.Services.GetService<IControlService>();
			var controlContext = (ControlContext)Activator.CreateInstance(this.InitialControlContextType, this.Services);
			controlService.ControlContext = controlContext;
		}

		protected override void Update(GameTime gameTime)
		{
			var controlService = this.Services.GetService<IControlService>();
			var controlState = controlService.ControlState;

			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			if (Keyboard.GetState().IsKeyDown(Keys.G))
			{

			}

			base.Update(gameTime);
			KeyboardTyping.OldPressedKeys = Keyboard.GetState().GetPressedKeys();
		}

		protected override void Draw(GameTime gameTime)
		{
			base.Draw(gameTime);
		}
	}
}

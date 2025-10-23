using Engine.Controls.Models;
using Engine.Controls.Services.Contracts;
using Engine.Controls.Typing;
using Engine.Core.Contracts;
using Engine.Core.Fonts.Services.Contracts;
using Engine.Core.Initialization.Services;
using Engine.Core.Initialization.Services.Contracts;
using Engine.Debugging.Services.Contracts;
using Engine.DiskModels;
using Engine.Graphics.Services.Contracts;
using Engine.RunTime.Services.Contracts;
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
	public partial class Engine : Game
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
		/// Gets the loadables.
		/// </summary>
		internal List<ILoadContent> Loadables { get; } = [];

		/// <summary>
		/// Gets the initializations. 
		/// </summary>
		internal List<INeedInitialization> Initializations { get; } = [];

		protected override void Initialize()
		{
			// Start services
			_ = ServiceInitializer.StartEngineServices(this);

			foreach (var externalServiceProvider in this.ExternalServiceProviders)
			{
				_ = ServiceInitializer.StartServices(this, externalServiceProvider);
			}

			this.ExternalServiceProviders.Clear();

			// Do initializations
			this._graphics.PreferredBackBufferWidth = 1920;
			this._graphics.PreferredBackBufferHeight = 1080;
			this._graphics.ApplyChanges();

			foreach (var initialization in this.Initializations)
			{
				initialization.Initialize();
			}

			// Load model processors
			ModelMapper.LoadEngineModelProcessingMappings(this.Services);

			foreach (var externalModelProcessorMapProvider in this.ExternalModelProcessorMapProviders)
			{
				ModelMapper.LoadModelProcessingMappings(this.Services, externalModelProcessorMapProvider);
			}

			this.ExternalModelProcessorMapProviders.Clear();

			// Loads the game functions
			var functionService = this.Services.GetService<IFunctionService>();

			foreach (var functionProvider in this.FunctionProviders)
			{
				var functionKpvs = functionProvider.Invoke(this.Services);

				foreach (var functionKpv in functionKpvs)
				{
					functionService.TryAddFunction(functionKpv.Key, functionKpv.Value);
				}
			}

			// Other

			base.Initialize();
		}

		protected override void LoadContent()
		{
			// Do any content loading
			foreach (var loadable in this.Loadables)
			{
				loadable.LoadContent();
			}

			this.Loadables.Clear();

			// Debug
			if (true == this.LaunchInDebugMode)
			{ 
				var debugService = this.Services.GetService<IDebugService>();
				var fontService = this.Services.GetService<IFontService>();

				fontService.SetDebugSpriteFont(this.DebugSpriteFontName);
				debugService.ToggleScreenAreaIndicators();
				debugService.TogglePerformanceRateCounter();
			}

			this.FunctionProviders.Clear();

			// Load the initial models
			foreach (var initialModelsProvider in this.InitialModelsProviders)
			{
				ModelProcessor.ProcessInitialModels(initialModelsProvider, this.Services);
			}

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
			{
				Exit();
			}

			if (Keyboard.GetState().IsKeyDown(Keys.G))
			{

			}

			base.Update(gameTime);

			KeyboardTyping.OldPressedKeys = Keyboard.GetState().GetPressedKeys();
		}

		protected override void Draw(GameTime gameTime)
		{
			this.GraphicsDevice.Clear(Color.CornflowerBlue);
			var drawService = this.Services.GetService<IDrawingService>();
			var imageService = this.Services.GetService<IImageService>();
			var controlService = this.Services.GetService<IControlService>();
			var image = imageService.GetImage("tile_grid_dark", 160, 160);
			var mouse = controlService.ControlState.MousePosition;

			base.Draw(gameTime);

			//drawService.BeginDraw();

			//drawService.Draw(image.Texture, this.GetLocalTileCoordinates(mouse, -2), new Rectangle(0, 0, 160, 160), TextColor.White);

			//drawService.EndDraw();

		}
	}
}

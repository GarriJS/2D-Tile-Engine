using Engine.Controls.Services.Contracts;
using Engine.Controls.Typing;
using Engine.Core.Contracts;
using Engine.Core.Fonts.Contracts;
using Engine.Core.Initialization;
using Engine.Debugging.Services.Contracts;
using Engine.DiskModels;
using Engine.DiskModels.UI;
using Engine.Drawables.Services.Contracts;
using Engine.RunTime.Services.Contracts;
using Engine.UI.Models.Contracts;
using Engine.UI.Services.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Engine
{
	/// <summary>
	/// Represents a engine.
	/// </summary>
	public partial class Engine : Game
	{
		public GraphicsDeviceManager _graphics;

		/// <summary>
		/// Gets and sets the external services.
		/// </summary>
		private List<Func<Game, (Type type, object provider)[]>> ExternalServiceProviders { get; } = [];

		/// <summary>
		/// Gets and sets the external model processor map providers.
		/// </summary>
		private List<Func<GameServiceContainer, (Type typeIn, Delegate)[]>> ExternalModelProcessorMapProviders { get; } = [];

		/// <summary>
		/// Gets and sets the initial models.
		/// </summary>
		private List<Func<GameServiceContainer, IList<object>>> InitialModelsProviders { get; } = [];

		/// <summary>
		/// Gets and sets the button hover event processors.
		/// </summary>
		private List<Func<GameServiceContainer, Dictionary<string, Action<IAmAUiElement, Vector2>>>> UiElementHoverEventProcessorsProviders { get; } = [];

		/// <summary>
		/// Gets and sets the button press event processors.
		/// </summary>
		private List<Func<GameServiceContainer, Dictionary<string, Action<IAmAUiElement, Vector2>>>> UiElementPressEventProcessorsProviders { get; } = [];

		/// <summary>
		/// Gets and sets the button click event processors.
		/// </summary>
		private List<Func<GameServiceContainer, Dictionary<string, Action<IAmAUiElement, Vector2>>>> UiElementClickEventProcessorsProviders { get; } = [];

		/// <summary>
		/// Gets and sets the initial models.
		/// </summary>
		private List<Func<GameServiceContainer, IList<UiGroupModel>>> InitialUiModelsProviders { get; } = [];

		/// <summary>
		/// Gets the loadables.
		/// </summary>
		internal List<ILoadContent> Loadables { get; } = [];

		/// <summary>
		/// Gets the initializations. 
		/// </summary>
		internal List<INeedInitialization> Initializations { get; } = [];

		/// <summary>
		/// Initializes a new instance of the engine.
		/// </summary>
		public Engine()
		{
			this._graphics = new GraphicsDeviceManager(this);
			this.Content.RootDirectory = "Content";
			this.Window.AllowUserResizing = true;
			this.IsMouseVisible = false;
		}

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

			// Debug
			if (true == this.LaunchDebugMode)
			{ 
				var debugService = this.Services.GetService<IDebugService>();
				var fontService = this.Services.GetService<IFontService>();

				fontService.SetDebugSpriteFont(this.DebugSpriteFontName);
				debugService.ToggleScreenAreaIndicators();
				debugService.TogglePerformanceRateCounter();
			}

			// Load the initial models
			foreach (var initialModelsProvider in this.InitialModelsProviders)
			{
				ModelProcessor.ProcessInitialModels(initialModelsProvider, this.Services);
			}

			this.InitialModelsProviders.Clear();

			// Load the initial user interface click events
			var uiElementService = this.Services.GetService<IUserInterfaceElementService>();

			foreach (var uiElementHoverEventProcessorsProviders in this.UiElementHoverEventProcessorsProviders)
			{
				var uiElementHoverEventProcessors = uiElementHoverEventProcessorsProviders.Invoke(this.Services);

				foreach (var uiElementHoverEventProcessor in uiElementHoverEventProcessors)
				{
					uiElementService.ElementHoverEventProcessors.Add(uiElementHoverEventProcessor.Key, uiElementHoverEventProcessor.Value);
				}
			}

			foreach (var uiElementPressEventProcessorsProviders in this.UiElementPressEventProcessorsProviders)
			{
				var uiElementPressEventProcessors = uiElementPressEventProcessorsProviders.Invoke(this.Services);

				foreach (var uiElementPressEventProcessor in uiElementPressEventProcessors)
				{
					uiElementService.ElementHoverEventProcessors.Add(uiElementPressEventProcessor.Key, uiElementPressEventProcessor.Value);
				}
			}

			foreach (var uiElementClickEventProcessorProvider in this.UiElementClickEventProcessorsProviders)
			{
				var uiElementClickEventProcessors = uiElementClickEventProcessorProvider.Invoke(this.Services);

				foreach (var uiElementClickEventProcessor in uiElementClickEventProcessors)
				{
					uiElementService.ElementClickEventProcessors.Add(uiElementClickEventProcessor.Key, uiElementClickEventProcessor.Value);
				}
			}

			this.UiElementHoverEventProcessorsProviders.Clear();
			this.UiElementPressEventProcessorsProviders.Clear();
			this.UiElementClickEventProcessorsProviders.Clear();

			// Load the initial user interface models
			foreach (var initialUiModelsProvider in this.InitialUiModelsProviders)
			{
				ModelProcessor.ProcessInitialUiModels(initialUiModelsProvider, this.Services);
			}

			this.InitialUiModelsProviders.Clear();

			// Other
			var uiService = this.Services.GetService<IUserInterfaceService>();
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
			var image = imageService.GetImage("tile_grid", 160, 160);
			var mouse = controlService.ControlState.MouseState.Position.ToVector2();

			base.Draw(gameTime);

			//drawService.BeginDraw();

			//drawService.Draw(image.Texture, this.GetLocalTileCoordinates(mouse, -2), new Rectangle(0, 0, 160, 160), Color.White);

			//drawService.EndDraw();

		}
	}
}

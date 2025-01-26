using Engine.Controls.Services.Contracts;
using Engine.Controls.Typing;
using Engine.Core.Contracts;
using Engine.Core.Initialization;
using Engine.Debugging.Services.Contracts;
using Engine.DiskModels;
using Engine.DiskModels.Engine.UI;
using Engine.UI.Models;
using Engine.UI.Models.Elements;
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
		private List<object> InitialModels { get; } = [];

		/// <summary>
		/// Gets the initial models.
		/// </summary>
		private List<UiGroupModel> InitialUiModels { get; } = [];

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
			this.IsMouseVisible = true;
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

			// Load the initial models
			ModelProcessor.ProcessInitialModels(this.InitialModels);
			this.InitialModels.Clear();

			// Load the initial user interface models.
			ModelProcessor.ProcessInitialUiModels(this.InitialUiModels, this.Services);
			this.InitialUiModels.Clear();

			// Other

			var debugService = this.Services.GetService<IDebugService>();
			debugService.ToggleScreenAreaIndicators();
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

			var mouse = Mouse.GetState().Position.ToVector2();
			var uiService = this.Services.GetService<IUserInterfaceService>();
			var foo = uiService.GetUiElementAtScreenLocation(mouse);

			if (foo?.Element is UiButton button &&
				Mouse.GetState().LeftButton == ButtonState.Pressed)
			{
				button.RaisePressEvent(foo.Location);
			}

			base.Update(gameTime);

			KeyboardTyping.OldPressedKeys = Keyboard.GetState().GetPressedKeys();
		}

		protected override void Draw(GameTime gameTime)
		{
			this.GraphicsDevice.Clear(Color.CornflowerBlue);
			//var drawService = this.Services.GetService<IDrawingService>();

			//drawService.BeginDraw();

			//drawService.EndDraw();

			base.Draw(gameTime);
		}
	}
}

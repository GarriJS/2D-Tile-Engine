using Engine.Controls.Services.Contracts;
using Engine.Controls.Typing;
using Engine.Core.Initialization;
using Engine.Core.Initialization.Models;
using Engine.Debugging.Services.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Engine
{
	public class Game1: Game
	{
		public GraphicsDeviceManager _graphics;

		public Game1()
		{
			this._graphics = new GraphicsDeviceManager(this);
			this.Content.RootDirectory = "Content";
			this.Window.AllowUserResizing = true;
			this.IsMouseVisible = true;
		}

		public void SetLoadingInstructions(LoadingInstructions loadingInstructions)
		{
			LoadingInstructionsContainer.LoadingInstructions = loadingInstructions;
		}

		protected override void Initialize()
		{
			_ = ServiceInitializer.StartServices(this);

			this._graphics.PreferredBackBufferWidth = 1920;
			this._graphics.PreferredBackBufferHeight = 1080;
			this._graphics.ApplyChanges();

			base.Initialize();
		}

		protected override void LoadContent()
		{
			foreach (var initialization in ServiceInitializer.Initializations)
			{
				initialization.Initialize();
			}

			foreach (var loadable in ServiceInitializer.Loadables)
			{ 
				loadable.LoadContent();
			}

			var debugService = this.Services.GetService<IDebugService>();
			debugService.ToggleScreenAreaIndicators();
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
			//var drawService = this.Services.GetService<IDrawingService>();

			//drawService.BeginDraw();

			//drawService.EndDraw();

			base.Draw(gameTime);
		}
	}
}

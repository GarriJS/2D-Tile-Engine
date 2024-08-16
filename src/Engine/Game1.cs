using DiscModels.Engine.Drawing;
using Engine.Core.Initialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using DiscModels.Engine.Physics;
using Engine.Drawing.Services.Contracts;
using Engine.RunTime.Services.Contracts;
using Engine.Terminal.Services.Contracts;
using Engine.Controls.Services;
using Engine.Controls.Services.Contracts;
using Engine.Controls.Models.Enums;
using Engine.Controls.Typing;

namespace Engine
{
	public class Game1 : Game
	{
		private GraphicsDeviceManager _graphics;

		public Game1()
		{
			this._graphics = new GraphicsDeviceManager(this);
			this.Content.RootDirectory = "Content";

			Window.AllowUserResizing = true;

			this.IsMouseVisible = true;
		}

		protected override void Initialize()
		{
			_ = ServiceInitializer.InitializeServices(this);

			this._graphics.PreferredBackBufferWidth = 1920;
			this._graphics.PreferredBackBufferHeight = 1200;
			this._graphics.ApplyChanges();

			base.Initialize();
		}

		protected override void LoadContent()
		{
			// TODO: use this.Content to load your game content here
		}

		protected override void Update(GameTime gameTime)
		{
			var controlService = this.Services.GetService<IControlService>();
			var controlState = controlService.ControlState;

			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
			{
				Exit();
			}

			if (true == controlService.ControlState.FreshActionTypes.Contains(ActionTypes.ToggleConsole))
			{
				var consoleService = this.Services.GetService<IConsoleService>();
				consoleService.ToggleConsole();
			}

			base.Update(gameTime);

			KeyboardTyping.OldPressedKeys = Keyboard.GetState().GetPressedKeys();
		}

		protected override void Draw(GameTime gameTime)
		{
			this.GraphicsDevice.Clear(Color.CornflowerBlue);
			var consoleService = this.Services.GetService<IConsoleService>();
			var drawService = this.Services.GetService<IDrawingService>();

			base.Draw(gameTime);

			drawService.BeginDraw();
			if (null != consoleService.Console.ActiveConsoleLine)
			{
				drawService.Draw(gameTime, consoleService.Console.ActiveConsoleLine.Command);
				
				foreach (var recommendedArg in consoleService.Console.RecommendedArguments)
				{
					drawService.Draw(gameTime, recommendedArg);
				}

				foreach (var commandLine in consoleService.Console.ConsoleLines)
				{
					drawService.Draw(gameTime, commandLine.Command, Color.Salmon);
					drawService.Draw(gameTime, commandLine.Response, Color.Yellow);
				}
			}
			drawService.EndDraw();
		}
	}
}

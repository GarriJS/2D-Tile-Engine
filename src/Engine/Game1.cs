using DiscModels.Engine.Drawing;
using Engine.Controls.Models.Enums;
using Engine.Controls.Services.Contracts;
using Engine.Controls.Typing;
using Engine.Core.Constants;
using Engine.Core.Fonts.Contracts;
using Engine.Core.Initialization;
using Engine.Drawing.Services.Contracts;
using Engine.Terminal.Services.Contracts;
using Engine.UserInterface.Models;
using Engine.UserInterface.Services;
using Engine.UserInterface.Services.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Engine
{
	public class Game1 : Game
	{
		private GraphicsDeviceManager _graphics;

		private TextLine foo;

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
			foreach (var loadable in ServiceInitializer.Loadables)
			{ 
				loadable.LoadContent();
			}

			var backgroundModel = new SpriteModel
			{
				SpritesheetBox = new Rectangle
				{
					X = 0,
					Y = 0,
					Width = 1080,
					Height = 1080
				},
				SpritesheetName = "gray"
			};

			var spriteService = this.Services.GetService<ISpriteService>();
			var background = spriteService.GetSprite(backgroundModel);

			var fontService = this.Services.GetService<IFontService>();
			var font = fontService.GetSpriteFont(FontNames.MonoRegular);

			this.foo = new TextLine
			{
				MaxVisibleTextWidth = 200,
				Text = "XThis is a test text I want to see how it looks and I need it to be kinda long",
				TextOffset = new Vector2(2, 2),
				Background = background,
				Position = new Physics.Models.Position
				{
					Coordinates = new Vector2(0, 0)
				},
				Font = font
			};

			var textInputLineService = this.Services.GetService<ITextInputLineService>();
			textInputLineService.UpdateTextLineSprite(this.foo);
		}

		protected override void Update(GameTime gameTime)
		{
			var textInputLineService = this.Services.GetService<ITextInputLineService>();
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

			if (Keyboard.GetState().IsKeyDown(Keys.Y))
			{
				textInputLineService.UpdateTextLineSprite(this.foo, this.foo.Text + "X");
			}

			if (Keyboard.GetState().IsKeyDown(Keys.U))
			{
				textInputLineService.UpdateTextLineSprite(this.foo, this.foo.Text[..^1]);
			}

			if (Keyboard.GetState().IsKeyDown(Keys.H))
			{
				textInputLineService.MoveTextLineViewArea(this.foo, 2);
			}

			if (Keyboard.GetState().IsKeyDown(Keys.G))
			{
				textInputLineService.MoveTextLineViewArea(this.foo, -2);
			}

			base.Update(gameTime);

			KeyboardTyping.OldPressedKeys = Keyboard.GetState().GetPressedKeys();
		}

		protected override void Draw(GameTime gameTime)
		{
			this.GraphicsDevice.Clear(Color.CornflowerBlue);
			var drawService = this.Services.GetService<IDrawingService>();

			drawService.BeginDraw();

			drawService.Draw(gameTime, this.foo);

			drawService.EndDraw();

			base.Draw(gameTime);
		}
	}
}

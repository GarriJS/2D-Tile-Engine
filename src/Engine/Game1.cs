using DiscModels.Engine.Drawing;
using Engine.Controls.Services.Contracts;
using Engine.Controls.Typing;
using Engine.Core.Constants;
using Engine.Core.Fonts.Contracts;
using Engine.Core.Initialization;
using Engine.Drawing.Services.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Engine
{
	public class Game1: Game
	{
		private GraphicsDeviceManager _graphics;

		public Game1(LaunchSettings launchSettings = null)
		{
			if (null != launchSettings)
			{
				ServiceInitializer.ContentManagers = launchSettings.ContentManagers;
			}

			this._graphics = new GraphicsDeviceManager(this);
			this.Content.RootDirectory = "Content";
			this.Window.AllowUserResizing = true;
			this.IsMouseVisible = true;
		}

		protected override void Initialize()
		{
			_ = ServiceInitializer.InitializeServices(this);

			this._graphics.PreferredBackBufferWidth = 1920;
			this._graphics.PreferredBackBufferHeight = 1080;
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
					Width = 150,
					Height = 600
				},
				SpritesheetName = "gray"
			};

			var spriteService = this.Services.GetService<ISpriteService>();
			var background = spriteService.GetSprite(backgroundModel);

			var fontService = this.Services.GetService<IFontService>();
			var font = fontService.GetSpriteFont(FontNames.MonoRegular);
		}

		protected override void Update(GameTime gameTime)
		{
			var controlService = this.Services.GetService<IControlService>();
			var controlState = controlService.ControlState;

			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
			{
				Exit();
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

using DiscModels.Engine.Drawing;
using DiscModels.Engine.Physics;
using Engine.Drawing.Services.Contracts;
using Engine.RunTime.Services.Contracts;
using Engine.Terminal.Model;
using Engine.Terminal.Services.Contracts;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Engine.Terminal.Services
{
	/// <summary>
	/// Represents a console service.
	/// </summary>
	public class ConsoleService : IConsoleService
	{
		private readonly GameServiceContainer _gameServiceContainer;

		/// <summary>
		/// Gets or sets a value indicating whether the console is started.
		/// </summary>
		private bool ConsoleStarted { get; set; }

		/// <summary>
		/// Gets or sets the console.
		/// </summary>
		private Console Console { get; set; }

		/// <summary>
		/// Initializes the console service.
		/// </summary>
		/// <param name="gameService">The game services.</param>
		public ConsoleService(GameServiceContainer gameService)
		{
			this._gameServiceContainer = gameService;
			this.ConsoleStarted = false;
		}

		/// <summary>
		/// Toggles the console.
		/// </summary>
		public void ToggleConsole()
		{
			if (true == this.ConsoleStarted)
			{
				this.StopConsole();
			}
			else
			{ 
				this.StartConsole();
			}
		}

		/// <summary>
		/// Starts the console.
		/// </summary>
		public void StartConsole()
		{
			if (true == this.ConsoleStarted)
			{
				return;			
			}

			this.ConsoleStarted = true;
			var runtimeDrawService = this._gameServiceContainer.GetService<IRuntimeDrawService>();

			if (null != this.Console)
			{
				runtimeDrawService.AddOverlaidDrawable(1, Console.ConsoleBackGround); 
				runtimeDrawService.AddOverlaidDrawable(1, Console.ConsoleTextArea);

				return;
			}

			var imageService = this._gameServiceContainer.GetService<IImageService>();
			ImageModel imageModel;

			imageModel = new ImageModel
			{
				Position = new PositionModel
				{
					X = 0,
					Y = 0
				},
				Sprite = new SpriteModel
				{
					SpritesheetBox = new Rectangle
					{
						X = 0,
						Y = 0,
						Width = 1080,
						Height = 1080
					},
					SpritesheetName = "console_back_ground"
				}
			};

			var background = imageService.GetImage(imageModel);
			runtimeDrawService.AddOverlaidDrawable(1, background);

			imageModel = new ImageModel
			{
				Position = new PositionModel
				{
					X = 0,
					Y = 1080
				},
				Sprite = new SpriteModel
				{
					SpritesheetBox = new Rectangle
					{
						X = 0,
						Y = 0,
						Width = 1080,
						Height = 20
					},
					SpritesheetName = "console_text_area"
				}
			};

			var textArea = imageService.GetImage(imageModel);
			runtimeDrawService.AddOverlaidDrawable(1, textArea);
			this.Console = new Console
			{
				ConsoleBackGround = background,
				ConsoleTextArea = textArea,
				ConsoleLines = new List<ConsoleLine>()
			};
		}

		/// <summary>
		/// Stops the console.
		/// </summary>
		public void StopConsole()
		{
			if ((false == this.ConsoleStarted) ||
				(null == this.Console))
			{
				return;
			}

			this.ConsoleStarted = false;
			var runtimeDrawService = this._gameServiceContainer.GetService<IRuntimeDrawService>();
			runtimeDrawService.RemoveOverlaidDrawable(1, this.Console.ConsoleBackGround);
			runtimeDrawService.RemoveOverlaidDrawable(1, this.Console.ConsoleTextArea);
		}
	}
}

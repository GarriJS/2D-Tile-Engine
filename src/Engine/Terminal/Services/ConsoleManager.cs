using DiscModels.Engine.Drawing;
using DiscModels.Engine.Physics;
using Engine.Controls.Typing;
using Engine.Drawing.Models;
using Engine.Drawing.Services.Contracts;
using Engine.Physics.Models;
using Engine.RunTime.Services.Contracts;
using Engine.Terminal.Commands;
using Engine.Terminal.Commands.Models;
using Engine.Terminal.Models;
using Engine.Terminal.Services.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Engine.Terminal.Services
{
	/// <summary>
	/// Represents a console manager.
	/// </summary>
	public class ConsoleManager : GameComponent, IConsoleService
	{
		/// <summary>
		/// Gets or sets a value indicating whether the console is started.
		/// </summary>
		private bool ConsoleStarted { get; set; }

		/// <summary>
		/// Gets or sets the console.
		/// </summary>
		public Console Console { get; private set; }

		/// <summary>
		/// Gets or sets the command profiles.
		/// </summary>
		private List<CommandProfile> CommandProfiles { get; set; }

		/// <summary>
		/// Initializes the console manager.
		/// </summary>
		/// <param name="game">The game.</param>
		public ConsoleManager(Game game) : base(game)
		{
			this.ConsoleStarted = false;
		}

		/// <summary>
		/// Initializes the console manager.
		/// </summary>
		public override void Initialize()
		{
			var imageService = this.Game.Services.GetService<IImageService>();
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
			this.Console = new Console
			{
				ConsoleBackGround = background,
				ConsoleTextArea = textArea,
				ConsoleLines = new List<ConsoleLine>(),
				RecommendedArguments = new List<DrawableText>()
			};

			this.CommandProfiles = CommandInitializer.GetCommandProfiles();

			base.Initialize();
		}

		/// <summary>
		/// Updates the console manager.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		public override void Update(GameTime gameTime)
		{
			if (false == this.ConsoleStarted)
			{
				return;
			}

			var pressedKeys = Keyboard.GetState().GetPressedKeys();
			var oldPressedKeys = KeyboardTyping.OldPressedKeys;
			var newLineContent = string.Empty;

			if (true == pressedKeys?.Any())
			{
				var isShiftPressed = pressedKeys.Any(e => (e == Keys.LeftShift) ||
														  (e == Keys.RightShift));

				foreach (var pressedKey in pressedKeys)
				{
					if ((true == oldPressedKeys.Contains(pressedKey)) ||
						(Keys.OemTilde == pressedKey) ||
						((0 == this.Console.ActiveConsoleLine.Command.Text.Length) &&
						 (Keys.Space == pressedKey)))
					{
						continue;
					}

					if ((Keys.Tab == pressedKey) &&
						(true == this.Console.RecommendedArguments.Any()))
					{
						var args = this.Console.ActiveConsoleLine.Command.Text.Substring(4)
																			  .Split(' ')
																			  .ToArray();

						if (1 == args.Length)
						{
							this.Console.ActiveConsoleLine.Command.Text = $" -> {this.Console.RecommendedArguments.FirstOrDefault().Text}";
						}
						else if (2 == args.Length)
						{
							this.Console.ActiveConsoleLine.Command.Text = $" -> {args[0]} {this.Console.RecommendedArguments.FirstOrDefault().Text}";
						}

						return;
					}

					if (Keys.Back == pressedKey)
					{
						if (4 < this.Console.ActiveConsoleLine.Command.Text.Length)
						{
							this.Console.ActiveConsoleLine.Command.Text = this.Console.ActiveConsoleLine.Command.Text[..^1];
						}

						continue;
					}

					if (Keys.Enter == pressedKey)
					{
						if (false == string.IsNullOrEmpty(this.Console.ActiveConsoleLine.Command.Text))
						{
							this.ProcessActiveConsoleLine();
						}

						continue;
					}

					var mappedChar = KeyboardTyping.ToChar(pressedKey, isShiftPressed);

					if (true == mappedChar.HasValue)
					{
						newLineContent = $"{newLineContent}{mappedChar}";
					}
				}
			}

			if ((false == string.IsNullOrEmpty(newLineContent)) ||
				(true == pressedKeys.Any(e => Keys.Back == e)))
			{
				var updateLineContent = $"{this.Console.ActiveConsoleLine.Command.Text}{newLineContent}";
				var formattedText = KeyboardTyping.FormatForDrawString(updateLineContent);
				formattedText = Regex.Replace(formattedText, @"\s{2,}", " ");
				var stringWidth = this.Console.ActiveConsoleLine.Command.SpriteFont.MeasureString(formattedText).X;

				if (this.Console.ConsoleTextArea.Sprite.TextureBox.Width >= stringWidth)
				{
					this.Console.ActiveConsoleLine.Command.Text = formattedText;
				}

				this.UpdateRecommendedArguments(formattedText);
			}

			base.Update(gameTime);
		}

		/// <summary>
		/// Updates the recommended arguments.
		/// </summary>
		/// <param name="formattedText"></param>
		private void UpdateRecommendedArguments(string formattedText)
		{
			this.Console.RecommendedArguments = new List<DrawableText>();

			if (0 == formattedText.Length)
			{
				return;
			}

			var args = formattedText.Substring(4)
									.Split(' ')
								    .ToArray();
			var runtimeDrawService = this.Game.Services.GetService<IRuntimeDrawService>();

			if (1 == args.Length)
			{
				var domain = args[0];
				var matchingDomains = this.CommandProfiles.Select(e => e.Domain)
														  .Where(e => (true == e.StartsWith(domain)) &&
																	  (e != domain));

				if (false == matchingDomains.Any())
				{
					return;
				}

				foreach (var matchingDomain in matchingDomains)
				{
					var y = this.Console.ConsoleBackGround.Sprite.TextureBox.Height + this.Console.ConsoleTextArea.Sprite.TextureBox.Height;

					foreach (var existingDomain in this.Console.RecommendedArguments)
					{
						y += 20;
					}

					var drawableText = new DrawableText
					{
						SpriteFont = this.Console.ActiveConsoleLine.Command.SpriteFont,
						Text = matchingDomain,
						Position = new Position
						{
							Coordinates = new Vector2(0, y)
						}
					};

					this.Console.RecommendedArguments.Add(drawableText);
				}
			}
			else if (2 == args.Length)
			{
				var domain = args[0];
				var command = args[^1];
				var commandProfile = this.CommandProfiles.FirstOrDefault(e => e.Domain == domain);

				if (null != commandProfile)
				{
					var matchingCommands = commandProfile.Parameters.Keys.Where(e => (true == e.StartsWith(command)) &&
																					 (e != domain));

					if (false == matchingCommands.Any())
					{
						return;
					}

					foreach (var matchingDomain in matchingCommands)
					{
						var y = this.Console.ConsoleBackGround.Sprite.TextureBox.Height + this.Console.ConsoleTextArea.Sprite.TextureBox.Height;

						foreach (var existingDomain in this.Console.RecommendedArguments)
						{
							y += 20;
						}

						var drawableText = new DrawableText
						{
							SpriteFont = this.Console.ActiveConsoleLine.Command.SpriteFont,
							Text = matchingDomain,
							Position = new Position
							{
								Coordinates = new Vector2(0, y)
							}
						};

						this.Console.RecommendedArguments.Add(drawableText);
					}
				}
			}
			else if (2 < args.Length)
			{
				var domain = args[0];
				var command = args[1];
				var argument = args[^1];
				var commandProfile = this.CommandProfiles.FirstOrDefault(e => e.Domain == domain);

				if (true == commandProfile?.Parameters.TryGetValue(command, out var commandArgument))
				{
					var relevantArgument = commandArgument.FirstOrDefault(e => e.ArgumentOrder == args.Length - 3);

					if (relevantArgument != null)
					{
						var y = this.Console.ConsoleBackGround.Sprite.TextureBox.Height + this.Console.ConsoleTextArea.Sprite.TextureBox.Height;

						foreach (var existingDomain in this.Console.RecommendedArguments)
						{
							y += 20;
						}

						var drawableText = new DrawableText
						{
							SpriteFont = this.Console.ActiveConsoleLine.Command.SpriteFont,
							Text = $"{relevantArgument.ArgumentName}, {relevantArgument.ArgumentType}",
							Position = new Position
							{
								Coordinates = new Vector2(0, y)
							}
						};

						this.Console.RecommendedArguments.Add(drawableText);
					}
				}
			}
		}

		/// <summary>
		/// Process the active console line.
		/// </summary>
		private void ProcessActiveConsoleLine()
		{
			this.Console.RecommendedArguments = new List<DrawableText>();
			var commandLineArguments = this.Console.ActiveConsoleLine.Command.Text.Substring(4)
																				  .Split(' ')
																				  .Where(e => false == string.IsNullOrEmpty(e))
																				  .ToArray();

			if (false == commandLineArguments.Any())
			{
				return;
			}

			this.Console.ActiveConsoleLine.Response.Text = $" <- {CommandExecuter.ExecuteArguments(this.CommandProfiles, commandLineArguments)}";

			foreach (var consoleLines in this.Console.ConsoleLines)
			{
				consoleLines.Command.Position.Y -= 40;
				consoleLines.Response.Position.Y -= 40;
			}

			this.Console.ActiveConsoleLine.Command.Position = new Position
			{
				Coordinates = new Vector2(0, 1080 - 40)
			};
			this.Console.ActiveConsoleLine.Response.Position = new Position
			{
				Coordinates = new Vector2(0, 1080 - 20)
			};

			this.Console.ConsoleLines.Add(this.Console.ActiveConsoleLine);
			var spriteFont = this.Game.Content.Load<SpriteFont>($@"..\..\..\Content\Fonts\MonoRegular");
			this.Console.ActiveConsoleLine = new ConsoleLine
			{
				Command = new DrawableText
				{
					Text = " -> ",
					SpriteFont = spriteFont,
					Position = new Position
					{
						Coordinates = new Vector2(0, this.Console.ConsoleBackGround.Sprite.TextureBox.Height)
					}
				},
				Response = new DrawableText
				{
					Text = " <- ",
					SpriteFont = spriteFont,
					Position = null
				},
			};
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
			if ((true == this.ConsoleStarted) ||
				(null == this.Console))
			{
				return;
			}

			this.ConsoleStarted = true;
			var spriteFont = this.Game.Content.Load<SpriteFont>($@"..\..\..\Content\Fonts\MonoRegular");
			this.Console.ActiveConsoleLine = new ConsoleLine
			{
				Command = new DrawableText
				{
					Text = " -> ",
					SpriteFont = spriteFont,
					Position = new Position
					{
						Coordinates = new Vector2(0, this.Console.ConsoleBackGround.Sprite.TextureBox.Height)
					}
				},
				Response = new DrawableText
				{
					Text = " <- ",
					SpriteFont = spriteFont,
					Position = null
				},
			};

			var runtimeDrawService = this.Game.Services.GetService<IRuntimeDrawService>();
			runtimeDrawService.AddOverlaidDrawable(1, Console.ConsoleBackGround);
			runtimeDrawService.AddOverlaidDrawable(1, Console.ConsoleTextArea);
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
			this.Console.ActiveConsoleLine = null;
			var runtimeDrawService = this.Game.Services.GetService<IRuntimeDrawService>();
			runtimeDrawService.RemoveOverlaidDrawable(1, this.Console.ConsoleBackGround);
			runtimeDrawService.RemoveOverlaidDrawable(1, this.Console.ConsoleTextArea);
		}
	}
}

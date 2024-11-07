using DiscModels.Engine.Drawing;
using Engine.Drawing.Services.Contracts;
using Engine.Physics.Models;
using Engine.RunTime.Services.Contracts;
using Engine.UserInterface.Models;
using Engine.UserInterface.Services.Contracts;
using Microsoft.Xna.Framework;

namespace Engine.UserInterface.Services
{
	/// <summary>
	/// Represents a cursor service.
	/// </summary>
	public class CursorService(GameServiceContainer gameServices) : ICursorService
	{
		private readonly GameServiceContainer _gameServices = gameServices;

		/// <summary>
		/// Gets a value indicating whether a active text line exists.
		/// </summary>
		public bool ActiveTextLineExists { get => this.TextCursor.TextLine == null; }

		/// <summary>
		/// Gets or sets the text cursor.
		/// </summary>
		public TextCursor TextCursor { get; private set; }

		/// <summary>
		/// Selects the text line.
		/// </summary>
		/// <param name="textLine">The text line.</param>
		public void SelectTextLine(TextLine textLine)
		{
			if (this.TextCursor.TextLine == textLine)
			{
				return;
			}

			var textLineService = this._gameServices.GetService<TextLineService>();

			if (null == this.TextCursor.Sprite)
			{ 
				textLineService.UpdateTextLineSprite(textLine);
			}

			textLineService.MoveTextLineViewArea(textLine, textLine.Text.Length);

			var runtimeDrawService = this._gameServices.GetService<IRuntimeDrawService>();
			this.TextCursor.TextLine = textLine;
			this.TextCursor.CursorLineIndex = textLine.Text.Length;
			runtimeDrawService.AddOverlaidDrawable(0, this.TextCursor);
		}

		/// <summary>
		/// Unselects the text line.
		/// </summary>
		public void UnselectTextLine()
		{ 
			this.TextCursor.TextLine = null;
		}

		/// <summary>
		/// Updates the selected text line.
		/// </summary>
		/// <param name="newText">The new text.</param>
		public void UpdateSelectedTextLine(string newText)
		{
			if (false == this.ActiveTextLineExists)
			{
				return;
			}

			if (newText == this.TextCursor.TextLine.Text)
			{
				return;
			}

			var textLineService = this._gameServices.GetService<TextLineService>();
			//textLineService.UpdateTextLineSprite(this.TextCursor.TextLine, newText);
			this.UpdateSelectedTextLine(newText);
		}

		/// <summary>
		/// Inserts text into the selected text line.
		/// </summary>
		/// <param name="insertText">The insert text.</param>
		/// <param name="gameTime">The game time.</param>
		public void InsertTextIntoSelectedTextLine(string insertText, GameTime gameTime)
		{
			if (false == this.ActiveTextLineExists)
			{
				return;
			}

			if (null == this.TextCursor.TextLine.Text)
			{
				this.TextCursor.TextLine.Text = null;
			}

			this.TextCursor.CursorLineIndex += insertText.Length;
			this.TextCursor.Animation.FrameStartTime = gameTime.TotalGameTime.TotalMilliseconds;
			var newText = this.TextCursor.TextLine.Text.Insert(this.TextCursor.CursorLineIndex, insertText);
			this.UpdateSelectedTextLine(newText);
		}

		/// <summary>
		/// Deletes text from the selected text line.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		public void DeleteTextFromSelectedTextLine(GameTime gameTime)
		{
			if (false == this.ActiveTextLineExists)
			{
				return;
			}

			if (null == this.TextCursor.TextLine.Text)
			{
				return;
			}

			if (this.TextCursor.CursorLineIndex < 0 || this.TextCursor.CursorLineIndex >= this.TextCursor.TextLine.Text.Length)
			{
				return;
			}

			this.TextCursor.CursorLineIndex--;
			this.TextCursor.Animation.FrameStartTime = gameTime.TotalGameTime.TotalMilliseconds;
			var newText = this.TextCursor.TextLine.Text.Remove(this.TextCursor.CursorLineIndex, 1);
			this.UpdateSelectedTextLine(newText);
		}

		/// <summary>
		/// Loads the content.
		/// </summary>
		public void LoadContent()
		{
			var animationService = this._gameServices.GetService<IAnimationService>();

			// load the text cursor
			var animationModel = new AnimationModel
			{
				CurrentFrameIndex = 0,
				FrameDuration = 750,
				Frames =
				[
					new SpriteModel
					{
						SpritesheetBox = new Rectangle
						{
							X= 0,
							Y= 0,
							Width = 1,
							Height = 1080
						},
						SpritesheetName = "white"
					},
					new SpriteModel
					{
						SpritesheetBox = new Rectangle
						{
							X= 0,
							Y= 0,
							Width = 1,
							Height = 1080
						},
						SpritesheetName = "empty"
					}
				]
			};

			var animation = animationService.GetAnimation(animationModel);
			this.TextCursor = new TextCursor
			{
				Animation = animation,
				CursorLineIndex = 0,
				TextLine = null,
				Position = new Position
				{
					Coordinates = new Vector2()
				}
			};
		}

		/// <summary>
		/// Updates the text cursor position.
		/// </summary>
		private void UpdateTextCursorPosition()
		{
			if (false == this.ActiveTextLineExists)
			{
				return;
			}

			this.TextCursor.Position ??= new Position
			{
				Coordinates = new Vector2()
			};

			this.TextCursor.Position.X = this.TextCursor.TextLine.Position.X + this.TextCursor.TextLine.TextBuffer.X + this.TextCursor.TextLine.Font.MeasureString(this.TextCursor.TextLine.Text[..(this.TextCursor.CursorLineIndex + 1)]).X;
			this.TextCursor.Position.Y = this.TextCursor.TextLine.Position.Y + this.TextCursor.TextLine.TextBuffer.Y;
		}
	}
}

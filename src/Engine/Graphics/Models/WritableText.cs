using Engine.Controls.Typing;
using Engine.Controls.Typing.Models;
using Engine.Controls.Typing.Models.Contracts;
using Engine.Physics.Models;
using Engine.RunTime.Services.Contracts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Engine.Graphics.Models
{
	/// <summary>
	/// Represents writable text.
	/// </summary>
	sealed public class WritableText : SimpleText, IHaveATextHighlightingState
	{
		/// <summary>
		/// Gets or sets a value indicating whether the text is being edited.
		/// </summary>
		required public bool TextIsBeingEdited { get; set; }

		/// <summary>
		/// Gets or sets the text highlighting state.
		/// </summary>
		required public TextHighlightingState TextHighlightingState { get; set; }

		/// <summary>
		/// Gets or sets the text cursor.
		/// </summary>
		required public TextCursor TextCursor { get; set; }

		/// <summary>
		/// Updates the text.
		/// </summary>
		/// <param name="freshKeys">The fresh keys.</param>
		/// <param name="pressedKeys">The pressed keys.</param>
		/// <param name="conformText">A value indicating whether to conform the text.</param>
		/// <returns>The dimensions of the next text.</returns>
		public void UpdateText(List<Keys> freshKeys, List<ElaspedTimeExtender<Keys>> pressedKeys, bool conformText = true)
		{
			var textEditingState = new TextEditingState
			{
				MaxLineCharacterCount = this.MaxLineCharacterCount,
				MaxLinesCount = this.MaxLinesCount,
				TextHighlightingState = this.TextHighlightingState,
				TextLines = [.. this.TextLines],
			};
			var typingResult = KeyboardTyping.ModifyTextFromKeys(textEditingState, this.TextCursor, freshKeys, pressedKeys);			
			this.TextLines = [.. typingResult.TextLines];
			this.TextHighlightingState = typingResult.TextHighlightingState;
		}

		/// <summary>
		/// Writes the sub writable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		/// <param name="coordinates">The coordinates.</param>
		/// <param name="offset">The offset.</param>
		override public void Write(GameTime gameTime, GameServiceContainer gameServices, Vector2 coordinates, Vector2 offset = default)
		{
			if (0 == this.TextLines.Count)
				return;

			var writingService = gameServices.GetService<IWritingService>();
			var textOffset = coordinates + offset;

			foreach (var textLine in this.TextLines)
			{
				writingService.Draw(this.Font, textLine, textOffset, this.TextColor);
				var lineDimensions = this.Font.MeasureString(textLine);
				textOffset.Y += lineDimensions.Y;
			}

			if (true == this.TextIsBeingEdited)
				this.DrawTextEditingState(gameTime, gameServices, coordinates, offset);
		}

		/// <summary>
		/// Draws the text editing state.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		/// <param name="coordinates">The coordinates.</param>
		/// <param name="offset">The offset.</param>
		private void DrawTextEditingState(GameTime gameTime, GameServiceContainer gameServices, Vector2 coordinates, Vector2 offset = default)
		{
			var textDimensions = this.GetTextDimensions();
			var textEditingStateOffset = offset + new Vector2
			{
				X = textDimensions.X,
				Y = (textDimensions.Y - this.TextCursor.Area.Height) / 2
			};
			var cursorText = this.TextLines[this.TextCursor.Position.Line][..this.TextCursor.Position.Index];
			var cursorTextOffset = this.GetTextDimensions(cursorText, includeFontHeightWhenEmpty: false);
			textEditingStateOffset.X += cursorTextOffset.X;
			this.TextCursor.Draw(gameTime, gameServices, coordinates, Color.White, textEditingStateOffset);
			//this.TextHighlightingState.Draw(gameTime, gameServices, coordinates, Color.White, textEditingStateOffset);
		}
	}
}

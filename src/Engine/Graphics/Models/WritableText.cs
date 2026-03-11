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
		/// Gets the start anchor.
		/// </summary>
		required public TextPosition StartAnchor { get; set; }

		/// <summary>
		/// Gets the end anchor.
		/// </summary>
		required public TextPosition EndAnchor { get; set; }

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
			var textConstraints = new TextConstraints
			{
				MaxLineCharacterCount = this.MaxLineCharacterCount,
				MaxLinesCount = this.MaxLinesCount,
				TextHighlightingState = this.TextHighlightingState
			};
			var textEditor = new TextEditor(this.TextLines);
			var typingResult = textEditor.ModifyTextLines(textConstraints, this.TextCursor.Position, freshKeys, pressedKeys);
			this.TextHighlightingState = typingResult.TextHighlightingState;
			this.StartAnchor = typingResult.StartAnchor;
			this.EndAnchor = typingResult.EndAnchor;
			this.TextCursor.Position = typingResult.TextCursorPosition;
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
			var drawingService = gameServices.GetService<IDrawingService>();
			var textOffset = coordinates + offset;

			for (int i = 0; i < this.TextLines.Count; i++)
			{
				var textLine = this.TextLines[i];
				writingService.Draw(this.Font, textLine.Text, textOffset, this.TextColor);
				var lineDimensions = this.GetTextDimensions(textLine.Text, includeFontHeightWhenEmpty: true);

				if (true == this.TextHighlightingState.IsHighlighting)
				{
					var highlightResult = TextLineHasHighlighting(this.StartAnchor, this.EndAnchor, i);

					if (TextHighlightResultType.None != highlightResult)
					{
						var highlightRectangle = this.GetHighlightRectangle(textLine.Text, textOffset, this.StartAnchor, this.EndAnchor, highlightResult);
						drawingService.DrawRectangle(highlightRectangle, this.TextHighlightingState.TextHighlightColor);
					}
				}

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
			var cursorText = this.TextLines[this.TextCursor.Position.Line].Text[..this.TextCursor.Position.Index];
			var cursorTextOffset = this.GetTextDimensions(cursorText, includeFontHeightWhenEmpty: true);
			var lineOffsetHeight = this.GetLineOffsetHeight(this.TextCursor.Position.Line);
			var textEditingStateOffset = offset + new Vector2
			{
				X = cursorTextOffset.X,
				Y = lineOffsetHeight + ((cursorTextOffset.Y - this.TextCursor.Area.Height) / 2)
			};
			this.TextCursor.Draw(gameTime, gameServices, coordinates, this.TextCursor.Color, textEditingStateOffset);
		}

		/// <summary>
		/// Gets the highlighted rectangle.
		/// </summary>
		/// <param name="textLine">The text line.</param>
		/// <param name="offset">The offset.</param>
		/// <param name="startAnchor">The start anchor.</param>
		/// <param name="endAnchor">The end anchor.</param>
		/// <param name="highlightResult">The highlight result.</param>
		/// <returns>The highlight rectangle.</returns>
		private Rectangle GetHighlightRectangle(string textLine, Vector2 offset, TextPosition startAnchor, TextPosition endAnchor, TextHighlightResultType highlightResult)
		{
			Rectangle result;

			if (TextHighlightResultType.StartLine == highlightResult)
			{
				var leftDimensions = this.GetTextDimensions(textLine[..startAnchor.Index]);
				var textLength = startAnchor.Line == endAnchor.Line ?
					endAnchor.Index - startAnchor.Index :
					textLine.Length - startAnchor.Index;
				var highlightedText = textLine.Substring(startAnchor.Index, textLength);
				var textDimensions = this.GetTextDimensions(highlightedText, includeFontHeightWhenEmpty: true);
				result = new Rectangle
				{
					X = (int)(offset.X + leftDimensions.X),
					Y = (int)offset.Y,
					Width = (int)textDimensions.X,
					Height = (int)textDimensions.Y,
				};
			}
			else if (TextHighlightResultType.EndLine == highlightResult)
			{
				var textLength = startAnchor.Line == endAnchor.Line ?
					endAnchor.Index - startAnchor.Index :
					endAnchor.Index;
				var highlightedText = textLine[..textLength];
				var textDimensions = this.GetTextDimensions(highlightedText, includeFontHeightWhenEmpty: true);
				result = new Rectangle
				{
					X = (int)offset.X,
					Y = (int)offset.Y,
					Width = (int)textDimensions.X,
					Height = (int)textDimensions.Y,
				};
			}
			else
			{
				var textDimensions = this.GetTextDimensions(textLine, includeFontHeightWhenEmpty: true);
				result = new Rectangle
				{
					X = (int)offset.X,
					Y = (int)offset.Y,
					Width = (int)textDimensions.X,
					Height = (int)textDimensions.Y,
				};
			}

			return result;
		}

		/// <summary>
		/// Determines if the text line has highlighting.
		/// </summary>
		/// <param name="startAnchor">The start anchor.</param>
		/// <param name="endAnchor">The end anchor.</param>
		/// <param name="line">The line.</param>
		/// <returns>A value indicating whether the text line has highlighting.</returns>
		static private TextHighlightResultType TextLineHasHighlighting(TextPosition startAnchor, TextPosition endAnchor, int line)
		{
			if ((startAnchor.Line <= line) &&
			    (endAnchor.Line >= line))
			{
				if (line == startAnchor.Line)
					return TextHighlightResultType.StartLine;
				else if (line == endAnchor.Line)
					return TextHighlightResultType.EndLine;

				return TextHighlightResultType.WholeLine;
			}

			return TextHighlightResultType.None;
		}

		/// <summary>
		/// Represents a text highlight result type.
		/// </summary>
		private enum TextHighlightResultType
		{ 
			/// <summary>
			/// The none result.
			/// </summary>
			None = 0,

			/// <summary>
			/// The start line result.
			/// </summary>
			StartLine = 1,

			/// <summary>
			/// The end line result.
			/// </summary>
			EndLine = 2,

			/// <summary>
			/// The whole line result.
			/// </summary>
			WholeLine = 3
		}
	}
}

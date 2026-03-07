using Engine.Controls.Typing.Models;
using Engine.Physics.Models;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Controls.Typing
{
	/// <summary>
	/// Provides methods typing with the keyboard.
	/// </summary>
	static public class KeyboardTyping
	{
		/// <summary>
		/// The short press time.
		/// </summary>
		static readonly public int ShortPressTime = 500;

		/// <summary>
		/// The long press time.
		/// </summary>
		static readonly public int LongPressTime = 750;

		/// <summary>
		/// The remove text keys.`
		/// </summary>
		static readonly List<Keys> RemoveTextKeys = [Keys.Back, Keys.Delete];

		/// <summary>
		/// The shift keys.
		/// </summary>
		static readonly List<Keys> ShiftKeys = [Keys.LeftShift, Keys.RightShift];

		/// <summary>
		/// The new line keys.
		/// </summary>
		static readonly List<Keys> NewLineKeys = [Keys.Enter];

		/// <summary>
		/// Formats the string for drawing.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <returns>The formatted text.</returns>
		static public string FormatForDrawString(string text)
		{
			var result = text.Replace("\t", "    ");

			return result;
		}

		/// <summary>
		/// Modifies the text from the keys.
		/// </summary>
		/// <param name="textEditState">The text edit state.</param>
		/// <param name="textCursor">The text cursor.</param>
		/// <param name="freshKeys">The fresh keys.</param>
		/// <param name="pressedKeys">The pressed keys.</param>
		/// <returns>The text modified from the keys.</returns>
		static public TextEditingState ModifyTextFromKeys(TextEditingState textEditState, TextCursor textCursor, List<Keys> freshKeys, List<ElaspedTimeExtender<Keys>> pressedKeys)
		{
			textEditState = HandleTextCursorMovement(textEditState, textCursor, freshKeys, pressedKeys);
			var removeText = AnyKeyIsActive([.. RemoveTextKeys], freshKeys, pressedKeys);
			var textCursorPosition = textCursor.Position;
			var textHighlightingState = textEditState.TextHighlightingState;
			TextPosition[] anchors = [];
			TextPosition startAnchor = default;
			TextPosition endAnchor = default;

			if (true == textEditState.TextHighlightingState.IsHighlighting)
			{
				anchors = [textEditState.TextHighlightingState.TextAnchor.Value, textCursor.Position];
				anchors = [.. anchors.OrderBy(e => e.Line).ThenBy(e => e.Index)];
				startAnchor = anchors.First();
				endAnchor = anchors.Last();
			}

			if (true == removeText)
			{
				if (true == textEditState.TextHighlightingState.IsHighlighting)
				{
					textEditState.TextLines = RemoveHighlightedText(startAnchor, endAnchor, textEditState, textCursor);
					textHighlightingState.TextAnchor = null;
					textCursorPosition = startAnchor;
				}
				else if (0 != textCursorPosition.Index)
				{
					textEditState.TextLines[textCursorPosition.Line] = textEditState.TextLines[textCursorPosition.Line].Remove(textCursorPosition.Index - 1, 1);
					textCursorPosition.Index--;
				}
			}

			textCursor.Position = textCursorPosition;
			textEditState.TextHighlightingState = textHighlightingState;
			var newLine = AnyKeyIsActive([.. NewLineKeys], freshKeys, pressedKeys);

			if ((true == newLine) &&
				((false == textEditState.MaxLinesCount.HasValue) ||
				 (textEditState.TextLines.Length < textEditState.MaxLinesCount)))
			{
				if (true == textEditState.TextHighlightingState.IsHighlighting)
				{
					textEditState.TextLines = RemoveHighlightedText(startAnchor, endAnchor, textEditState, textCursor);
					textHighlightingState.TextAnchor = null;
					textCursorPosition = startAnchor;
				}

				var textLines = textEditState.TextLines.ToList();
				textLines.Insert(textCursorPosition.Line + 1, "");
				textEditState.TextLines = [.. textLines];
				textCursorPosition.Line++;
				textCursorPosition.Index = 0;
			}

			var textFromKeys = GetTextFromKeys(freshKeys, pressedKeys);
			textCursor.Position = textCursorPosition;
			textEditState.TextHighlightingState = textHighlightingState;

			if (false == string.IsNullOrEmpty(textFromKeys))
			{
				if (true == textEditState.TextHighlightingState.IsHighlighting)
				{
					textEditState.TextLines = RemoveHighlightedText(startAnchor, endAnchor, textEditState, textCursor);
					textHighlightingState.TextAnchor = null; 
					textCursorPosition = startAnchor;
				}

				var existingLeft = textEditState.TextLines[textCursorPosition.Line][..textCursorPosition.Index];
				var existingRight = textEditState.TextLines[textCursorPosition.Line][textCursorPosition.Index..];
				textEditState.TextLines[textCursorPosition.Line] = existingLeft + textFromKeys + existingRight;
				textCursorPosition.Index += textFromKeys.Length;
			}

			textCursor.Position = textCursorPosition;
			textEditState.TextHighlightingState = textHighlightingState;

			return textEditState;
		}

		/// <summary>
		/// Handles the text cursor movement.
		/// </summary>
		/// <param name="textEditState">The text edit state.</param>
		/// <param name="textCursor">The text cursor.</param>
		/// <param name="freshKeys">The fresh keys.</param>
		/// <param name="pressedKeys">The pressed keys.</param>
		/// <returns>The new text editing state.</returns>
		static public TextEditingState HandleTextCursorMovement(TextEditingState textEditState, TextCursor textCursor, List<Keys> freshKeys, List<ElaspedTimeExtender<Keys>> pressedKeys)
		{
			if (null == textCursor?.Position)
				return textEditState;

			var leftActive = KeyIsActive(Keys.Left, freshKeys, pressedKeys);
			var rightActive = KeyIsActive(Keys.Right, freshKeys, pressedKeys);
			var upActive = KeyIsActive(Keys.Up, freshKeys, pressedKeys);
			var downActive = KeyIsActive(Keys.Down, freshKeys, pressedKeys);

			if ((false == leftActive) &&
				(false == rightActive) &&
				(false == upActive) &&
				(false == downActive))
				return textEditState;

			var highlightKeyActive = pressedKeys.Any(e => ShiftKeys.Contains(e.Subject));
			var textHighlightingState = textEditState.TextHighlightingState;

			if (true == highlightKeyActive)
				textHighlightingState.TextAnchor ??= textCursor.Position;
			else
				textHighlightingState.TextAnchor = null;

			textEditState.TextHighlightingState = textHighlightingState;
			var textCursorPosition = textCursor.Position;

			if ((true == leftActive) &&
				(false == rightActive))
			{
				if ((0 == textCursorPosition.Index) &&
					(0 != textCursorPosition.Line))
				{
					textCursorPosition.Line--;
					textCursorPosition.Index = textEditState.TextLines[textCursorPosition.Line].Length;
				}
				else if (0 != textCursorPosition.Index)
					textCursorPosition.Index--;
			}
			else if ((true == rightActive) &&
					 (false == leftActive))
			{
				if ((textCursorPosition.Index == textEditState.TextLines[textCursorPosition.Line].Length) &&
					(textCursorPosition.Line < textEditState.TextLines.Length - 1))
				{
					textCursorPosition.Line++;
					textCursorPosition.Index = 0;
				}
				else if (textCursorPosition.Index < textEditState.TextLines[textCursorPosition.Line].Length)
					textCursorPosition.Index++;
			}
			else if ((true == upActive) &&
					 (false == downActive))
			{
				if (0 != textCursorPosition.Line)
					textCursorPosition.Line--;

				if (textCursorPosition.Index > textEditState.TextLines[textCursorPosition.Line].Length)
					textCursorPosition.Index = textEditState.TextLines[textCursorPosition.Line].Length;
			}
			else if ((true == downActive) &&
					 (false == upActive))
			{
				if (textCursorPosition.Line < textEditState.TextLines.Length - 1)
					textCursorPosition.Line++;

				if (textCursorPosition.Index > textEditState.TextLines[textCursorPosition.Line].Length)
					textCursorPosition.Index = textEditState.TextLines[textCursorPosition.Line].Length;
			}

			textCursor.Position = textCursorPosition;

			return textEditState;
		}

		/// <summary>
		/// Removes the highlighted text.
		/// </summary>
		/// <param name="startAnchor">The start anchor.</param>
		/// <param name="endAnchor">The end anchor.</param>
		/// <param name="textEditState">The text edit state.</param>
		/// <param name="textCursor">The text cursor.</param>
		/// <returns>The new text line.</returns>
		static public string[] RemoveHighlightedText(TextPosition startAnchor, TextPosition endAnchor, TextEditingState textEditState, TextCursor textCursor)
		{
			List<string> resultLines = [];
			resultLines.AddRange(textEditState.TextLines.Take(startAnchor.Line));
			var startLineText = textEditState.TextLines[startAnchor.Line][..startAnchor.Index];
			var endLineText = textEditState.TextLines[endAnchor.Line][endAnchor.Index..];
			resultLines.Add(startLineText + endLineText);

			if (endAnchor.Line + 1 < textEditState.TextLines.Length)
				resultLines.AddRange(textEditState.TextLines.Skip(endAnchor.Line + 1));

			resultLines.TrimExcess();

			return [.. resultLines];
		}

		/// <summary>
		/// Determines if any of the keys are active.
		/// </summary>
		/// <param name="keys">The keys.</param>
		/// <param name="freshKeys">The fresh keys.</param>
		/// <param name="pressedKeys">The pressed keys.</param>
		/// <returns>A value indicating whether any of the keys are active.</returns>
		static public bool AnyKeyIsActive(Keys[] keys, List<Keys> freshKeys, List<ElaspedTimeExtender<Keys>> pressedKeys)
		{
			foreach (var key in keys ?? [])
				if (true == KeyIsActive(key, freshKeys, pressedKeys))
					return true;

			return false;
		}

		/// <summary>
		/// Determines if the key is active.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="freshKeys">The fresh keys.</param>
		/// <param name="pressedKeys">The pressed keys.</param>
		/// <returns>A value indicating whether the key is active.</returns>
		static public bool KeyIsActive(Keys key, List<Keys> freshKeys, List<ElaspedTimeExtender<Keys>> pressedKeys)
		{
			var result = true == freshKeys?.Contains(key) ||
						 true == pressedKeys?.Any(e => (e.Subject == key) &&
												 (e.ElaspedTime > LongPressTime) &&
												 (e.ElaspedTime >= ShortPressTime));

			return result;
		}

		/// <summary>
		/// Gets the text from the keys.
		/// </summary>
		/// <param name="freshKeys">The fresh keys.</param>
		/// <param name="pressedKeys">The pressed keys.</param>
		/// <returns>A matching string for the keys.</returns>
		static public string GetTextFromKeys(List<Keys> freshKeys, List<ElaspedTimeExtender<Keys>> pressedKeys)
		{
			var textKeys = freshKeys.ToList();
			var heldKeys = pressedKeys.Where(e => (e.ElaspedTime > LongPressTime) &&
												  (e.ElaspedTime >= ShortPressTime))
									  .Select(e => e.Subject)
									  .ToArray();
			textKeys.AddRange(heldKeys);
			var isShiftPressed = pressedKeys.Any(e => ShiftKeys.Contains(e.Subject));
			var rawResult = ToString(textKeys, isShiftPressed);
			var result = FormatForDrawString(rawResult);

			return result;
		}

		/// <summary>
		/// Gets the keys as a string.
		/// </summary>
		/// <param name="keys">The keys.</param>
		/// <param name="isShiftPressed">A value indicating whether the shift key is pressed.</param>
		/// <returns>The matching string for the keys.</returns>
		static public string ToString(IList<Keys> keys, bool isShiftPressed)
		{
			var text = new StringBuilder();

			foreach (var key in keys)
			{
				var keyChar = ToChar(key, isShiftPressed);

				if (false == keyChar.HasValue)
					continue;

				text.Append(keyChar.Value);
			}

			return text.ToString();
		}

		/// <summary>
		/// Gets the matching char for the key.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="isShiftPressed">A value indicating whether the shift key is pressed.</param>
		/// <returns>The matching char for the key.</returns>
		static public char? ToChar(Keys key, bool isShiftPressed)
		{
			if (key == Keys.A) return isShiftPressed ? 'A' : 'a';
			if (key == Keys.B) return isShiftPressed ? 'B' : 'b';
			if (key == Keys.C) return isShiftPressed ? 'C' : 'c';
			if (key == Keys.D) return isShiftPressed ? 'D' : 'd';
			if (key == Keys.E) return isShiftPressed ? 'E' : 'e';
			if (key == Keys.F) return isShiftPressed ? 'F' : 'f';
			if (key == Keys.G) return isShiftPressed ? 'G' : 'g';
			if (key == Keys.H) return isShiftPressed ? 'H' : 'h';
			if (key == Keys.I) return isShiftPressed ? 'I' : 'i';
			if (key == Keys.J) return isShiftPressed ? 'J' : 'j';
			if (key == Keys.K) return isShiftPressed ? 'K' : 'k';
			if (key == Keys.L) return isShiftPressed ? 'L' : 'l';
			if (key == Keys.M) return isShiftPressed ? 'M' : 'm';
			if (key == Keys.N) return isShiftPressed ? 'N' : 'n';
			if (key == Keys.O) return isShiftPressed ? 'O' : 'o';
			if (key == Keys.P) return isShiftPressed ? 'P' : 'p';
			if (key == Keys.Q) return isShiftPressed ? 'Q' : 'q';
			if (key == Keys.R) return isShiftPressed ? 'R' : 'r';
			if (key == Keys.S) return isShiftPressed ? 'S' : 's';
			if (key == Keys.T) return isShiftPressed ? 'T' : 't';
			if (key == Keys.U) return isShiftPressed ? 'U' : 'u';
			if (key == Keys.V) return isShiftPressed ? 'V' : 'v';
			if (key == Keys.W) return isShiftPressed ? 'W' : 'w';
			if (key == Keys.X) return isShiftPressed ? 'X' : 'x';
			if (key == Keys.Y) return isShiftPressed ? 'Y' : 'y';
			if (key == Keys.Z) return isShiftPressed ? 'Z' : 'z';
			if (((key == Keys.D0) && !isShiftPressed) || (key == Keys.NumPad0)) return '0';
			if (((key == Keys.D1) && !isShiftPressed) || (key == Keys.NumPad1)) return '1';
			if (((key == Keys.D2) && !isShiftPressed) || (key == Keys.NumPad2)) return '2';
			if (((key == Keys.D3) && !isShiftPressed) || (key == Keys.NumPad3)) return '3';
			if (((key == Keys.D4) && !isShiftPressed) || (key == Keys.NumPad4)) return '4';
			if (((key == Keys.D5) && !isShiftPressed) || (key == Keys.NumPad5)) return '5';
			if (((key == Keys.D6) && !isShiftPressed) || (key == Keys.NumPad6)) return '6';
			if (((key == Keys.D7) && !isShiftPressed) || (key == Keys.NumPad7)) return '7';
			if (((key == Keys.D8) && !isShiftPressed) || (key == Keys.NumPad8)) return '8';
			if (((key == Keys.D9) && !isShiftPressed) || (key == Keys.NumPad9)) return '9';
			if ((key == Keys.D0) && isShiftPressed) return ')';
			if ((key == Keys.D1) && isShiftPressed) return '!';
			if ((key == Keys.D2) && isShiftPressed) return '@';
			if ((key == Keys.D3) && isShiftPressed) return '#';
			if ((key == Keys.D4) && isShiftPressed) return '$';
			if ((key == Keys.D5) && isShiftPressed) return '%';
			if ((key == Keys.D6) && isShiftPressed) return '^';
			if ((key == Keys.D7) && isShiftPressed) return '&';
			if ((key == Keys.D8) && isShiftPressed) return '*';
			if ((key == Keys.D9) && isShiftPressed) return '(';
			if (key == Keys.Space) return ' ';
			if (key == Keys.Tab) return '\t';
			//if (key == Keys.Enter) return (char)13;
			//if (key == Keys.Back) return (char)8;
			if (key == Keys.Add) return '+';
			if (key == Keys.Decimal) return '.';
			if (key == Keys.Divide) return '/';
			if (key == Keys.Multiply) return '*';
			if (key == Keys.OemBackslash) return '\\';
			if ((key == Keys.OemComma) && !isShiftPressed) return ',';
			if ((key == Keys.OemComma) && isShiftPressed) return '<';
			if ((key == Keys.OemOpenBrackets) && !isShiftPressed) return '[';
			if ((key == Keys.OemOpenBrackets) && isShiftPressed) return '{';
			if ((key == Keys.OemCloseBrackets) && !isShiftPressed) return ']';
			if ((key == Keys.OemCloseBrackets) && isShiftPressed) return '}';
			if ((key == Keys.OemPeriod) && !isShiftPressed) return '.';
			if ((key == Keys.OemPeriod) && isShiftPressed) return '>';
			if ((key == Keys.OemPipe) && !isShiftPressed) return '\\';
			if ((key == Keys.OemPipe) && isShiftPressed) return '|';
			if ((key == Keys.OemPlus) && !isShiftPressed) return '=';
			if ((key == Keys.OemPlus) && isShiftPressed) return '+';
			if ((key == Keys.OemMinus) && !isShiftPressed) return '-';
			if ((key == Keys.OemMinus) && isShiftPressed) return '_';
			if ((key == Keys.OemQuestion) && !isShiftPressed) return '/';
			if ((key == Keys.OemQuestion) && isShiftPressed) return '?';
			if ((key == Keys.OemQuotes) && !isShiftPressed) return '\'';
			if ((key == Keys.OemQuotes) && isShiftPressed) return '"';
			if ((key == Keys.OemSemicolon) && !isShiftPressed) return ';';
			if ((key == Keys.OemSemicolon) && isShiftPressed) return ':';
			if ((key == Keys.OemTilde) && !isShiftPressed) return '`';
			if ((key == Keys.OemTilde) && isShiftPressed) return '~';
			if (key == Keys.Subtract) return '-';

			return null;
		}
	}
}

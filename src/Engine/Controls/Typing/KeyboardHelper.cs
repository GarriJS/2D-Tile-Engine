using Engine.Controls.Typing.Models;
using Engine.Physics.Models;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Controls.Typing
{
	/// <summary>
	/// Provides methods for typing with the keyboard.
	/// </summary>
	static public class KeyboardHelper
	{
		/// <summary>
		/// The long press time.
		/// </summary>
		static readonly public int LongPressTime = 500;

		/// <summary>
		/// The remove text keys.`
		/// </summary>
		static readonly public Keys[] RemoveTextKeys = [Keys.Back, Keys.Delete];

		/// <summary>
		/// The shift keys.
		/// </summary>
		static readonly public Keys[] ShiftKeys = [Keys.LeftShift, Keys.RightShift];

		/// <summary>
		/// The new line keys.
		/// </summary>
		static readonly public Keys[] NewLineKeys = [Keys.Enter];

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
		/// Gets the text anchors.
		/// </summary>
		/// <param name="a">The text position.</param>
		/// <param name="b">The nullable text position.</param>
		/// <returns>The text anchors.</returns>
		static public (TextPosition start, TextPosition end) GetAnchors(TextPosition a, TextPosition? b)
		{
			if (b is null)
				return (a, a);

			var anchor = b.Value;

			return (anchor.Line < a.Line ||
				   (anchor.Line == a.Line && anchor.Index <= a.Index))
				? (anchor, a)
				: (a, anchor);
		}

		/// <summary>
		/// Determines if any of the keys are active.
		/// </summary>
		/// <param name="keys">The keys.</param>
		/// <param name="freshKeys">The fresh keys.</param>
		/// <param name="pressedKeys">The pressed keys.</param>
		/// <param name="ignoreTime">A value indicating whether to ignore the time the key has been pressed.</param>
		/// <returns>A value indicating whether any of the keys are active.</returns>
		static public bool AnyKeyIsActive(Keys[] keys, List<Keys> freshKeys, List<ElapsedTimeExtender<Keys>> pressedKeys, bool ignoreTime = false)
		{
			foreach (var key in keys)
				if (true == KeyIsActive(key, freshKeys, pressedKeys, ignoreTime))
					return true;

			return false;
		}

		/// <summary>
		/// Determines if the key is active.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="freshKeys">The fresh keys.</param>
		/// <param name="pressedKeys">The pressed keys.</param>
		/// <param name="ignoreTime">A value indicating whether to ignore the time the key has been pressed.</param>
		/// <returns>A value indicating whether the key is active.</returns>
		static public bool KeyIsActive(Keys key, List<Keys> freshKeys, List<ElapsedTimeExtender<Keys>> pressedKeys, bool ignoreTime = false)
		{
			var result = (true == freshKeys?.Contains(key)) ||
						 (true == pressedKeys?.Any(e => (e.Subject == key) &&
														((true == ignoreTime) ||
														 (e.ElaspedTime > LongPressTime))));

			return result;
		}

		/// <summary>
		/// Gets the text from the keys.
		/// </summary>
		/// <param name="freshKeys">The fresh keys.</param>
		/// <param name="pressedKeys">The pressed keys.</param>
		/// <returns>A matching string for the keys.</returns>
		static public string GetTextFromKeys(List<Keys> freshKeys, List<ElapsedTimeExtender<Keys>> pressedKeys)
		{
			var textKeys = pressedKeys.Where(e => KeyIsActive(e.Subject, freshKeys, pressedKeys))
									  .Select(e => e.Subject)
									  .ToArray();
			var isShiftPressed = IsShiftPressed(pressedKeys);
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
		/// Determines whether any shift key is being pressed.
		/// </summary>
		/// <param name="pressedKeys">The pressed keys.</param>
		/// <returns>A value indicating whether a shift key is pressed.</returns>
		static public bool IsShiftPressed(IList<ElapsedTimeExtender<Keys>> pressedKeys)
		{
			var result = pressedKeys.Any(e => ShiftKeys.Contains(e.Subject));
		
			return result;
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

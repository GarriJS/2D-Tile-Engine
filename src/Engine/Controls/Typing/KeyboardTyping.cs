using Microsoft.Xna.Framework.Input;

namespace Engine.Controls.Typing
{
	/// <summary>
	/// Provides methods typing with the keyboard.
	/// </summary>
	static public class KeyboardTyping
	{
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
			if (key == Keys.Enter) return (char)13;
			if (key == Keys.Back) return (char)8;
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
	}
}

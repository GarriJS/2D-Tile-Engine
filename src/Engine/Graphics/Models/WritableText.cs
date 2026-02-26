using Engine.Controls.Typing;
using Engine.Physics.Models;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Engine.Graphics.Models
{
	/// <summary>
	/// Represents writable text.
	/// </summary>
	sealed public class WritableText : SimpleText
	{
		/// <summary>
		/// Updates the text.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="conformText">A value indicating whether to conform the text.</param>
		/// <returns>The dimensions of the next text.</returns>
		public void UpdateText(string text, bool conformText = true)
		{
			this.Text = text;

			if (true == conformText)
				this.ConformTextToMaxWidth();
		}

		/// <summary>
		/// Updates the text.
		/// </summary>
		/// <param name="freshKeys">The fresh keys.</param>
		/// <param name="pressedKeys">The pressed keys.</param>
		/// <param name="conformText">A value indicating whether to conform the text.</param>
		/// <returns>The dimensions of the next text.</returns>
		public void UpdateText(List<Keys> freshKeys, List<ElaspedTimeExtender<Keys>> pressedKeys, bool conformText = true)
		{
			var newText = KeyboardTyping.ModifyTextFromKeys(this.Text, freshKeys, pressedKeys);
			this.Text = newText;

			if (true == conformText)
				this.ConformTextToMaxWidth();
		}
	}
}

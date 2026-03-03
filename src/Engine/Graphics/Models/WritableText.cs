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
	sealed public class WritableText : SimpleText, IHaveATextEditingState
	{
		/// <summary>
		/// Gets or sets the text editing state.
		/// </summary>
		required public TextEditingState TextEditingState { get; set; }

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
			var typingResult = KeyboardTyping.ModifyTextFromKeys(this.Text, this.TextEditingState, freshKeys, pressedKeys);
			this.Text = typingResult.Text;
			this.TextEditingState = typingResult.TextEditingState;

			if (true == conformText)
				this.ConformTextToMaxWidth();
		}

		/// <summary>
		/// Draws the text cursor.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		/// <param name="coordinates">The coordinates.</param>
		/// <param name="offset">The offset.</param>
		public void DrawTextCursor(GameTime gameTime, GameServiceContainer gameServices, Vector2 coordinates, Vector2 offset = default)
		{
			this.TextEditingState.TypingCursor?.Draw(gameTime, gameServices, coordinates, this.TextEditingState.TypingCursorColor, offset);
		}

		public void DrawHighlightedTextIndicator(GameTime gameTime, GameServiceContainer gameServices, Vector2 coordinates, Vector2 offset = default)
		{
			if (false == this.TextEditingState.IsHighlighting)
				return;

			var drawingService = gameServices.GetService<IDrawingService>();
			var highlightedRectangle = this.TextEditingState.GetHighlightedRectangle(this.Text, this.Font, coordinates, offset);

			if (highlightedRectangle is null)
				return;

			drawingService.DrawRectangle(highlightedRectangle.Value, this.TextEditingState.TextHighlightColor);
		}
	}
}
